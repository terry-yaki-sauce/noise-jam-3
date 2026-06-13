using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace UI
{
    public class KeyBindLegend : MonoBehaviour
    {
        protected bool subscribed = false;

        [Header("UI Tooltips")]
        [SerializeField] GameObject gridLegend;
        [SerializeField] List<KeyBindDisplay> gridBindDisplays;
        // this needs an override
        [SerializeField] KeyBindDisplay gridMoveCursorDisplay;

        [SerializeField] GameObject noteLegend;
        [SerializeField] List<KeyBindDisplay> noteBindDisplays;
        // this needs an override
        [SerializeField] KeyBindDisplay noteControlsDisplay;

        protected virtual void OnEnable()
        {
            GameManager.instance.ControlsChanged += OnControlsChanged;
            // GameManager.instance.UIMenuChanged += ShowLegend;
            GameManager.instance.UIMenuSetActive += ShowLegend;
            subscribed = true;
        }

        protected virtual void OnDisable()
        {
            GameManager.instance.ControlsChanged -= OnControlsChanged;
            // GameManager.instance.UIMenuChanged -= ShowLegend;
            GameManager.instance.UIMenuSetActive -= ShowLegend;
            subscribed = false;
        }

        protected virtual void Start()
        {
            if (!subscribed)
            {
                GameManager.instance.ControlsChanged += OnControlsChanged;
                // GameManager.instance.UIMenuChanged += ShowLegend;
                GameManager.instance.UIMenuSetActive += ShowLegend;
                subscribed = true;
            }

            gridLegend.SetActive(false);
            noteLegend.SetActive(false);
        }

        public void ShowLegend(UIMode mode)
        {
            gridLegend.SetActive(mode == UIMode.Grid);
            noteLegend.SetActive(mode == UIMode.Note);
        }

        public void ShowLegend(UIMode mode,bool active)
        {
            gridLegend.SetActive(mode == UIMode.Grid && active);
            noteLegend.SetActive(mode == UIMode.Note && active);
        }

        protected void OnControlsChanged(PlayerInput input)
        {
            // Grid Keybinds
            foreach (KeyBindDisplay kbd in gridBindDisplays)
            {
                string pickupBind = GameUtils.GetKeybind(input, kbd.action);
                kbd.TMP.text = string.Format(kbd.format, pickupBind);
            }

            if (input.currentControlScheme == "Gamepad")
            {
                gridMoveCursorDisplay.TMP.text = string.Format(gridMoveCursorDisplay.format, "D-pad");
            }
            else
            {
                string moveBind = GameUtils.GetKeybind(input, gridMoveCursorDisplay.action);
                gridMoveCursorDisplay.TMP.text = string.Format(gridMoveCursorDisplay.format, moveBind);
            }

            // Note Keybinds
            foreach (KeyBindDisplay kbd in noteBindDisplays)
            {
                string pickupBind = GameUtils.GetKeybind(input, kbd.action);
                kbd.TMP.text = string.Format(kbd.format, pickupBind);
            }

            if (input.currentControlScheme == "Gamepad")
            {
                noteControlsDisplay.TMP.text = string.Format(noteControlsDisplay.format, "D-pad");
            }
            else
            {
                noteControlsDisplay.TMP.text = string.Format(noteControlsDisplay.format, "1, 2, 3 , 4");
            }
        }

    }

    [System.Serializable]
    public struct KeyBindDisplay
    {
        public string format;
        public InputActionReference action;
        public TextMeshProUGUI TMP;
    }

    public enum UIMode
    {
        None,
        Grid,
        Note
    }
}

