using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

[RequireComponent(typeof(Collider2D))]
public class ProximityKeybindPrompt : MonoBehaviour, IPromptable
{
  [SerializeField] private TextMeshProUGUI keyBindPrompt;
  [SerializeField] private string keyBindName = "Player/Interact";
  private bool subscribed = false;
  

  void OnEnable()
  {
    Player player = GameManager.instance.player;
    if (player)
    {
      player.PlayerInput.onControlsChanged += onControlsChanged;
      subscribed = true;
      player.interacted += HidePrompt;
    }
  }

  void OnDisable()
  {
    Player player = GameManager.instance.player;
    if (player)
    {
      player.PlayerInput.onControlsChanged -= onControlsChanged;
      player.interacted -= HidePrompt;
    }
    subscribed = false;
  }

  void Start()
  {
    ChangePrompt(GameUtils.GetKeybind(GameManager.instance.player.PlayerInput, keyBindName));
    HidePrompt();
    if (!subscribed)
    {
      GameManager.instance.player.PlayerInput.onControlsChanged += onControlsChanged;
      subscribed = true;
      GameManager.instance.player.interacted += HidePrompt;
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
    Player player = collisionObject.GetComponent<Player>();

    keyBindPrompt.gameObject.SetActive(false);
  }

  public void ChangePrompt(string text)
  {
    keyBindPrompt.text = $"[{text}]";
  }

  public void ShowPrompt()
  {
    keyBindPrompt.gameObject.SetActive(true);
  }

  public void HidePrompt()
  {
    keyBindPrompt.gameObject.SetActive(false);
  }

  private void onControlsChanged(PlayerInput input)
  {
    ChangePrompt(GameUtils.GetKeybind(input, keyBindName));
  }
}