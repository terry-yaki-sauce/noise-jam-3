using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int targetFrameRate = 120;

    public const int TITLE_INDEX = 0;
    
    protected override void Awake()
    {
        base.Awake();

        // DontDestroyOnLoad(gameObject); // might need later, but this causes problems with some buttons/references

#if UNITY_EDITOR
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
#endif

    }

    public void LoadNextLevel() => LoadLevel(SceneManager.GetActiveScene().buildIndex + 1 %
            SceneManager.sceneCountInBuildSettings);

    public void ReturnToTitle() => LoadLevel(TITLE_INDEX);

    public void LoadLevel(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
