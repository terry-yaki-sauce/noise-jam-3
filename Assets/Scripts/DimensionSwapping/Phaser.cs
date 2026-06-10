using UnityEngine;
namespace DimensionSwapping
{
  public abstract class Phaser : MonoBehaviour
  {
    [SerializeField] protected Dimension activeDimension;
    public Dimension ActiveDimension { get => activeDimension; set => activeDimension = value; }

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

    protected virtual void SetActivity(Dimension dimension)
    {
      bool active = activeDimension == dimension;
    }
  }
}