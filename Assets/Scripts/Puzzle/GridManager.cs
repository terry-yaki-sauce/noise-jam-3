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

  void Start()
  {

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
    Debug.Log("here");
    GridCell cursorCell = GetCell(cursor.GridPosition);
    if (cursorCell.IsOccupied())
    {
      TryPickUpObject(cursorCell);
    }
  }

  private void TryPickUpObject(GridCell cursorCell)
  {
    Debug.Log("here 2");
    selectedObject = cursorCell.occupyingObject;
    selectedTransform = cursorCell.occupyingTransform;

    foreach (Transform t in selectedObject.Shape)
    {
      t.SetParent(selectedTransform);
    }
  }

  private void TryReleaseObject(GridCell cursorCell)
  {

  }

  /// <summary>
  /// Move the Grid cursor in the direction of <c>dir</c>. Check boundaries and determine if the movement is legal
  /// </summary>
  /// <param name="dir"></param>
  public static void MoveCursor(Vector2Int dir) => instance.MoveCursorHelper(dir);
  private void MoveCursorHelper(Vector2Int dir)
  {
    Vector3Int position3d = grid.WorldToCell(cursor.transform.position);

    // check for any obstacles that prevent movement


    // Clamp the position to be within the grid boundary
    int x = Math.Clamp(position3d.x + dir.x, leftBound, rightBound);
    int y = Math.Clamp(position3d.y + dir.y, bottomBound, topBound);

    // Clamp the position within the camera bounds
    Camera camera = Camera.main;
    float halfHeight = camera.orthographicSize;
    float halfWidth = camera.aspect * halfHeight;

    Vector3 cameraPosition = camera.transform.position;
    float cameraX = cameraPosition.x;
    float cameraY = cameraPosition.y;
    float cameraLeftBound = (cameraX - halfWidth);
    float cameraRightBound = (cameraX + halfWidth);
    float cameraTopBound = (cameraY + halfHeight);
    float cameraBottomBound = (cameraY - halfHeight);
    Vector3Int cameraTopLeft = grid.WorldToCell(new(cameraLeftBound, cameraTopBound));
    Vector3Int cameraBottomRight = grid.WorldToCell(new(cameraRightBound, cameraBottomBound));

    x = Math.Clamp(x, cameraTopLeft.x, cameraBottomRight.x);
    y = Math.Clamp(y, cameraBottomRight.y, cameraTopLeft.y);

    Vector2Int newPosition = new(x, y);

    // make sure to check whether moving the piece is legal too. It may be prudent to always normalize dir, and force it be in one of the cardinal directions only

    SetCursor(newPosition);
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
    Vector3Int gridPos = grid.WorldToCell(gridObject.transform.position);
    gridObject.transform.position = grid.GetCellCenterWorld(gridPos);

    // logically define object location
    foreach (Transform t in gridObject.Shape)
    {
      Vector3Int cellCoord = grid.WorldToCell(t.position);
      SetCellObject(gridObject, t, cellCoord);
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

  private GridCell GetCell(int x, int y)
  {
    return cells[x - leftBound][y - topBound];
  }

  private void SetCellObject(GridObject gridObject, Transform goTransform, Vector3Int position)
  {
    Debug.Log($"{position.x} {position.y}");
    Debug.Log($"{position.x - leftBound} {position.y + topBound}");
    GridCell cell = cells[position.x - leftBound][position.y + topBound];
    cell.occupyingObject = gridObject;
    cell.occupyingTransform = goTransform;
  }

  private void SetCellObject(GridObject gridObject, Transform goTransform, int x, int y)
  {
    // Debug.Log($"No Translate {x},{y}");
    // Debug.Log($"Translate {x - leftBound},{y + topBound}");
    GridCell cell = cells[x - leftBound][y + topBound];
    cell.occupyingObject = gridObject;
    cell.occupyingTransform = goTransform;
  }

  private bool IsHoldingObject => selectedObject != null;
}