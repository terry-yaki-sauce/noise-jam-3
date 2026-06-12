using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : Singleton<PauseMenu>
{

    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    [SerializeField] private List<AudioClip> buttonClicks;
    [SerializeField] private float buttonVolume = 1f;
    private System.Random rng = new();

    [SerializeField] private GameObject firstSelected;

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public static void Show() => instance.ShowHelper();
    private void ShowHelper()
    {
        if (GameManager.Player.PlayerInput.currentControlScheme == "Gamepad")
        {
            EventSystem.current.SetSelectedGameObject(firstSelected); 
        }else
        {
            EventSystem.current.SetSelectedGameObject(null); 
        }

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
        
        EventSystem.current.SetSelectedGameObject(null); 
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
