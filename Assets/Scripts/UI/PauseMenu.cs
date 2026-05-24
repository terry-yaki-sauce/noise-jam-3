using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public static void Show()
    {
        Time.timeScale = 0;
        instance.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void Hide()
    {
        Time.timeScale = 1;
        instance.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ReturnToTitle() => GameManager.ReturnToTitle();
}
