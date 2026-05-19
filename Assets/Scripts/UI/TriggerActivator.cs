using UnityEngine;

namespace Dialogue
{
  public class TriggerActivator : CutsceneActivator
  {
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
      if (triggered) return;

      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;

      Activate();
    }
  }

}
