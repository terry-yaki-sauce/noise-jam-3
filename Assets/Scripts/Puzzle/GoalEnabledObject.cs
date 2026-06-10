using UnityEngine;

public class GoalEnabledObject : MonoBehaviour
{
  void Start()
  {
    gameObject.SetActive(false);
  }

  public void SetState(bool isGoal)
  {
    gameObject.SetActive(isGoal);
  }
}