using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
  public static T instance {get; private set;}

  protected virtual void Awake()
  {
    if (instance != null)
    {
      if (instance != this)
      {
        string typename = typeof(T).Name;
        // Debug.Log($"Duplicate Singleton of type [{typename}] found on object [{gameObject}]");
        Destroy(gameObject);
      }
    }
    else
    {
      instance = this as T;
    }
  }
}