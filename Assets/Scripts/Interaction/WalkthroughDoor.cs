using System.Threading.Tasks;
using Interaction;
using UnityEditor;
using UnityEngine;

public class WalkthroughDoor : MonoBehaviour, IFocusable
{
    [SerializeField] private SceneAsset scene;
    [SerializeField] private Vector2 loadPoint;

    void OnTriggerEnter2D(Collider2D collision) => (this as IFocusable).OnTriggerEnter2D(collision);

    void OnTriggerExit2D(Collider2D collision) => (this as IFocusable).OnTriggerExit2D(collision);

    public async Task Enter()
    {
        await GameManager.LoadScene(scene, loadPoint);
    }
}
