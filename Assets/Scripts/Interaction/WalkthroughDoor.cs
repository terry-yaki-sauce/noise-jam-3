using System.Collections.Generic;
using System.Threading.Tasks;
using Interaction;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkthroughDoor : MonoBehaviour, IFocusable
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset scene;
#endif
    [SerializeField] private int buildIndex; 
    [SerializeField] private Vector2 loadPoint;

    [SerializeField] private List<AudioClip> doorSFX;
    private System.Random rng = new();

    void OnTriggerEnter2D(Collider2D collision) => (this as IFocusable).OnTriggerEnter2D(collision);

    void OnTriggerExit2D(Collider2D collision) => (this as IFocusable).OnTriggerExit2D(collision);

    public async Task Enter()
    {
        int k = rng.Next(0,doorSFX.Count);
        AudioClip clip = doorSFX[k];
        AudioManager.PlaySFX(clip);
#if UNITY_EDITOR
        await GameManager.LoadScene(scene, loadPoint);
#else
        await GameManager.LoadScene(buildIndex, loadPoint);
#endif
    }
}
