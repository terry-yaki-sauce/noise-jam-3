using System;
using System.Collections.Generic;
using DimensionSwapping;
using NoteSystem;
using UnityEngine;
using Util;

public class AudioManager : Singleton<AudioManager>
{
    private System.Random rng = new();

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource heavenSource;
    [SerializeField] private AudioSource hellSource;
    private bool startHeavenHellTrack = false;

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

    [Header("Dimension Swap")]
    [SerializeField] private AudioSource dimensionSwapSource;
    [SerializeField] private AudioClip heavenSwap;
    [SerializeField] private AudioClip hellSwap;

    [Header("Extra")]
    [SerializeField] private float invalidVolume = .3f;
    [SerializeField] private List<AudioClip> invalid;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (!musicSource) musicSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (GameManager.instance)
        {
            heavenSource.mute = GameManager.ActiveDimension != Dimension.Heaven;
        }
        else
        {
            heavenSource.mute = false;
        }
        hellSource.mute = !hellSource.mute;
    }

    void Update()
    {
        // if either one stops, restart both
        if (startHeavenHellTrack && !(heavenSource.isPlaying && hellSource.isPlaying))
        {
            heavenSource.Stop();
            hellSource.Stop();

            heavenSource.Play();
            hellSource.Play();
        }
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

    public static void SampleAndPlay(List<AudioClip> clips, float volume = 1f)
    {
        AudioClip clip = GameUtils.GetRandomElement(clips);
        instance?.sfxSource?.PlayOneShot(clip, volume);
    }

    public static void PlaySFXfromSource(AudioSource source, AudioClip clip, float volume = 1f)
    {
        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public static void PlayInvalid()
    {
        if (!instance) return;
        SampleAndPlay(instance.invalid, instance.invalidVolume);
    }

    public static void PlayNote(NoteValue value, float volume = 1f) => instance?.PlayNoteHelper(value, volume);
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
        int k = rng.Next(0, footStepClips.Count);
        AudioClip nextClip = footStepClips[k];
        // footstepIndex = (footstepIndex + 1) % footStepClips.Count;

        stepSource.Stop();
        stepSource.clip = nextClip;
        stepSource?.Play();
    }

    public static void PlayDimensionSwap(Dimension dimension) => instance?.PlayDimensionSwapHelper(dimension);
    private void PlayDimensionSwapHelper(Dimension dimension)
    {
        AudioClip clip = dimension == Dimension.Heaven ? heavenSwap : hellSwap;
        // TODO: add some fade-in? meh...
        dimensionSwapSource.Stop();
        dimensionSwapSource.clip = clip;
        dimensionSwapSource.Play();
    }

    public static void SwitchDimensionTracks(Dimension dimension) => instance?.SwitchDimensionsHelper(dimension);
    private void SwitchDimensionsHelper(Dimension dimension)
    {
        AudioSource liveSource = dimension == Dimension.Heaven ? heavenSource : hellSource;
        AudioSource offSource = dimension == Dimension.Heaven ? hellSource : heavenSource;

        offSource.mute = true;
        liveSource.mute = false;
    }

    public static void StartHeavenHellTrack(bool start = true) => instance?.StartHeavenHellTrackHelper(start);
    private void StartHeavenHellTrackHelper(bool start)
    {
        musicSource.Stop();

        startHeavenHellTrack = start;
        heavenSource.Play();
        hellSource.Play();
        heavenSource.mute = GameManager.ActiveDimension != Dimension.Heaven;
        hellSource.mute = !heavenSource.mute;
    }

    public static void StopAllTracks() => instance?.StopAllTracksHelper();
    private void StopAllTracksHelper()
    {
        startHeavenHellTrack = false;
        heavenSource.Stop();
        hellSource.Stop();
        musicSource.Stop();
    }

    public static void StartGenericTrack(AudioClip clip = null) => instance.StartGenericTrackHelper(clip);
    private void StartGenericTrackHelper(AudioClip clip = null)
    {
        startHeavenHellTrack = false;
        heavenSource.Stop();
        hellSource.Stop();

        musicSource.Stop();
        if (clip) musicSource.clip = clip;
        musicSource.Play(); 
    }
}
