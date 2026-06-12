using System;
using System.Threading.Tasks;
using DimensionSwapping;
using NoteSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public const int TITLE_INDEX = 0;
    private Player player;
    public static Player Player { get => instance.player; }

    [SerializeField] private int targetFrameRate = 120;

    public event Action<Dimension> SwappedDimension;
    private Dimension activeDimension = Dimension.Heaven;
    public static Dimension ActiveDimension { get => instance.activeDimension; }

    public event Action<PlayerInput> ControlsChanged;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject); // might need later, but this causes problems with some buttons/references

#if UNITY_EDITOR
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
#endif
    }

    public static void LoadNextScene() => LoadScene(SceneManager.GetActiveScene().buildIndex + 1 %
              SceneManager.sceneCountInBuildSettings);

    public static void ReturnToTitle()
    {
        AudioManager.StopAllTracks();
        LoadScene(TITLE_INDEX);
    } 

    public static void LoadScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public static async Task LoadScene(SceneAsset scene, Vector2 loadPoint) => await instance.LoadSceneHelper(scene, loadPoint);
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
        EditorApplication.isPlaying = false;
#endif
    }

    public static void SetPlayer(Player player) => instance.SetPlayerHelper(player);
    private void SetPlayerHelper(Player _player)
    {
        player = _player;
    }

    public static void SwapDimension() => instance.SwapDimensionHelper();
    private void SwapDimensionHelper()
    {
        // NOTE: may want to move this to PlayerNoteControl, if we want rejection feedback to be on the note page, but that is probably not desirable?
        if (!GridManager.IsValidDimensionSwap())
        {
            // TODO: REJECTION FEEDBACK
            Debug.Log("Swap failed due to impeding object");
            AudioManager.PlayInvalid();
            return;
        }

        // success
        activeDimension = (Dimension)(((int)activeDimension + 1) % DimensionConfig.NUM_DIMENSIONS);
        SwappedDimension?.Invoke(activeDimension);
        AudioManager.PlayDimensionSwap(activeDimension);
        AudioManager.SwitchDimensionTracks(activeDimension);
        GridManager.CheckGoalHovered();
    }


    public static NoteStatus SwapDimension(Dimension dimension) => instance.SwapDimensionHelper(dimension);
    private NoteStatus SwapDimensionHelper(Dimension dimension)
    {
        // if we didn't actually change dimensions...
        if (activeDimension == dimension) return NoteStatus.warn;


        // NOTE: may want to move this to PlayerNoteControl, if we want rejection feedback to be on the note page, but that is probably not desirable?
        if (!GridManager.IsValidDimensionSwap())
        {
            // TODO: REJECTION FEEDBACK
            Debug.Log("Swap failed due to impeding object");
            AudioManager.PlayInvalid();
            return NoteStatus.warn;
        }

        // success
        activeDimension = dimension;
        SwappedDimension?.Invoke(activeDimension);
        AudioManager.PlayDimensionSwap(dimension);
        AudioManager.SwitchDimensionTracks(dimension);
        GridManager.CheckGoalHovered();

        return NoteStatus.sucess;
    }

    public static void RefreshKeybindUIs()
    {
        // we do all these stupid checks because i fucking hate Unity
        if (!instance) return;
        if (!Player) return;
        if (!Player.PlayerInput) return;
        instance.ControlsChanged.Invoke(Player.PlayerInput);

        NoteMenuView.RefreshControls(Player.PlayerInput);
    }
}
