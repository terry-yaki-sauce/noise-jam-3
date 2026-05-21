using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public enum NoteValue
    {
        Hidden,
        G,
        ASharp,
        C,
        F,
        Note5
    }
    /// <summary>
    /// Simple data holding script for notes in the UI. Let <code>NoteDisplay<code/> handle the modification of these values.
    /// </summary>
    public class Note : MonoBehaviour
    {
        public NoteValue value = NoteValue.Hidden;
        [HideInInspector] public Image image;
        public TextMeshProUGUI textMesh;

        void Start()
        {
            image = GetComponent<Image>();
            // because we only set the color to clear, this means that the notes will still take up space inside of the Grid Layout. You need to either delete the objects and instantiate as needed, or disable the gameObjects themselves for the Grid Layout to automatically space notes. This (probably) won't matter
            image.color = Color.clear;
            textMesh.text = "";
        }

        public void Clear()
        {
            image.color = Color.clear;
            textMesh.text = "";
        }
    }

}

