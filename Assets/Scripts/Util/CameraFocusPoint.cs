using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(Collider2D))]
    public class CameraFocusPoint : MonoBehaviour
    {
        [SerializeField] private Transform focusTransform;
        public Vector3 position => focusTransform.position;

        [SerializeField] private float cameraZoom = 1f;
        public float CameraZoom => cameraZoom;

        void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject collisionObject = collision.gameObject;
            if (collisionObject.tag != "Player") return;

            GameManager.Player.PlayerCamera.SetCameraFocusPoint(this);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            GameObject collisionObject = collision.gameObject;
            if (collisionObject.tag != "Player") return;

            if (GameManager.Player.PlayerCamera.FocusPoint == this)
            {
                GameManager.Player.PlayerCamera.SetCameraFocusPoint(null);
            }
        }
    }
}
