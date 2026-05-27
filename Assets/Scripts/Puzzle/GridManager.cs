using UnityEngine;
using UnityEngine.Tilemaps;
public class GridManager : Singleton<GridManager>
{
  [SerializeField] private GridCursor cursor;
  [SerializeField] private Grid grid;

  [SerializeField] private int width,height;
  [SerializeField] private GridCell gridCellPrefab;

  void Start()
  {
    Vector3Int cell = grid.WorldToCell(cursor.transform.position);
    cursor.SetPosition(new(cell.x,cell.y));
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
    Vector2Int position = new(position3d.x,position3d.y);
    // Clamp the position to be within the grid. Keep in mind the grid may not be rectangular.
    
    // make sure to check whether moving the piece is legal too. It may be prudent to always normalize dir, and force it be in one of the cardinal directions only

    SetCursor(position + dir);
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