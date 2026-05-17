using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    private PlayerInput playerInput;
    private string previousActionmap; 

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        previousActionmap = playerInput.defaultActionMap;
    }

    void OnOpenPauseMenu()
    {
        previousActionmap = playerInput.currentActionMap.name;
        playerInput.SwitchCurrentActionMap("UI");
        PauseMenu.instance.Show();
    }

    public void OnResumeGame()
    {
        playerInput.SwitchCurrentActionMap(previousActionmap);
        previousActionmap = playerInput.defaultActionMap;
        PauseMenu.instance.Hide();
    }
}
