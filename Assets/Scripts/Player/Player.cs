using System;
using Interaction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// class takes care of sending linking data to other objects, like the GameManager
// This is the outward face where Player Systems should send and receive data
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    
    public PlayerInput PlayerInput { get; private set; }
    public InputAction DialogueNext { get; private set; }
    
    [DoNotSerialize] public IInteractable interactionTarget;
    public Action interacted;

    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        DialogueNext = PlayerInput.actions.FindAction("Dialogue/Next");
    }

    void Start()
    {
        if (GameManager.instance)
            GameManager.instance.player = this;
        else
            Debug.LogWarning("No Game Manager instance found.");
    }
}
