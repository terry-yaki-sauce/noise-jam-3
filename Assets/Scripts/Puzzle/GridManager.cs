using System;
using Unity.VisualScripting;
using UnityEngine;
public class GridManager : Singleton<GridManager>
{
  [SerializeField] private GridCursor cursor;
  [SerializeField] private Grid grid;

  [SerializeField] private Transform topLeft, bottomRight;
  private int leftBound { get => grid.WorldToCell(topLeft.position).x; }
  private int topBound { get => grid.WorldToCell(topLeft.position).y; }
  private int rightBound { get => grid.WorldToCell(bottomRight.position).x; }
  private int bottomBound { get => grid.WorldToCell(bottomRight.position).y; }
  [SerializeField] private GridCell gridCellPrefab;

  private GridObject selectedObject;
  public static GridObject SelectedObject => instance.selectedObject;
  private Transform selectedTransform;
  private GridCell[][] cells;

  protected override void Awake()
  {
    base.Awake();

    Vector3Int cursorCell = grid.WorldToCell(cursor.transform.position);
    cursor.SetPosition(new(cursorCell.x, cursorCell.y));
    cursor.gameObject.SetActive(false);

    cells = new GridCell[rightBound - leftBound + 1][];
    for (int i = 0; i < cells.Length; i++)
    {
      cells[i] = new GridCell[topBound - bottomBound + 1];
      for (int j = 0; j < cells[i].Length; j++)
      {
        GridCell cell = Instantiate(gridCellPrefab, gameObject.transform);
        cell.transform.position = grid.GetCellCenterWorld(new(leftBound + i, bottomBound + j));
        cells[i][j] = cell;
      }
    }
  }

  public static void Show() => instance.ShowHelper();
  private void ShowHelper()
  {
    // TODO: keep cursor bound to the screen using the camera bounds

    cursor.gameObject.SetActive(true);
  }
  public static void Hide() => instance.HideHelper();
  private void HideHelper()
  {
    cursor.gameObject.SetActive(false);
  }

  /// <summary>
  /// Called via player input. Checks where the cursor is located, and whether an object is selected or not
  /// </summary>
  public static void ActivateCursor() => instance.ActivateCursorHelper();
  private void ActivateCursorHelper()
  {
    GridCell cursorCell = GetCell(cursor.GridPosition);
    if (!IsHoldingObject)
    {
      TryPickUpObject(cursorCell);
    }
    else
    {
      TryReleaseObject(cursorCell);
    }
  }

  private void TryPickUpObject(GridCell cursorCell)
  {
    if (!cursorCell.IsOccupied)
    {
      Debug.Log("no object in cell");
      // TODO: rejection feedback
      return;
    }

    selectedObject = cursorCell.OccupyingObject;
    selectedTransform = cursorCell.OccupyingTransform;

    cursorCell.OccupyingObject = null;
    cursorCell.OccupyingTransform = null;

    foreach (Transform t in selectedObject.Shape)
    {
      Vector3Int cellPosition = grid.WorldToCell(t.position);
      GridCell cell = GetCell(cellPosition);
      cell.OccupyingObject = null;
      cell.OccupyingTransform = null;

      t.SetParent(selectedObject.transform);
    }
    foreach (Transform t in selectedObject.Shape)
    {
      t.SetParent(selectedTransform);
    }
  }

  private void TryReleaseObject(GridCell cursorCell)
  {
    // theoretically, it should be impossible to place an object in an invalid location since IsValidMovement is always checked
    // if (cellIndex.IsOccupied())
    // {
    //   return;
    // }

    foreach (Transform t in selectedObject.Shape)
    {
      Vector3Int cellPosition = grid.WorldToCell(t.position);
      GridCell cell = GetCell(cellPosition);
      cell.OccupyingObject = selectedObject;
      cell.OccupyingTransform = t;
    }

    selectedObject = null;
    selectedTransform = null;
  }

  /// <summary>
  /// Move the Grid cursor in the direction of <c>dir</c>. Check boundaries and determine if the movement is legal
  /// </summary>
  /// <param name="dir"></param>
  public static void MoveCursor(Vector2Int dir) => instance.MoveCursorHelper(dir);
  private void MoveCursorHelper(Vector2Int dir)
  {
    Vector3Int position3d = grid.WorldToCell(cursor.transform.position);
    Vector2Int position2d = new(position3d.x, position3d.y);
    Vector2Int desiredPosition = position2d + dir;

    // check for any obstacles that prevent movement
    if (selectedObject && !IsValidMovement(selectedObject, dir)) return;

    Vector2Int newPosition = ClampToCameraWorldBounds(desiredPosition);

    // make sure to check whether moving the piece is legal too. It may be prudent to always normalize dir, and force it be in one of the cardinal directions only

    SetCursor(newPosition);
  }

