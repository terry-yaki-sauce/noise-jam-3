using System;
using System.Threading.Tasks;
using System.Timers;
using Dialogue;
using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

// Handle input for opening UIs
public class PlayerUI : PlayerSystem
{
    private PlayerInput playerInput;
    private string previousActionmap;

    private float eplapsedTime = 0f;
    [SerializeField] private float doorTimeout = .5f;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        previousActionmap = playerInput.defaultActionMap;
    }

    void Update()
    {
        eplapsedTime+=Time.deltaTime;
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
        GameManager.RefreshKeybindUIs();
    }

    private void OnInteract()
    {
        IFocusable target = player.focusedTarget;

        if (target == null) return;
        if (target is not IInteractable) return;

        (target as IInteractable).Interact();
        player.interacted.Invoke();
    }

    
    private async Task OnEnterDoor()
    {
        // if we just opened a door, stop ourselves from instantly opening another (useful when spawning on a door)
        if (eplapsedTime < doorTimeout) return;

        IFocusable target = player.focusedTarget;

        if (target == null) return;
        if (target is not WalkthroughDoor) return;

        player.interacted.Invoke();
        eplapsedTime = 0f;
        await (target as WalkthroughDoor).Enter();
    }
}
