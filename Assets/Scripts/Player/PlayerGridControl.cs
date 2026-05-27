using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridControl : PlayerSystem
{
  void OnMoveCursor(InputValue value)
  {
    Vector2 dirf = value.Get<Vector2>();
    int x = Mathf.RoundToInt(dirf.x);
    int y = Mathf.RoundToInt(dirf.y);
    Vector2Int dir = new(x,y);
    GridManager.MoveCursor(dir);
  }
  void OnCloseGridMenu()
  {
    GridManager.Hide();
    player.PlayerInput.SwitchCurrentActionMap("Player");
  }
}