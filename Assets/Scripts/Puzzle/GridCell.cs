using DimensionSwapping;
using UnityEngine;

public class GridCell : MonoBehaviour
{
  private GridObject heavenObject, hellObject;
  private Transform heavenTransform, hellTransform;

  public GridObject HeavenObject => heavenObject;
  public GridObject HellObject => hellObject;

  public GridObject OccupyingObject
  {
    get
    {
      if (GameManager.ActiveDimension == Dimension.Heaven)
        return heavenObject;
      else
        return hellObject;
    }
    set
    {
      if (GameManager.ActiveDimension == Dimension.Heaven)
        heavenObject = value;
      else
        hellObject = value;
    }
  }
  public Transform OccupyingTransform
  {
    get
    {
      if (GameManager.ActiveDimension == Dimension.Heaven)
        return heavenTransform;
      else
        return hellTransform;
    }
    set
    {
      if (GameManager.ActiveDimension == Dimension.Heaven)
        heavenTransform = value;
      else
        hellTransform = value;
    }
  }

  public bool IsOccupied { get => OccupyingObject != null; }

  // only use during game state initialization
  public void InitializeGridObject(GridObject gridObject, Transform t)
  {
    Dimension dimension = gridObject.GridPhaser.ActiveDimension;
    if (dimension == Dimension.Heaven)
    {
      heavenObject = gridObject;
      heavenTransform = t;
    }
    else
    {
      hellObject = gridObject;
      hellTransform = t;
    }
  }
}