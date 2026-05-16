using UnityEngine;
using UnityEngine.UI;
using NoteSystem;
using System;
using System.Collections;

/// <summary>
/// Handles the displaying of the notes specifically, using a Grid Layout to arrange the notes
/// </summary>
public class NoteDisplay : MonoBehaviour
{
    [SerializeField] private float finishSongWaitTime = 1.5f;
    private Note[] notes;

    void Start()
    {
        notes = GetComponentsInChildren<Note>();
    }

    /// <summary>
    /// Set the note at position <c>index</c>.
    /// </summary>
    /// <param name="value">The type (or pitch) of note to show</param>
    public void SetNote(NoteValue value, int index)
    {
        if (index >= notes.Length) return; // failsafe

        // unique properties to change for each note type
        Color color = Color.clear;

        // select properties based on value param
        switch (value)
        {
            case NoteValue.Note1:
                color = Color.black;
                break;
            case NoteValue.Note2:
                color = Color.gray;
                break;
        }

        // set the next note
        notes[index].image.color = color;
    }

    /// <summary>
    /// Reset all notes to transparent.
    /// </summary>
    public void ResetNotes()
    {
        foreach (Note n in notes)
        {
            n.image.color = Color.clear;
        }
    }

    /// <summary>
    /// indicate to the player that a note combination was valid
    /// </summary>
    public IEnumerator ShowSuccess()
    {
        foreach (Note n in notes)
        {
            n.image.color = Color.green;
        }

        yield return new WaitForSeconds(finishSongWaitTime);
    }
}
