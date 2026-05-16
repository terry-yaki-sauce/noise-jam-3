using NoteSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Handles revealing the overall UI of the note playing menu. This includes any potential extra UI elements or effects
/// </summary>
public class NoteMenuView : MonoBehaviour
{
    public static NoteMenuView instance;

    [SerializeField] private Image menu;
    [SerializeField] private NoteDisplay noteDisplay;
    public NoteDisplay NoteDisplay {get => noteDisplay;}

    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    
    void Start()
    {
        menu.gameObject.SetActive(false);
    }
    
    public void Show()
    {
        menu.gameObject.SetActive(true);
    }

    public void Hide()
    {
        menu.gameObject.SetActive(false);
        noteDisplay.ResetNotes();
    }
}
