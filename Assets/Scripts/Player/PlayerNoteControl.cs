using System;
using System.Collections;
using System.Linq;
using DimensionSwapping;
using NoteSystem;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerNoteControl : PlayerSystem
{
  private static readonly int PlayingInstrumentHash = Animator.StringToHash("playingInstrument");
  private PlayerInput playerInput;

    [SerializeField] private int numNotes = 3;
    private int noteIndex = 0;
    private NoteValue[] notes;

    private string targetActionMap = "Player";

    private static readonly NoteValue[] DIMENSION_SWAP_COMBO = { NoteValue.G, NoteValue.G, NoteValue.ASharp };

    // 1 2 4
    private static readonly NoteValue[] DIMENSION_SWAP_HEAVEN = { NoteValue.G, NoteValue.ASharp, NoteValue.F };
    // 4 3 2
    private static readonly NoteValue[] DIMENSION_SWAP_HELL = { NoteValue.F, NoteValue.C, NoteValue.ASharp };
    // 1 3 2
    private static readonly NoteValue[] MODIFY_GRID_COMBO = { NoteValue.G, NoteValue.C, NoteValue.ASharp };
    // 444
    private static readonly NoteValue[] RESET_LEVEL_COMBO = { NoteValue.F, NoteValue.F, NoteValue.F };

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        notes = new NoteValue[numNotes];
    }

    void OnOpenNoteMenu()
    {
        targetActionMap = playerInput.currentActionMap.name;
        NoteMenuView.Show();
        playerInput.SwitchCurrentActionMap("Note Menu");
        player.Animator.SetBool(PlayingInstrumentHash, true);
    }

    void OnCloseNoteMenu()
    {
        ClearNotes();

        NoteMenuView.Hide();
        NoteMenuView.PlayClosingSFX();
        playerInput.SwitchCurrentActionMap(targetActionMap);
        // nice little hack cuz im lazy. this should called via some sort event invocation
        if (targetActionMap == "Grid")
        {
            GridManager.Show();
        }
        player.Animator.SetBool(PlayingInstrumentHash, false);
    }

    void OnHint()
    {
        NoteMenuView.ToggleSongHints();
    }

    // there is probably a more elegant way to do this using the input actions map, but im way too lazy rn to read those docs so whatever
    void OnNote1() => AddNote(NoteValue.G);
    void OnNote2() => AddNote(NoteValue.ASharp);
    void OnNote3() => AddNote(NoteValue.C);
    void OnNote4() => AddNote(NoteValue.F);

    void AddNote(NoteValue value)
    {
        if (noteIndex >= numNotes) return;

        // set the next note
        notes[noteIndex] = value;
        NoteMenuView.instance.NoteDisplay.SetNote(value, noteIndex);
        AudioManager.PlayNote(value);
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
        // targetActionMap = playerInput.currentActionMap.name;
        // if the note are valid...
        if (CheckEqual(notes, DIMENSION_SWAP_COMBO))
        {
            // set a timeout and give visual feedback
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(NoteStatus.sucess);
            GameManager.SwapDimension();
        }
        else if (CheckEqual(notes, DIMENSION_SWAP_HEAVEN))
        {
            NoteStatus status = GameManager.SwapDimension(Dimension.Heaven);
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(status);
        }
        else if (CheckEqual(notes, DIMENSION_SWAP_HELL))
        {
            NoteStatus status = GameManager.SwapDimension(Dimension.Hell);
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(status);
        }
        else if (CheckEqual(notes, MODIFY_GRID_COMBO) && GridManager.instance)
        {
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(NoteStatus.sucess);
            // instead of the default player input, we need the grid control
            targetActionMap = "Grid";
        }
        else if (CheckEqual(notes,RESET_LEVEL_COMBO))
        {
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(NoteStatus.sucess);
            
            GridManager.ResetPuzzle();
        }
        else
        {
            // ... also set a timeout and give visual feedback?
            enumerator = NoteMenuView.CloseMenuWithNoteCombo(NoteStatus.fail);
        }

        yield return StartCoroutine(enumerator);
        ClearNotes();
        playerInput.SwitchCurrentActionMap(targetActionMap);
        player.Animator.SetBool(PlayingInstrumentHash, false);
        if (targetActionMap == "Grid")
        {
            GridManager.Show();
        }
        // reset the action map
        targetActionMap = "Player";
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
