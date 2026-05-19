using System;
using TMPro;
using UnityEngine;

namespace Dialogue
{
  public class CutsceneManager : Singleton<CutsceneManager>
  {
    private DialogueLines currentLines;
    private int dialogueIndex;

    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private TextMeshProUGUI playerNamePlateText;
    [SerializeField] private TextMeshProUGUI NPCNamePlateText;
    [SerializeField] private GameObject playerNamePlateBox;
    [SerializeField] private GameObject NPCNamePlateBox;

    void Start()
    {
      gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueLines dialogueLines)
    {
      if (dialogueLines.DialogueNodes.Count == 0)
      {
        Debug.LogWarning("Empty dialogue passed to CutsceneManager");
        return;
      }

      currentLines = dialogueLines;
      dialogueIndex = 0;

      gameObject.SetActive(true);
      ShowNextLine();
    }

    private void ShowNextLine()
    {
      DialogueNode node = currentLines.DialogueNodes[dialogueIndex]; 

      textBox.text = node.Text;

      switch (node.CharacterToShow)
      {
        case Character.Lucy:
          playerNamePlateText.text = "Lucy";
          playerNamePlateBox.SetActive(true);
          NPCNamePlateBox.SetActive(false);
          break;
        case Character.NPC:
          NPCNamePlateText.text = "NPC";
          playerNamePlateBox.SetActive(false);
          NPCNamePlateBox.SetActive(true);
          break;
        default:
          Debug.LogWarning("No Character Plate Found!");
          playerNamePlateBox.SetActive(false);
          NPCNamePlateBox.SetActive(false);
          break;
      }

    }
  }
}