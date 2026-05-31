using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Dialogue
{
  [CreateAssetMenu(fileName = "DialogueLines", menuName = "Scriptable Objects/DialogueLines")]
  public class DialogueLines : ScriptableObject
  {
    [SerializeReference] private List<DialogueNode> lines = new List<DialogueNode>();
    public List<DialogueNode> Lines { get => lines; }
  }

  [System.Serializable]
  public class DialogueNode
  {
    [SerializeField,TextArea(3,10)] private string text;
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
    NPC,
    System,
    Stranger,
    ReverB,
    Gabe,
    Amy, 
    DJ

  }


}


#if UNITY_EDITOR
[CustomEditor(typeof(DialogueLines), true)]
public class DialogueLinesEditor : Editor
{
  public override void OnInspectorGUI()
  {
    serializedObject.Update();
    // DrawPropertiesExcluding(serializedObject, "dialogueNodes");


    // reorderableList.DoLayoutList();
    var list = serializedObject.FindProperty("lines");
    list.isExpanded = EditorGUILayout.Foldout(
        list.isExpanded,
        $"Lines ({list.arraySize})",
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

    DialogueLines myTarget = (DialogueLines)target;
    if (GUILayout.Button("Add Regular Node"))
    {
      myTarget.Lines.Add(new DialogueNode());
    }
    if (GUILayout.Button("Add Choice Node"))
    {
      myTarget.Lines.Add(new DialogueChoiceNode());
    }
    if (GUILayout.Button("Remove Last Node"))
    {
      myTarget.Lines.RemoveAt(myTarget.Lines.Count-1);
    }
  }
}
#endif