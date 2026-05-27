using UnityEngine;

public class PlayerCamera : PlayerSystem
{
    private CameraBounds bounds;
    [SerializeField] private Transform cameraFollowPoint;

    void Start()
    {
        bounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update()
    {
        Camera camera = Camera.main;
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        Transform cameraTransform = camera.transform;
        float leftBound = bounds.Left + halfWidth;
        float rightBound = bounds.Right - halfWidth;
        float topBound = bounds.Top - halfHeight;
        float bottomBound = bounds.Bottom - halfHeight;

        float x = cameraFollowPoint.position.x;
        x = Mathf.Clamp(x,leftBound,rightBound);
        float y = cameraFollowPoint.position.y;
        y = Mathf.Clamp(y,bottomBound,topBound);
        float z = cameraTransform.position.z;
        cameraTransform.position = new Vector3(x, y, z);
    }
}
