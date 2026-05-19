using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void ReturnToTitle() => GameManager.instance.ReturnToTitle();
}
