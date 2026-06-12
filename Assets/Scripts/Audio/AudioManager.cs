using System.Collections.Generic;
using NoteSystem;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Notes")]
    [SerializeField] private AudioSource noteSource;
    [SerializeField] private AudioClip G_clip;
    [SerializeField] private AudioClip ASharp_clip;
    [SerializeField] private AudioClip C_clip;
    [SerializeField] private AudioClip F_clip;

    [Header("Footsteps")]
    [SerializeField] private AudioSource stepSource;
    [SerializeField] private List<AudioClip> footStepClips;
    // private int footstepIndex = 0;
    private System.Random rng = new();

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
        instance?.musicSource?.Play();
    }

    public static void PlaySFX(AudioClip clip, float volume = 1f)
    {
        instance?.sfxSource?.PlayOneShot(clip, volume);
    }

    public static void PlayNote(NoteValue value, float volume = 1f) => instance.PlayNoteHelper(value,volume);
    private void PlayNoteHelper(NoteValue value, float volume = 1f)
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
        if (clip == null)
        {
            Debug.LogWarning("Failed to assign clip to passed NoteValue");
            return;
        }
        instance?.noteSource?.PlayOneShot(clip, volume);
    }

    public static void PlayFootstep() => instance.PlayFootstepHelper();
    private void PlayFootstepHelper()
    {
        int k = rng.Next(0,footStepClips.Count);
        AudioClip nextClip = footStepClips[k];
        // footstepIndex = (footstepIndex + 1) % footStepClips.Count;

        stepSource.Stop();
        stepSource.clip = nextClip;
        stepSource?.Play();
    }
}
