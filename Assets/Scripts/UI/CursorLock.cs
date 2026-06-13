using UnityEngine;

public class CursorLock : MonoBehaviour
{
  [SerializeField] private bool lockCursor = true;

  void Start()
  {
    if (lockCursor)
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
    else
    {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }
}