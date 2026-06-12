using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class KeybindPrompt : MonoBehaviour, IPromptable
{
  [SerializeField] private TextMeshProUGUI textMesh;
  [SerializeField] protected InputActionReference action;
  [SerializeField] private string format = "[{0}]";
  protected bool subscribed = false;

  protected virtual void OnEnable()
  {
    GameManager.instance.ControlsChanged += OnControlsChanged;
    subscribed = true;
  }

  protected virtual void OnDisable()
  {
    GameManager.instance.ControlsChanged -= OnControlsChanged;
    subscribed = false;
  }

  protected virtual void Start()
  {
    ChangePrompt(GameUtils.GetKeybind(GameManager.Player.PlayerInput, action));
    if (!subscribed)
    {
      GameManager.instance.ControlsChanged += OnControlsChanged;
      subscribed = true;
    }
  }

  public void ChangePrompt(string text)
  {
    textMesh.text = string.Format(format, text);
  }

  public void HidePrompt()
  {
    textMesh.gameObject.SetActive(false);
  }

  public void ShowPrompt()
  {
    textMesh.gameObject.SetActive(true);
  }

  protected void OnControlsChanged(PlayerInput input)
  {
    ChangePrompt(GameUtils.GetKeybind(input, action));
  }
}