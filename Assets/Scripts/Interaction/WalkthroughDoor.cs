using Interaction;
using TMPro;
using UnityEngine;

public class WalkthroughDoor : MonoBehaviour, IInteractable
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.tag != "Player") return;
        Player player = collisionObject.GetComponent<Player>();

        player.interactionTarget = this;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.tag != "Player") return;
        Player player = collisionObject.GetComponent<Player>();

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
        if (player.interactionTarget == this)
        {
            player.interactionTarget = null;
        }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
    }

    public void Interact()
    {
        Debug.Log("Door");
    }
}
