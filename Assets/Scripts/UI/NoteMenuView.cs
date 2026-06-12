using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

/// <summary>
/// Handles revealing the overall UI of the note playing menu. This includes any potential extra UI elements or effects
/// </summary>
public class NoteMenuView : Singleton<NoteMenuView>
{

    [SerializeField] private NoteDisplay noteDisplay;
    public NoteDisplay NoteDisplay { get => noteDisplay; }

    private Coroutine closingSequence;

    [SerializeField] private List<AudioClip> openClips;
    [SerializeField] private List<AudioClip> closeClips;
    private System.Random rng = new();
    private int clipIndex;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public static void Show() => instance.ShowHelper();
    public void ShowHelper()
    {
        instance.gameObject.SetActive(true);
        if (openClips.Count > 0)
        {
            clipIndex = rng.Next(0,openClips.Count);
            AudioClip clip = openClips[clipIndex];
            AudioManager.PlaySFX(clip);
        }
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
    }

    public static void PlayClosingSFX() => instance.PlayClosingSFXHelper();
    private void PlayClosingSFXHelper()
    {
        if (closeClips.Count > 0)
        {
            // play the corresponding close clip
            AudioClip clip = closeClips[clipIndex];
            AudioManager.PlaySFX(clip);
        }
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
