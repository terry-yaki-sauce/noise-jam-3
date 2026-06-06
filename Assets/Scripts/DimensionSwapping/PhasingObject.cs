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
      SetActivity(GameManager.ActiveDimension);
    }

    void OnEnable()
    {
      GameManager.instance.SwappedDimension += SetActivity;
    }

    void OnDisable()
    {
      GameManager.instance.SwappedDimension -= SetActivity;
    }

    void SetActivity(Dimension dimension)
    {
      bool active = activeDimension == GameManager.ActiveDimension;
      spriteRenderer.enabled = active;
      collider.enabled = active;
    }
  }
}