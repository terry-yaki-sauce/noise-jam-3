using UnityEngine;
using UnityEngine.UI;
using NoteSystem;
using System;
using System.Collections;
using Unity.Mathematics;

/// <summary>
/// Handles the displaying of the notes specifically, using a Grid Layout to arrange the notes
/// </summary>
public class NoteDisplay : MonoBehaviour
{
    [SerializeField] private float finishSongWaitTime = 1.5f;
    [SerializeField] private Note[] notes = new Note[3];

    [SerializeField] private Color G_color;
    [SerializeField] private Vector3 G_Rotation;
    [SerializeField] private Color ASharp_color;
    [SerializeField] private Vector3 ASharp_Rotation;
    [SerializeField] private Color C_color;
    [SerializeField] private Vector3 C_Rotation;
    [SerializeField] private Color F_color;
    [SerializeField] private Vector3 F_Rotation;

    void Start()
    {
        notes = GetComponentsInChildren<Note>();
        foreach(Note n in notes)
        {
            n.Clear();
        }
    }

    /// <summary>
    /// Set the note at position <c>index</c>.
    /// </summary>
    /// <param name="value">The type (or pitch) of note to show</param>
    public void SetNote(NoteValue value, int index)
    {
        if (index >= notes.Length) return; // failsafe

        // unique properties to change for each note type
        Vector3 rotation = Vector3.one;
        Color color = Color.clear;

        // select properties based on value param
        switch (value)
        {
            case NoteValue.G:
                rotation = G_Rotation;
                color = G_color;
                break;
            case NoteValue.ASharp:
                rotation = ASharp_Rotation;
                color = ASharp_color;
                break;
            case NoteValue.C:
                rotation = C_Rotation;
                color = C_color;
                break;
            case NoteValue.F:
                rotation = F_Rotation;
                color = F_color;
                break;
        }

        Note n = notes[index];
        n.transform.eulerAngles = rotation;
        n.image.color = color;
        n.Show();
    }

    /// <summary>
    /// Reset all notes to transparent.
    /// </summary>
    public void ResetNotes()
    {
        foreach (Note n in notes)
        {
            n.Clear();
        }
    }

    /// <summary>
    /// indicate to the player that a note combination was valid
    /// </summary>
    public IEnumerator ShowSuccess(bool success = true)
    {
        for (int i = 0; i < 2; i++)
        {
            Color[] originalColors = new Color[notes.Length];
            for (int j = 0; j < notes.Length; j++)
            {
                Note n = notes[j];
                originalColors[j] = n.image.color;

                if (success)
                    n.image.color = Color.green;
                else
                    n.image.color = Color.red;
            }

            yield return new WaitForSeconds(finishSongWaitTime / 5);

            for (int j = 0; j < notes.Length; j++)
            {
                Note n = notes[j];
                // n.image.color = originalColors[j];
                n.Clear();
            }

            yield return new WaitForSeconds(finishSongWaitTime / 5);
        }

        for (int j = 0; j < notes.Length; j++)
        {
            Note n = notes[j];
            n.Clear();

            if (success)
                n.image.color = Color.green;
            else
                n.image.color = Color.red;
        }

        yield return new WaitForSeconds(finishSongWaitTime / 5);
    }

    public IEnumerator ShowSuccess(NoteStatus success = NoteStatus.sucess)
    {
        for (int i = 0; i < 2; i++)
        {
            Color[] originalColors = new Color[notes.Length];
            for (int j = 0; j < notes.Length; j++)
            {
                Note n = notes[j];
                n.Show();

                switch (success)
                {
                    case NoteStatus.sucess:
                        n.image.color = Color.green;
                        break;
                    case NoteStatus.fail:
                        n.image.color = Color.red;
                        break;
                    case NoteStatus.warn:
                        n.image.color = Color.yellow;
                        break;
                }
            }

            yield return new WaitForSeconds(finishSongWaitTime / 5);

            for (int j = 0; j < notes.Length; j++)
            {
                Note n = notes[j];
                // n.image.color = originalColors[j];
                n.Clear();
            }

            yield return new WaitForSeconds(finishSongWaitTime / 5);
        }

        for (int j = 0; j < notes.Length; j++)
        {
            Note n = notes[j];
            n.Show();

            switch (success)
            {
                case NoteStatus.sucess:
                    n.image.color = Color.green;
                    break;
                case NoteStatus.fail:
                    n.image.color = Color.red;
                    break;
                case NoteStatus.warn:
                    n.image.color = Color.yellow;
                    break;
            }
        }

        yield return new WaitForSeconds(finishSongWaitTime / 5);
    }
}
