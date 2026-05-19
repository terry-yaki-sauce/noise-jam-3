using UnityEngine;
using Interaction;
using TMPro;
using UnityEngine.InputSystem;
using Util;

namespace Dialogue
{

  public class InteractActivator : DialogueActivator, IInteractable, IPromptable
  {
    [SerializeField] private TextMeshProUGUI interactPrompt;
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
        player.interacted-=HidePrompt;
      } 
      subscribed = false;
    }

    void Start()
    {
      HidePrompt();
      if (!subscribed)
      {
        Debug.Log("Not Subbed");
        GameManager.instance.player.PlayerInput.onControlsChanged += onControlsChanged;
        subscribed = true;
        GameManager.instance.player.interacted += HidePrompt;
      }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;
      Player player = collisionObject.GetComponent<Player>();

      ShowPrompt();

      player.interactionTarget = this;
      Debug.Log("Updated target");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
      GameObject collisionObject = collision.gameObject;
      if (collisionObject.tag != "Player") return;
      Player player = collisionObject.GetComponent<Player>();

      interactPrompt.gameObject.SetActive(false);

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
      if (player.interactionTarget == this)
      {
        player.interactionTarget = null;
      }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
    }

    public void Interact() => Activate();

    public void ShowPrompt()
    {
      interactPrompt.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
      interactPrompt.gameObject.SetActive(false);
    }

    public void ChangePrompt(string text)
    {
      interactPrompt.text = text;
    }

    private void onControlsChanged(PlayerInput input)
    {
      ChangePrompt(GameUtils.GetKeybind(input, "Player/Interact"));
    }
  }
}