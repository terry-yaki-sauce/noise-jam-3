using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoteSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace NoteSystem
{
    public enum NoteStatus
    {
        sucess,
        fail,
        warn
    }
}
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

    [SerializeField] private AudioClip failComboClip;

    [Header("UI Tooltips")]
    [SerializeField] private string notesFormat = "Notes - [{0}]";
    [SerializeField] private List<InputActionReference> noteActions;
    [SerializeField] private TextMeshProUGUI notesTMP;


    [SerializeField] private string hideFormat = "Hide - [{0}]";
    [SerializeField] private InputActionReference hideAction;
    [SerializeField] private TextMeshProUGUI hideTMP;

    [SerializeField] private string hintFormat = "Hint - [{0}]";
    [SerializeField] private InputActionReference hintAction;
    [SerializeField] private TextMeshProUGUI hintTMP;

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
            clipIndex = rng.Next(0, openClips.Count);
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
        if (!success)
            AudioManager.PlaySFX(failComboClip);
        closingSequence = StartCoroutine(NoteDisplay.ShowSuccess(success));
        yield return closingSequence;
        Hide();
    }

    public static IEnumerator CloseMenuWithNoteCombo(NoteStatus status = NoteStatus.sucess)
    {
        yield return instance.CloseMenuWithNoteComboHelper(status);
    }
    public IEnumerator CloseMenuWithNoteComboHelper(NoteStatus status)
    {
        if (status == NoteStatus.fail)
            AudioManager.PlaySFX(failComboClip);
        closingSequence = StartCoroutine(NoteDisplay.ShowSuccess(status));
        yield return closingSequence;
        Hide();
    }

    public static void RefreshControls(PlayerInput input)
    {
        if (!input) return;

        instance.ChangeTooltips(input);
    }

    private void ChangeTooltips(PlayerInput input)
    {
        if (input.currentControlScheme == "Gamepad")
        {
            notesTMP.text = string.Format(notesFormat, "D-pad");
        }
        else
        {
            string noteBinds = "";
            int i;
            InputActionReference action;
            string bind;
            for (i = 0; i < noteActions.Count - 1; i++)
            {
                action = noteActions[i];
                bind = GameUtils.GetKeybind(input, action);

                noteBinds += $"{bind}, ";
            }
            action = noteActions[i];
            bind = GameUtils.GetKeybind(input, action);
            noteBinds += $"{bind}";


            notesTMP.text = string.Format(notesFormat, noteBinds[0], noteBinds[1], noteBinds[2], noteBinds[3]);
        }

        string hideBind = GameUtils.GetKeybind(input, hideAction);
        hideTMP.text = string.Format(hideFormat, hideBind);

        string hintBind = GameUtils.GetKeybind(input, hintAction);
        hintTMP.text = string.Format(hintFormat, hintBind);
    }
}
