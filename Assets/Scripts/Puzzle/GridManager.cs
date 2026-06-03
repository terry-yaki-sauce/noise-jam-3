using System;
using UnityEngine;
using UnityEngine.Tilemaps;
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

  void Start()
  {
    Vector3Int cell = grid.WorldToCell(cursor.transform.position);
    cursor.SetPosition(new(cell.x, cell.y));
    cursor.gameObject.SetActive(false);
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

  }

  private void TryPickUpObject()
  {

  }

  private void TryReleaseObject()
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
    Vector3Int cameraTopLeft = grid.WorldToCell(new(cameraLeftBound,cameraTopBound));
    Vector3Int cameraBottomRight = grid.WorldToCell(new(cameraRightBound,cameraBottomBound));

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
  }
}