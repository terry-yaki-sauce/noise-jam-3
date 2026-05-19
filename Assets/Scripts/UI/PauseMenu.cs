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
    }

    public static void Hide()
    {
        Time.timeScale = 1;
        instance.gameObject.SetActive(false);
    }

    public static void ReturnToTitle() => GameManager.ReturnToTitle();
}
