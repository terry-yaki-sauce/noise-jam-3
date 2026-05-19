using System;
using Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

// Handle input for opening UIs
public class PlayerUI : PlayerSystem
{
    private PlayerInput playerInput;
    private string previousActionmap;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        previousActionmap = playerInput.defaultActionMap;
    }

    #region PauseMenu

    void OnOpenPauseMenu()
    {
        previousActionmap = playerInput.currentActionMap.name;
        playerInput.SwitchCurrentActionMap("UI");
        PauseMenu.Show();
    }

    public void OnResumeGame()
    {
        playerInput.SwitchCurrentActionMap(previousActionmap);
        previousActionmap = playerInput.defaultActionMap;
        PauseMenu.Hide();
    }

    #endregion

    /// <summary>
    /// For handling Dialogue UI
    /// </summary>
    private void OnNext()
    {
        DialogueManager.TryShowNext();
    }

    private void OnControlsChanged()
    {
        if (DialogueManager.instance)
            DialogueManager.SetNextKeybind();
    }

    private void OnInteract()
    {
        if (player.interactionTarget != null)
        {
            player.interactionTarget.Interact();
            player.interacted.Invoke();
        }
    }
}
