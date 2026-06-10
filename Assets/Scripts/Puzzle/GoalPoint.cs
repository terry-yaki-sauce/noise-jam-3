using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using DimensionSwapping;
using UnityEngine.Events;


public class GoalPoint : MonoBehaviour
{
  [SerializeField] private Dimension dimension;
  public Dimension Dimension => dimension;
  [SerializeField] private List<Transform> shape;
  public ReadOnlyArray<Transform> Shape {get => shape.ToArray();}

  public UnityEvent<bool> goalEvent;
}