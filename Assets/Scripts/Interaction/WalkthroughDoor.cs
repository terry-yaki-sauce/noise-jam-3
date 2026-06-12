using System.Collections.Generic;
using System.Threading.Tasks;
using Interaction;
using UnityEditor;
using UnityEngine;

public class WalkthroughDoor : MonoBehaviour, IFocusable
{
    [SerializeField] private SceneAsset scene;
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
        await GameManager.LoadScene(scene, loadPoint);
    }
}
