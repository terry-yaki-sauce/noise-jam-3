using UnityEngine;

public class GridCursor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer {get => spriteRenderer;}

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }
}
