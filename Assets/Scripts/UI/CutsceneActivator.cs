using UnityEngine;

namespace Dialogue
{
  public abstract class CutsceneActivator : MonoBehaviour
  {
    public DialogueLines dialogueLines;

    public void Activate()
    {
      CutsceneManager.instance.StartDialogue(dialogueLines);
    }
  }

}