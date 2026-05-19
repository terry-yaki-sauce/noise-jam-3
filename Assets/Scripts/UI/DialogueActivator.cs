using UnityEngine;

namespace Dialogue
{
  public abstract class DialogueActivator : MonoBehaviour
  {
    public DialogueLines dialogueLines;

    public void Activate()
    {
      DialogueManager.StartDialogue(dialogueLines);
    }
  }

}