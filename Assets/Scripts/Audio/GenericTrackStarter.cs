using UnityEngine;

public class GenericTrackerStarter : MonoBehaviour
{
  [SerializeField] private AudioClip clip;

  void Start()
  {
    AudioManager.StartGenericTrack(clip);
  }
}