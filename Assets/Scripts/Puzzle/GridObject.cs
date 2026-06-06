using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class GridObject : MonoBehaviour
{
  [SerializeField] private List<Transform> shape;
  public ReadOnlyArray<Transform> Shape {get => shape.ToArray();}

  void Start()
  {
    GridManager.AddGridObject(this);
  }
}