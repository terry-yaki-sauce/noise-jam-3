using System.Collections;
using NoteSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNoteControl : MonoBehaviour
{
    PlayerInput playerInput;

    [SerializeField] private int numNotes = 3; // run this by other coders before changing this
    private int noteIndex = 0;
    private NoteValue [] notes;

    // TODO: closingSequence is visual and should be in the NoteMenuView or NoteDisplay
    private Coroutine closingSequence;

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
        clearNotes();

        if (closingSequence != null)
        {
            StopCoroutine(closingSequence);
            closingSequence = null;
        }
        NoteMenuView.Hide();
        playerInput.SwitchCurrentActionMap("Player");
    }

    // there is probably a more elegant way to do this using the input actions map, but im way too lazy rn to read those docs so whatever
    void OnNote1() => AddNote(NoteValue.Note1);

    void OnNote2() => AddNote(NoteValue.Note2);

    void AddNote(NoteValue value)
    {
        if(noteIndex >= numNotes) return; 

        // set the next note
        notes[noteIndex] = value;
        NoteMenuView.instance.NoteDisplay.SetNote(value,noteIndex);
        noteIndex++;


        // once we've played all the notes, check for a valid combo
        if (noteIndex >= numNotes)
        {
            tryNoteCombo();
        }
    }

    void clearNotes()
    {
        noteIndex = 0;
        notes = new NoteValue[numNotes];
    }

    void tryNoteCombo()
    {
        // TODO: create actual logic here

        // if the note are valid...
        // set a timeout and give visual feedback
        closingSequence = StartCoroutine(CloseMenuWithNoteCombo());

        // else...
        // ... also set a timeout and give visual feedback?
    }

    IEnumerator CloseMenuWithNoteCombo()
    {
        yield return StartCoroutine(NoteMenuView.instance.NoteDisplay.ShowSuccess());

        clearNotes();
        NoteMenuView.Hide();
        playerInput.SwitchCurrentActionMap("Player");
    }
}
