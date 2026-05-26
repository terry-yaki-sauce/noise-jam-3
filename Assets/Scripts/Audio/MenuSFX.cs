using UnityEngine;

namespace Audio
{
  [RequireComponent(typeof(AudioSource))]
  public class MenuSFX : MonoBehaviour
  {
    private AudioSource audioSource;

    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    void Awake()
    {
      audioSource = GetComponent<AudioSource>();
    }

    public void PlayOpenSound()
    {
      audioSource.PlayOneShot(openClip);
    }

    public void PlayCloseSound()
    {
      audioSource.PlayOneShot(closeClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
      audioSource.PlayOneShot(audioClip);
    }
  }
}