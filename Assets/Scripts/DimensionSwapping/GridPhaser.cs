using UnityEngine;
using DimensionSwapping;
using System.Collections.Generic;

// [RequireComponent(typeof(GridObject))]
public class GridPhaser : Phaser
{
  [SerializeField] private List<SpriteRenderer> spriteRenderers;
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
    foreach(SpriteRenderer sp in spriteRenderers)
      sp.enabled = active;
  }
}