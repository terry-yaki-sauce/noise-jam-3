using System.Collections.Generic;
using UnityEngine;

public class Enabler : MonoBehaviour
{
  [SerializeField] private List<GameObject> gameObjects;

  public void SetGameObjectsActive(bool active)
  {
    foreach(GameObject go in gameObjects)
    {
      Debug.Log("here");
      go.SetActive(active);
    }
  }
}