using NoteSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Handles revealing the overall UI of the note playing menu. This includes any potential extra UI elements or effects
/// </summary>
public class NoteMenuView : Singleton<NoteMenuView>
{

    [SerializeField] private Image menu;
    [SerializeField] private NoteDisplay noteDisplay;
    public NoteDisplay NoteDisplay {get => noteDisplay;}

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        noteDisplay.ResetNotes();
    }
}
