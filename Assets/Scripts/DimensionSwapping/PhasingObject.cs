using UnityEngine;

namespace DimensionSwapping
{
  public class PhasingObject : MonoBehaviour
  {
    [SerializeField] private Dimension activeDimension;

    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;

    void Awake()
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
      collider = GetComponent<Collider2D>();
    }

    void Start()
    {
      SetActivity();
    }

    void OnEnable()
    {
      GameManager.instance.swappedDimension += SetActivity;
    }

    void OnDisable()
    {
      GameManager.instance.swappedDimension -= SetActivity;
    }

    void SetActivity()
    {
      bool active = activeDimension == GameManager.ActiveDimension;
      spriteRenderer.enabled = active;
      collider.enabled = active;
    }
  }
}