using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
  public class ChoiceButton : MonoBehaviour
  {
    [SerializeField] private int buttonIndex;
    public int ButtonIndex {get => buttonIndex;}
    private TextMeshProUGUI textMesh;

    void Awake()
    {
      textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
      textMesh.text = text;
    }

    public void Select()
    {
      DialogueManager.SelectChoice(buttonIndex);
    }
  }
}