  /// <summary>
  /// Checks whether the given <c>GridObject</c> can be placed in the cell <c>position + dir</c>. NOTE this always assumes dir is either a horizontal or vertical vector of magnitude 1.
  /// </summary>
  /// <param name="selectedObject"></param>
  /// <param name="desiredPosition"></param>
  /// <returns></returns>
  private bool IsValidMovement(GridObject selectedObject, Vector2Int dir)
  {
    if (selectedObject == null) return false;

    foreach (Transform t in selectedObject.Shape)
    {
      Vector3Int gridPosition3 = grid.WorldToCell(t.position);
      Vector2Int gridPosition2 = new(gridPosition3.x, gridPosition3.y);
      if (!IsInBounds(gridPosition2 + dir)) return false;
      if (GetCell(gridPosition2 + dir).IsOccupied) return false;
    }

    return true;
  }

  /// <summary>
  /// Checks whether <c>pos</c> is in the bounds of the grid. checks bounds inclusively.
  /// </summary>
  /// <param name="pos"></param>
  /// <returns></returns>
  private bool IsInBounds(Vector2Int pos)
  {
    return pos.x >= leftBound && pos.x <= rightBound && pos.y <= topBound && pos.y >= bottomBound;
  }

  private Vector2Int ClampToCameraWorldBounds(Vector2Int vector)
  {
    // Clamp the position to be within the grid boundary
    int x = Math.Clamp(vector.x, leftBound, rightBound);
    int y = Math.Clamp(vector.y, bottomBound, topBound);

    // Clamp the position within the camera bounds
    Camera camera = Camera.main;
    float halfHeight = camera.orthographicSize;
    float halfWidth = camera.aspect * halfHeight;

    Vector3 cameraPosition = camera.transform.position;
    float cameraX = cameraPosition.x;
    float cameraY = cameraPosition.y;
    float cameraLeftBound = cameraX - halfWidth;
    float cameraRightBound = cameraX + halfWidth;
    float cameraTopBound = cameraY + halfHeight;
    float cameraBottomBound = cameraY - halfHeight;
    Vector3Int cameraTopLeft = grid.WorldToCell(new(cameraLeftBound, cameraTopBound));
    Vector3Int cameraBottomRight = grid.WorldToCell(new(cameraRightBound, cameraBottomBound));

    x = Math.Clamp(x, cameraTopLeft.x, cameraBottomRight.x);
    y = Math.Clamp(y, cameraBottomRight.y, cameraTopLeft.y);

    Vector2Int clampedPosition = new(x, y);

    return clampedPosition;
  }

  /// <summary>
  /// Set the raw position of the Grid cursor. Does not check the legality of the movement.
  /// </summary>
  /// <param name="position"></param>
  public static void SetCursor(Vector2Int position) => instance.SetCursorHelper(position);
  private void SetCursorHelper(Vector2Int position)
  {
    cursor.SetPosition(position);

    // drag the object along with it
    if (IsHoldingObject)
    {
      selectedTransform.position = cursor.transform.position;
    }
  }

  /// <summary>
  /// Level Start Utility. Adds a grid object to its current cell location
  /// </summary>
  /// <param name="gridObject"></param>
  public static void AddGridObject(GridObject gridObject) => instance.AddGridObjectHelper(gridObject);
  private void AddGridObjectHelper(GridObject gridObject)
  {
    // visually render object in cell center (fixes weird offsets on load)
    Vector3Int gridPos = grid.WorldToCell(gridObject.Shape[0].position);
    gridObject.Shape[0].position = grid.GetCellCenterWorld(gridPos);

    // logically define object location
    foreach (Transform t in gridObject.Shape)
    {
      Vector3Int cellCoord = grid.WorldToCell(t.position);
      InitCellObject(gridObject, t, cellCoord);
    }

  }

  /// <summary>
  /// Automatically translate a position on the grid and return the cell at its corresponding index
  /// </summary>
  /// <param name="position"></param>
  /// <returns></returns>
  private GridCell GetCell(Vector3Int position)
  {
    return cells[position.x - leftBound][position.y + topBound];
  }

  private GridCell GetCell(Vector2Int position)
  {
    return cells[position.x - leftBound][position.y + topBound];
  }

  private GridCell GetCell(int x, int y)
  {
    return cells[x - leftBound][y - topBound];
  }

  private Vector2Int GetCellIndex(Vector3Int position)
  {
    return new(position.x - leftBound, position.y + topBound);
  }

  private Vector2Int GetCellIndex(int x, int y)
  {
    return new(x - leftBound, y + topBound);
  }

  /// <summary>
  /// Only to be used during initialization of game state. places grid objects in the correct dimensions.
  /// </summary>
  /// <param name="gridObject"></param>
  /// <param name="goTransform"></param>
  /// <param name="position"></param>
  private void InitCellObject(GridObject gridObject, Transform goTransform, Vector3Int position)
  {
    // Debug.Log($"{position.x} {position.y}");
    // Debug.Log($"Setting {gridObject.name} to index {position.x - leftBound} {position.y + topBound}");
    GridCell cell = cells[position.x - leftBound][position.y + topBound];
    cell.InitializeGridObject(gridObject,goTransform);
  }

  private void InitCellObject(GridObject gridObject, Transform goTransform, int x, int y)
  {
    // Debug.Log($"No Translate {x},{y}");
    // Debug.Log($"Translate {x - leftBound},{y + topBound}");
    GridCell cell = cells[x - leftBound][y + topBound];
    cell.InitializeGridObject(gridObject,goTransform);
  }

  private bool IsHoldingObject => selectedObject != null;
}