using System;
using System.Collections.Generic;
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
        private Image[] images;
        private float a;
        private float[] alphas;
        public TextMeshProUGUI textMesh;

        void Awake()
        {
            image = GetComponent<Image>();
            images = GetComponentsInChildren<Image>();
            alphas = new float[images.Length];
            a = image.color.a;
            for (int i = 0; i < images.Length; i++)
            {
                alphas[i] = images[i].color.a;
            }
            // because we only set the color to clear, this means that the notes will still take up space inside of the Grid Layout. You need to either delete the objects and instantiate as needed, or disable the gameObjects themselves for the Grid Layout to automatically space notes. This (probably) won't matter
        }

        public void Clear()
        {
            a = image.color.a;
            for (int i = 0; i < images.Length; i++)
            {
                alphas[i] = images[i].color.a;
            }

            image.color = new(image.color.r, image.color.g, image.color.b, 0);
            foreach (Image i in images)
            {
                i.color = new(i.color.r, i.color.g, i.color.b, 0);
            }
            textMesh.text = "";
        }

        internal void Show()
        {
            image.color = new(image.color.r, image.color.g, image.color.b, a);
            for (int i = 0; i < images.Length; i++)
            {
                Image image = images[i];
                image.color = new(image.color.r, image.color.g, image.color.b, alphas[i]);
            }
            textMesh.text = "";
        }
    }

}

