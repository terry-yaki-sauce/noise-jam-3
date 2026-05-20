using System.Threading.Tasks;
using Interaction;
using UnityEditor;
using UnityEngine;

public class WalkthroughDoor : MonoBehaviour, IFocusable
{
    [SerializeField] private SceneAsset scene;
    [SerializeField] private Vector2 loadPoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.tag != "Player") return;
        Player player = collisionObject.GetComponent<Player>();

        player.focusedTarget = this;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.tag != "Player") return;
        Player player = collisionObject.GetComponent<Player>();

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
        if (player.focusedTarget == this)
        {
            player.focusedTarget = null;
        }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
    }

    public async Task Enter()
    {
        await GameManager.LoadScene(scene,loadPoint);
    }
}
