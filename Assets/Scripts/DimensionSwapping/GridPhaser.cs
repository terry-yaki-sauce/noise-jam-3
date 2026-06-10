using UnityEngine;
using DimensionSwapping;

[RequireComponent(typeof(GridObject))]
public class GridPhaser : Phaser
{
  [SerializeField] private SpriteRenderer spriteRenderer;
  private GridObject gridObject;

  void Awake()
  {
    gridObject = GetComponent<GridObject>();
  }

  protected override void SetActivity(Dimension dimension)
  {
    if (GridManager.SelectedObject == gridObject)
    {
      activeDimension = dimension;
    }

    bool active = activeDimension == dimension;
    spriteRenderer.enabled = active;
  }
}