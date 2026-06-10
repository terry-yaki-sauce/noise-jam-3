using UnityEngine;

namespace DimensionSwapping
{
  public class PhasingObject : Phaser
  {
    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;

    void Awake()
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
      collider = GetComponent<Collider2D>();
    }

    protected override void SetActivity(Dimension dimension)
    {
      bool active = activeDimension == GameManager.ActiveDimension;
      spriteRenderer.enabled = active;
      collider.enabled = active;
    }
  }
}