using UnityEngine;
using UnityEngine.Tilemaps;
public class GridManager : Singleton<GridManager>
{
  [SerializeField] private GridCursor cursor;
  [SerializeField] private Grid grid;

  void Start()
  {
    Vector3Int cell = grid.WorldToCell(cursor.transform.position);
    cursor.transform.position = grid.GetCellCenterWorld(cell);
    cursor.gameObject.SetActive(false);
  }

  public static void Show() => instance.ShowHelper();
  private void ShowHelper()
  {
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


  /// <summary>
  /// Move the Grid cursor in the direction of <c>dir</c>. Check boundaries and determine if the movement is legal
  /// </summary>
  /// <param name="dir"></param>
  public static void MoveCursor(Vector3Int dir) => MoveCursor(dir);
  private void MoveCursorHelper(Vector3Int dir)
  {
    Vector3Int position = grid.WorldToCell(cursor.transform.position);
    // Clamp the position to be within the grid. Keep in mind the grid may not be rectangular.
    
    // make sure to check whether moving the piece is legal too. It may be prudent to always normalize dir, and force it be in one of the cardinal directions only

    SetCursor(position);
  }
  /// <summary>
  /// Set the raw position of the Grid cursor. Does not check the legality of the movement.
  /// </summary>
  /// <param name="position"></param>
  public static void SetCursor(Vector3Int position) => instance.SetCursorHelper(position);
  private void SetCursorHelper(Vector3Int position)
  {
    cursor.transform.position = position;
    // drag the object along with it
  }
}