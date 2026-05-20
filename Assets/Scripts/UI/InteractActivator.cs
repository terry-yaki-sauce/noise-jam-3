using UnityEngine;
using Interaction;
using TMPro;
using UnityEngine.InputSystem;
using Util;

namespace Dialogue
{

  public class InteractActivator : DialogueActivator, IInteractable
  {
    void OnTriggerEnter2D(Collider2D collision) => (this as IFocusable).OnTriggerEnter2D(collision);

    void OnTriggerExit2D(Collider2D collision) => (this as IFocusable).OnTriggerExit2D(collision);

    public void Interact() => Activate();

  }
}