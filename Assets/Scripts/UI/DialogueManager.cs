using System.Collections;
using TMPro;
using UnityEngine;
using Util;

namespace Dialogue
{
  public class DialogueManager : Singleton<DialogueManager>
  {
    private DialogueLines currentLines;
    private int dialogueIndex;

    [Header("UI Object References")]
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private TextMeshProUGUI playerNamePlateText;
    [SerializeField] private TextMeshProUGUI NPCNamePlateText;
    [SerializeField] private GameObject playerNamePlateBox;
    [SerializeField] private GameObject NPCNamePlateBox;
    [SerializeField] private TextMeshProUGUI NextKeybindText;

    [Header("Animated Text Configuration")]
    [SerializeField] private float characterFrequency;
    [SerializeField] private float textSkipLeniency = .02f;
    private Coroutine textScrollRoutine;

    void Start()
    {
      gameObject.SetActive(false);
      SetNextKeybindHelper();
    }

    public static void StartDialogue(DialogueLines dialogueLines)
    {
      if (dialogueLines.DialogueNodes.Count == 0)
      {
        Debug.LogWarning("Empty dialogue passed to CutsceneManager");
        return;
      }

      instance.currentLines = dialogueLines;
      instance.dialogueIndex = 0;

      instance.Show();
      GameManager.instance.player.PlayerInput.SwitchCurrentActionMap("Dialogue");
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
      }
      else if (dialogueIndex >= currentLines.DialogueNodes.Count - 1)
      {
        EndDialogue();
      }
      else
      {
        // otherwise, continue to the next text box
        dialogueIndex += 1;
        ShowLine();
      }
    }

    /// <summary>
    /// Shows the line at <c>dialogueIndex</c> and begins the text scroll
    /// </summary>
    private void ShowLine()
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

      if (textScrollRoutine != null)
        StopCoroutine(textScrollRoutine);
      textScrollRoutine = StartCoroutine(instance.TextScroll());
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
    }

    private void EndDialogue()
    {
      GameManager.instance.player.PlayerInput.SwitchCurrentActionMap("Player");
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

    public static void SetNextKeybind() => instance.SetNextKeybindHelper();
    private void SetNextKeybindHelper()
    {
      string nextKeybind = GameUtils.GetKeybind(GameManager.instance?.player.PlayerInput, "Dialogue/Next");
      NextKeybindText.text = $"Next [{nextKeybind}]";
    }
  }
}