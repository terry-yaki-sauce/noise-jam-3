using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

[RequireComponent(typeof(Collider2D))]
public class ProximityKeybindPrompt : KeybindPrompt, IPromptable
{
  protected override void OnEnable()
  {
    Player player = GameManager.Player;
    if (player)
    {
      GameManager.instance.ControlsChanged += OnControlsChanged;
      player.interacted += HidePrompt;
      subscribed = true;
    }
  }

  protected override void OnDisable()
  {
    Player player = GameManager.Player;
    if (player)
    {
      GameManager.instance.ControlsChanged -= OnControlsChanged;
      player.interacted -= HidePrompt;
    }
    subscribed = false;
  }

  protected override void Start()
  {
    ChangePrompt(GameUtils.GetKeybind(GameManager.Player.PlayerInput, action));
    HidePrompt();
    if (!subscribed)
    {
      GameManager.Player.PlayerInput.onControlsChanged += OnControlsChanged;
      subscribed = true;
      GameManager.Player.interacted += HidePrompt;
    }

    GetComponent<Collider2D>().isTrigger = true; //failsafe
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    GameObject collisionObject = collision.gameObject;
    if (collisionObject.tag != "Player") return;

    ShowPrompt();
  }

  void OnTriggerExit2D(Collider2D collision)
  {
    GameObject collisionObject = collision.gameObject;
    if (collisionObject.tag != "Player") return;

    HidePrompt();
  }
}