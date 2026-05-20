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
      Player player = collisionObject.GetComponent<Player>();

      player.focusedTarget = this;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;
      Player player = collisionObject.GetComponent<Player>();

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
      if (player.focusedTarget == this)
      {
        player.focusedTarget = null;
      }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
    }
  }
}
