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
    
    public static void Show()
    {
        instance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
        instance.noteDisplay.ResetNotes();
    }
}
