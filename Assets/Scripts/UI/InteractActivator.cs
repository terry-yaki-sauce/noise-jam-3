using UnityEngine;
using Interaction;
using TMPro;
using UnityEngine.InputSystem;
using Util;

namespace Dialogue
{

  public class InteractActivator : DialogueActivator, IInteractable
  {
    void OnTriggerEnter2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;
      Player player = collisionObject.GetComponent<Player>();

      player.focusedTarget = this;
    }

    void OnTriggerExit2D(Collider2D collision)
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

    public void Interact() => Activate();

  }
}