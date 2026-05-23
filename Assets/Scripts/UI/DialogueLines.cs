using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Dialogue
{
  [CreateAssetMenu(fileName = "DialogueLines", menuName = "Scriptable Objects/DialogueLines")]
  public class DialogueLines : ScriptableObject
  {
    [SerializeReference] private List<DialogueNode> dialogueNodes;
    public List<DialogueNode> DialogueNodes { get => dialogueNodes; }
  }

  [System.Serializable]
  public class DialogueNode
  {
    [SerializeField] private string text;
    public string Text { get => text; }

    [SerializeField] private Character characterToShow;
    public Character CharacterToShow { get => characterToShow; }

    [SerializeField] private int jumpIndex = -1;
    public int JumpIndex { get => jumpIndex; }
  }

  [System.Serializable]
  public class DialogueChoiceNode : DialogueNode
  {
    [SerializeField] private List<Choice> choices;
    public List<Choice> Choices { get => choices; }
  }

  [System.Serializable]
  public struct Choice
  {
    public string text;
    public int jumpIndex;
  }

  public enum Character
  {
    Lucy,
    NPC
  }


}


#if UNITY_EDITOR
[CustomEditor(typeof(DialogueLines), true)]
public class DialogueLinesEditor : Editor
{
  public override void OnInspectorGUI()
  {
    serializedObject.Update();
    DrawPropertiesExcluding(serializedObject, "dialogueNodes");

    DialogueLines myTarget = (DialogueLines)target;

    var list = serializedObject.FindProperty("dialogueNodes");
    list.isExpanded = EditorGUILayout.Foldout(
        list.isExpanded,
        $"My List ({list.arraySize})",
        true
    );
    EditorGUI.indentLevel += 1;
    if (list.isExpanded)
    {
      EditorGUI.indentLevel += 1;
      for (int i = 0; i < list.arraySize; i++)
      {
        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent($"{i}"), true);
      }
      EditorGUI.indentLevel -= 1;
    }
    EditorGUI.indentLevel -= 1;
    serializedObject.ApplyModifiedProperties();

    if (GUILayout.Button("Add Regular Node"))
    {
      myTarget.DialogueNodes.Add(new DialogueNode());
    }
    if (GUILayout.Button("Add Choice Node"))
    {
      myTarget.DialogueNodes.Add(new DialogueChoiceNode());
    }
  }
}
#endif