using UnityEngine;

public class GridCell : MonoBehaviour
{
  public GridObject occupyingObject;
  public Transform occupyingTransform;

  public bool IsOccupied() => occupyingObject != null;
}