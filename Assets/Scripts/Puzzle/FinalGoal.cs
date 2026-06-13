using UnityEngine;

public class FinalGoal : MonoBehaviour
{
  protected bool subscribed = false;

  [SerializeField] private GameObject goal;

  void OnEnable()
  {
    GameManager.instance.GameCleared += SetUnlockStatus;
    subscribed = true;
  }

  void OnDisable()
  {
    GameManager.instance.GameCleared -= SetUnlockStatus;
    subscribed = false;
  }

  void Start()
  {
    if (!subscribed)
    {
      GameManager.instance.GameCleared += SetUnlockStatus;
      subscribed = true;
    }
    SetUnlockStatus(GameManager.IsFinalDoorUnlocked());
  }

  void SetUnlockStatus(bool active)
  {
    goal.SetActive(active);
  }
}