using System.Collections.Generic;
using Dialogue;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
  [CreateAssetMenu(fileName = "DialogueLines", menuName = "Scriptable Objects/DialogueLines")]
  public class DialogueLines : ScriptableObject
  {
    [SerializeField] private UnityEvent finalCallback;
    public UnityEvent FinalCallback => finalCallback;

    [SerializeField] private UnityEvent<DialogueContext> finalCallbackContext;
    public UnityEvent<DialogueContext> FinalCallbackContext => finalCallbackContext;
    [SerializeField] private DialogueContext finalContext;
    public DialogueContext FinalContext => finalContext;

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

    [SerializeField] private AudioClip voice;
    public AudioClip Voice => voice;


    // simple script for passing in events
    [SerializeField] private UnityEvent<Sprite,bool> onStart,onEnd;
    public UnityEvent<Sprite,bool> OnStart => onStart;
    public UnityEvent<Sprite,bool> OnEnd => onEnd;

    [SerializeField] private UnityEvent<DialogueContext> onStartContext,onEndContext;
    public UnityEvent<DialogueContext> OnStartContext => onStartContext;
    public UnityEvent<DialogueContext> OnEndContext => onEndContext;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private DialogueContext context;
    public DialogueContext Context => context;
  }

  [System.Serializable]
  public class DialogueContext
  {
    public Sprite splashSprite;
    public bool isSplashSpriteActive = false;

    public string tag = "";
    public bool isTagObjectActive = false;

    public string name = "";
    public bool isNamedObjectActive = false;

    public DialogueContext(Sprite sp = null, bool isSpActive = false, string tag = "", bool isTagObjectActive = false)
    {
      splashSprite = sp;
      isSplashSpriteActive = isSpActive;
      this.tag = tag;
      this.isTagObjectActive = isTagObjectActive;
    }
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
    DrawPropertiesExcluding(serializedObject, "lines");


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