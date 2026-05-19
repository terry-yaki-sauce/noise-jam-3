using Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public const int TITLE_INDEX = 0;
    public Player player;

    [SerializeField] private int targetFrameRate = 120;
    
    protected override void Awake()
    {
        base.Awake();

        // DontDestroyOnLoad(gameObject); // might need later, but this causes problems with some buttons/references

#if UNITY_EDITOR
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
#endif
    }

    void Start()
    {
        //failsafe
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public static void LoadNextLevel() => LoadLevel(SceneManager.GetActiveScene().buildIndex + 1 %
            SceneManager.sceneCountInBuildSettings);

    public static void ReturnToTitle() => LoadLevel(TITLE_INDEX);

    public static void LoadLevel(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public static void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
