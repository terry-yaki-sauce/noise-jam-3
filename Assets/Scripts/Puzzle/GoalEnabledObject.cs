using UnityEngine;

public class GoalEnabledObject : MonoBehaviour
{
  void Start()
  {
    gameObject.SetActive(GameManager.CurrenSceneSolved);
  }

  public void SetState(bool isGoal)
  {
    gameObject.SetActive(isGoal);
  }
}