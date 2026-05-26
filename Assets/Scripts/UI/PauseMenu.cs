using Audio;
using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{

    [SerializeField] private MenuSFX menuSFX;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

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
        menuSFX.PlaySound(openClip);
    }

    public static void Hide() => instance.HideHelper();
    private void HideHelper()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menuSFX.PlaySound(closeClip);
    }

    public static void ReturnToTitle() => GameManager.ReturnToTitle();
}
