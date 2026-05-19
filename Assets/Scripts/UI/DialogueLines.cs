using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
  [CreateAssetMenu(fileName = "DialogueLines", menuName = "Scriptable Objects/DialogueLines")]
  public class DialogueLines : ScriptableObject
  {
    [SerializeField] private List<DialogueNode> dialogueNodes;
    public List<DialogueNode> DialogueNodes { get => dialogueNodes; }
  }

  [System.Serializable]
  public class DialogueNode
  {
    [SerializeField] private string text;
    public string Text { get => text; }
    
    [SerializeField] private Character characterToShow;
    public Character CharacterToShow { get => characterToShow; }
  }

  public enum Character
  {
    Lucy,
    NPC
  }
}
