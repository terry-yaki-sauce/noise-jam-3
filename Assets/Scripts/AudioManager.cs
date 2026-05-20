using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    public static void LoadSong(AudioClip audioClip)
    {
        instance.audioSource.clip = audioClip;
    }

    public static void Play()
    {
        instance.audioSource.Play();
    }
}
