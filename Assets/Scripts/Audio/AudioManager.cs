using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (!musicSource) musicSource = GetComponent<AudioSource>();
    }

    public static void LoadSong(AudioClip audioClip)
    {
        if (!instance) return;

        instance.musicSource.clip = audioClip;
    }

    public static void PlaySong()
    {
        instance?.musicSource.Play();
    }

    public static void PlaySFX(AudioClip clip, float volume = 1f)
    {
        instance?.sfxSource.PlayOneShot(clip,volume);
    }
}
