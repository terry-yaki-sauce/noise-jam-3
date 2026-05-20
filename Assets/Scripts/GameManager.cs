using System;
using System.Threading.Tasks;
using Dialogue;
using UnityEditor;
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

        DontDestroyOnLoad(gameObject); // might need later, but this causes problems with some buttons/references

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

    public static void LoadNextScene() => LoadScene(SceneManager.GetActiveScene().buildIndex + 1 %
            SceneManager.sceneCountInBuildSettings);

    public static void ReturnToTitle() => LoadScene(TITLE_INDEX);

    public static void LoadScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public static async Task LoadScene(SceneAsset scene, Vector2 loadPoint) => await instance.LoadSceneHelper(scene,loadPoint);
    public async Task LoadSceneHelper(SceneAsset scene, Vector2 loadPoint)
    {
        var sceneLoad = SceneManager.LoadSceneAsync(scene.name);
        sceneLoad.completed += (operation) =>
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.transform.position = loadPoint;
        };
        await sceneLoad;
    }

    public static void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
