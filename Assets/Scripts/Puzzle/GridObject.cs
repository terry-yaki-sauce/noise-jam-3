using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(GridPhaser))]
public class GridObject : MonoBehaviour
{
  [SerializeField] private List<Transform> shape;
  public ReadOnlyArray<Transform> Shape {get => shape.ToArray();}

  public GridPhaser GridPhaser {get; private set;}

  [SerializeField] private bool isMovable = false;
  public bool IsMovable => isMovable;

  void Awake()
  {
    GridPhaser = GetComponent<GridPhaser>();
  }

  void Start()
  {
    GridManager.AddGridObject(this);
  }
}