using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DimensionSwapping;
using NoteSystem;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
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
    public event Action<UIMode> UIMenuChanged;
    public event Action<UIMode,bool> UIMenuSetActive;
    private Stack<UIMode> menuHistory = new();

    [SerializeField] private Renderer2DData renderer2D;
    private readonly string[] toggleShaders = { "Chromatic Aberration", "Dither" };
    private List<ScriptableRendererFeature> rendererFeatures = new();
    [SerializeField] private bool shadersEnabled = true;
    private readonly string invertShader = "Invert";
    [SerializeField] private bool invertEnabled = true;
    private ScriptableRendererFeature invertFeature;

    private bool [] solvedScenes;
    public static bool CurrenSceneSolved => instance.solvedScenes[SceneManager.GetActiveScene().buildIndex];
    [SerializeField] private float solveSum = 3;
    public event Action<bool> GameCleared;

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
        foreach (ScriptableRendererFeature r in renderer2D.rendererFeatures)
        {
            if (toggleShaders.Contains(r.name)) rendererFeatures.Add(r);

            if (r.name == invertShader)
            {
                invertFeature = r;
                r.SetActive(false);
            }
        }

        foreach (ScriptableRendererFeature r in rendererFeatures)
        {
            r.SetActive(activeDimension == Dimension.Hell && shadersEnabled);
        }

        solvedScenes = new bool[SceneManager.sceneCountInBuildSettings];
        for(int i = 0; i < solvedScenes.Length; i++)
        {
            solvedScenes[i] = false;
        }
    }

    public static void LoadNextScene() => LoadScene(SceneManager.GetActiveScene().buildIndex + 1 %
                SceneManager.sceneCountInBuildSettings);

    public static void ReturnToTitle()
    {
        for(int i = 0; i < instance.solvedScenes.Length; i++)
        {
            instance.solvedScenes[i] = false;
        }

        AudioManager.StopAllTracks();
        LoadScene(TITLE_INDEX);
        AudioManager.StartGenericTrack();
    }

    public static void SolveScene(int buildIndex) => instance.SolveSceneHelper(buildIndex);
    private void SolveSceneHelper(int buildIndex)
    {
        solvedScenes[buildIndex] = true;
        GridManager.Hide();
        player.PlayerInput.SwitchCurrentActionMap("Player");

        if(IsFinalDoorUnlocked())
            GameCleared?.Invoke(true);
    }

    public static bool IsFinalDoorUnlocked()
    {
        int solveCount = 0;
        foreach(bool b in instance.solvedScenes)
        {
            if(b) solveCount++;
        }
        
        return solveCount >= instance.solveSum;
    }

    public static void LoadScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

#if UNITY_EDITOR
    public static async Task LoadScene(SceneAsset scene, Vector2 loadPoint) => await instance.LoadSceneHelper(scene, loadPoint);
    public async Task LoadSceneHelper(SceneAsset scene, Vector2 loadPoint)
    {
        var sceneLoad = SceneManager.LoadSceneAsync(scene.name);
        sceneLoad.completed += (operation) =>
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;
            player = playerObject.GetComponent<Player>();
            player.transform.position = loadPoint;
        };
        await sceneLoad;
    }
#endif

    public static async Task LoadScene(int buildIndex, Vector2 loadPoint) => await instance.LoadSceneHelper(buildIndex, loadPoint);
    public async Task LoadSceneHelper(int buildIndex, Vector2 loadPoint)
    {
        var sceneLoad = SceneManager.LoadSceneAsync(buildIndex);
        sceneLoad.completed += (operation) =>
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;
            player = playerObject.GetComponent<Player>();
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

        // activate shaders
        foreach (ScriptableRendererFeature r in rendererFeatures)
        {
            r.SetActive(dimension == Dimension.Hell && shadersEnabled);
        }

        return NoteStatus.sucess;
    }

    public static void RefreshKeybindUIs()
    {
        // we do all these stupid checks because i fucking hate Unity
        if (!instance) return;
        if (!Player) return;
        if (!Player.PlayerInput) return;
        instance.ControlsChanged.Invoke(Player.PlayerInput);

        // NoteMenuView.RefreshControls(Player.PlayerInput);
        // GridManager.RefreshControls(Player.PlayerInput);
    }

    public static void InvertColors(bool active)
    {
        if (!instance) return;
        if (!instance.invertFeature) return;
        instance.invertFeature.SetActive(active && instance.invertEnabled);
    }

    public static void ChangeUIMenu(UIMode mode) => instance?.ChangeUIMenuHelper(mode);
    private void ChangeUIMenuHelper(UIMode mode)
    {
        menuHistory.Push(mode);
        UIMenuChanged.Invoke(mode);
    }

    public static void CloseMenu() => instance?.CloseMenuHelper();
    private void CloseMenuHelper()
    {
        if (menuHistory.Count <= 1)
        {
            menuHistory.Clear();
            UIMenuChanged.Invoke(UIMode.None);
        }
        else
        {
            menuHistory.Pop();
            UIMenuChanged.Invoke(menuHistory.Peek());
        }

    }

    public static void ShowUIKeybinds(UIMode mode, bool active) => instance?.ShowUIKeybindsHelper(mode,active);
    private void ShowUIKeybindsHelper(UIMode mode, bool active)
    {
        UIMenuSetActive.Invoke(mode,active);
    }
}
