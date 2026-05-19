using UnityEngine;

namespace Dialogue
{
  public class InteractActivator : CutsceneActivator
  {
    void OnTriggerEnter2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;

      GameManager.instance.targetInteractActivator = this;
      Debug.Log("Updated target");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;

      if (GameManager.instance.targetInteractActivator == this)
      {
        GameManager.instance.targetInteractActivator = null;
      }
    }
  }
}