using NoteSystem;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoteAudioPlayer : Singleton<NoteAudioPlayer>
{
  private AudioSource audioSource;

  [Header("Notes")]
  [SerializeField] private AudioClip G_clip;
  [SerializeField] private AudioClip ASharp_clip;
  [SerializeField] private AudioClip C_clip;
  [SerializeField] private AudioClip F_clip;
  

  protected override void Awake()
  {
    base.Awake();
    audioSource = GetComponent<AudioSource>();
  }

  public static void PlayNote(NoteValue value) => instance.PlayNoteHelper(value);
  private void PlayNoteHelper(NoteValue value)
  {
    AudioClip clip = null;
    switch (value)
    {
      case NoteValue.G:
        clip = G_clip;
        break;
      case NoteValue.ASharp:
        clip = ASharp_clip;
        break;
      case NoteValue.C:
        clip = C_clip;
        break;
      case NoteValue.F:
        clip = F_clip;
        break;
    }
    if (clip == null){
      Debug.LogWarning("Failed to assign clip to passed NoteValue");
      return;
    }
    instance.audioSource.PlayOneShot(clip);
  }
}