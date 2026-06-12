using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{

    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    [SerializeField] private List<AudioClip> buttonClicks;
    [SerializeField] private float buttonVolume = 1f;
    private System.Random rng = new();

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public static void Show() => instance.ShowHelper();
    private void ShowHelper()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioManager.PlaySFX(openClip);
    }

    public static void Hide() => instance.HideHelper();
    private void HideHelper()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioManager.PlaySFX(closeClip);
    }

    public static void ReturnToTitle() => GameManager.ReturnToTitle();

    public static void Resume() => GameManager.Player.UI.OnResumeGame();

    public void PlayButtonClickSound()
    {
        int index = rng.Next(0,buttonClicks.Count);
        AudioClip clip = buttonClicks[index];
        
        AudioManager.PlaySFX(clip,buttonVolume);
    }
}
