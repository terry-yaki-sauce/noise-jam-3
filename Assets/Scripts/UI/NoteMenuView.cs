using System.Collections;
using System.Threading.Tasks;
using Audio;
using NoteSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Handles revealing the overall UI of the note playing menu. This includes any potential extra UI elements or effects
/// </summary>
public class NoteMenuView : Singleton<NoteMenuView>
{

    [SerializeField] private NoteDisplay noteDisplay;
    public NoteDisplay NoteDisplay {get => noteDisplay;}
    
    private Coroutine closingSequence;

    [SerializeField] private MenuSFX menuSFX;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeCLip;

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public static void Show() => instance.ShowHelper();
    public void ShowHelper()
    {
        instance.gameObject.SetActive(true);
        menuSFX.PlaySound(openClip);
    }

    public static void Hide() => instance.HideHelper();
    public void HideHelper()
    {
        if (closingSequence != null)
        {
            StopCoroutine(closingSequence);
            closingSequence = null;
        }
        gameObject.SetActive(false);
        noteDisplay.ResetNotes();
        menuSFX.PlaySound(closeCLip);
    }

    public static IEnumerator CloseMenuWithNoteCombo(bool success = true)
    {
        yield return instance.CloseMenuWithNoteComboHelper(success);
    }
    public IEnumerator CloseMenuWithNoteComboHelper(bool success)
    {
        closingSequence = StartCoroutine(NoteDisplay.ShowSuccess(success));
        yield return closingSequence;
        Hide();
    }

}
