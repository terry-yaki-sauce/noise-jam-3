using UnityEngine;

public class GridCursor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer {get => spriteRenderer;}

    [SerializeField] private Grid grid;
    private Vector3Int gridPosition;
    public Vector3Int GridPosition {get => gridPosition;}

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void SetPosition(Vector2Int newPosition)
  {
    gridPosition = new Vector3Int(newPosition.x,newPosition.y);
    transform.position = grid.GetCellCenterWorld(gridPosition);
  }
}
