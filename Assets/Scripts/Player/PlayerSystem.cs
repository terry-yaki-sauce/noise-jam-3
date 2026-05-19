using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract class PlayerSystem : MonoBehaviour
{
  protected Player player;

  protected virtual void Awake()
  {
    player = GetComponent<Player>();
  }
}