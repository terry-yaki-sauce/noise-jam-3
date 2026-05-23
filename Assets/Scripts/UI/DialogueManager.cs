using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Dialogue
{
  public class DialogueManager : Singleton<DialogueManager>
  {
    private DialogueLines currentLines;
    private DialogueNode currentNode;
    private int dialogueIndex;

    [Header("UI Object References")]
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private TextMeshProUGUI playerNamePlateText;
    [SerializeField] private TextMeshProUGUI NPCNamePlateText;
    [SerializeField] private GameObject playerNamePlateBox;
    [SerializeField] private GameObject NPCNamePlateBox;
    [SerializeField] private List<ChoiceButton> choiceButtons;

    [Header("Animated Text Configuration")]
    [SerializeField] private float characterFrequency;
    [SerializeField] private float textSkipLeniency = .02f;
    private Coroutine textScrollRoutine;

    void Start()
    {
      gameObject.SetActive(false);
      // may need to moved to an OnEnable script
      for (int i = 0; i < choiceButtons.Count; i++)
      {
        choiceButtons[i].gameObject.SetActive(false);
      }
    }

    public static void StartDialogue(DialogueLines dialogueLines)
    {
      if (instance == null) return;

      if (dialogueLines.DialogueNodes.Count == 0)
      {
        Debug.LogWarning("Empty dialogue passed to CutsceneManager");
        return;
      }

      instance.currentLines = dialogueLines;
      instance.dialogueIndex = 0;

      instance.Show();
      GameManager.Player.PlayerInput.SwitchCurrentActionMap("Dialogue");
      instance.ShowLine();
    }

    /// <summary>
    /// Handles the player input to get the next dialogue
    /// </summary>
    public static void TryShowNext() => instance.TryShowNextHelper();
    private void TryShowNextHelper()
    {
      // if text is scrolling, finish the scroll and consume the input
      if (textScrollRoutine != null)
      {
        StopCoroutine(textScrollRoutine);
        textScrollRoutine = null;
        textBox.maxVisibleCharacters = textBox.text.Length;

        if (currentNode is DialogueChoiceNode)
        {
          DialogueChoiceNode choiceNode = currentNode as DialogueChoiceNode;
          // WARNING that the number of choices should always be less than the total number of UI choice boxes
          for (int i = 0; i < choiceNode.Choices.Count; i++)
          {
            var choiceButton = choiceButtons[i];
            choiceButton.gameObject.SetActive(true);
          }
        }

      }
      else if (currentNode is DialogueChoiceNode)
      {
        // don't auto advance when the player needs to make a choice
      }
      else if (dialogueIndex >= currentLines.DialogueNodes.Count)
      {
        EndDialogue();
      }
      else
      {
        // otherwise, continue to the next text box
        ShowLine();
      }
    }

    /// <summary>
    /// Shows the line at <c>dialogueIndex</c> and begins the text scroll
    /// </summary>
    private void ShowLine()
    {
      currentNode = currentLines.DialogueNodes[dialogueIndex];

      textBox.text = currentNode.Text;

      switch (currentNode.CharacterToShow)
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

      if (currentNode is DialogueChoiceNode)
      {
        DialogueChoiceNode choiceNode = currentNode as DialogueChoiceNode;
        // WARNING that the number of choices should always be less than the total number of UI choice boxes
        for (int i = 0; i < choiceNode.Choices.Count; i++)
        {
          ChoiceButton choiceButton = choiceButtons[i];
          Choice choice = choiceNode.Choices[i];
          choiceButton.SetText(choice.text);
        }
      }

      // always hide the text Boxes, only show once the animation ends or is canceled early
      for (int i = 0; i < choiceButtons.Count; i++)
      {
        choiceButtons[i].gameObject.SetActive(false);
      }

      if (textScrollRoutine != null)
        StopCoroutine(textScrollRoutine);
      textScrollRoutine = StartCoroutine(instance.TextScroll());

      dialogueIndex = (currentNode.JumpIndex < 0) ? dialogueIndex + 1: currentNode.JumpIndex;
    }

    private IEnumerator TextScroll()
    {
      textBox.maxVisibleCharacters = 0;
      for (int i = 0; i < textBox.text.Length; i++)
      {
        textBox.maxVisibleCharacters = i + 1;
        yield return new WaitForSeconds(1 / characterFrequency);
      }
      yield return new WaitForSeconds(textSkipLeniency); // give people a little time in case they mess up and press the next button too fast
      textScrollRoutine = null;

      if (currentNode is DialogueChoiceNode)
      {
        DialogueChoiceNode choiceNode = currentNode as DialogueChoiceNode;
        // WARNING that the number of choices should always be less than the total number of UI choice boxes
        for (int i = 0; i < choiceNode.Choices.Count; i++)
        {
          var choiceButton = choiceButtons[i];
          choiceButton.gameObject.SetActive(true);
        }
      }
    }

    private void EndDialogue()
    {
      GameManager.Player.PlayerInput.SwitchCurrentActionMap("Player");
      Hide();
    }

    private void Show()
    {
      instance.gameObject.SetActive(true);
    }

    private void Hide()
    {
      currentLines = null;
      textBox.text = "";
      playerNamePlateText.text = "";
      NPCNamePlateText.text = "";
      instance.gameObject.SetActive(false);
    }

    public static void SelectChoice(int buttonIndex) => instance.SelectChoiceHelper(buttonIndex);
    private void SelectChoiceHelper(int buttonIndex)
    {
      if (currentNode is not DialogueChoiceNode) return;

      var choiceNode = currentNode as DialogueChoiceNode;

      if(buttonIndex > choiceNode.Choices.Count || buttonIndex < 0)
      {
        Debug.LogWarning($"choice Index out of visialbe button choices (Got {buttonIndex})");
        return;
      }
      dialogueIndex = choiceNode.Choices[buttonIndex].jumpIndex;
      ShowLine();
    }
  }
}