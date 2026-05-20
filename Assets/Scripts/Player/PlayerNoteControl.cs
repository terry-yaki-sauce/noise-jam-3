using System;
using System.Collections;
using System.Linq;
using NoteSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNoteControl : PlayerSystem
{
    PlayerInput playerInput;

    [SerializeField] private int numNotes = 3;
    private int noteIndex = 0;
    private NoteValue[] notes;

    private static readonly NoteValue[] DIMENSION_SWAP_COMBO = { NoteValue.Note1, NoteValue.Note1, NoteValue.Note2 };

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        notes = new NoteValue[numNotes];
    }

    void OnOpenNoteMenu()
    {
        NoteMenuView.Show();
        playerInput.SwitchCurrentActionMap("Note Menu");
    }

    void OnCloseNoteMenu()
    {
        ClearNotes();

        NoteMenuView.Hide();
        playerInput.SwitchCurrentActionMap("Player");
    }

    // there is probably a more elegant way to do this using the input actions map, but im way too lazy rn to read those docs so whatever
    void OnNote1() => AddNote(NoteValue.Note1);

    void OnNote2() => AddNote(NoteValue.Note2);

    void AddNote(NoteValue value)
    {
        if (noteIndex >= numNotes) return;

        // set the next note
        notes[noteIndex] = value;
        NoteMenuView.instance.NoteDisplay.SetNote(value, noteIndex);
        noteIndex++;


        // once we've played all the notes, check for a valid combo
        if (noteIndex >= numNotes)
        {
            StartCoroutine(TryNoteCombo());
        }
    }

    void ClearNotes()
    {
        noteIndex = 0;
        notes = new NoteValue[numNotes];
    }

    IEnumerator TryNoteCombo()
    {
        IEnumerator enumerator = null;
        // if the note are valid...
        if (CheckEqual(notes, DIMENSION_SWAP_COMBO))
        {
            // set a timeout and give visual feedback
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(success: true);
            GameManager.SwapDimension();
        }
        else
        {
            // ... also set a timeout and give visual feedback?
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(success: false);
        }

        yield return StartCoroutine(enumerator);
        ClearNotes();
        playerInput.SwitchCurrentActionMap("Player");

    }

    private static bool CheckEqual(NoteValue[] n1, NoteValue[] n2)
    {
        if (n1.Length != n2.Length) return false;

        for (int i = 0; i < n1.Length; i++)
        {
            if (n1[i] != n2[i]) return false;
        }
        return true;
    }
}
