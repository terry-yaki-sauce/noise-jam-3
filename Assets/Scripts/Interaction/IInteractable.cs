using UnityEngine;

namespace Interaction
{
  public interface IInteractable : IFocusable
  {
    public void Interact();
  }

  public interface IFocusable
  {
    public void OnTriggerEnter2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;

      GameManager.Player.focusedTarget = this;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;
      Player player = GameManager.Player;

      if (player.focusedTarget == this)
      {
        player.focusedTarget = null;
      }
    }
  }
}
