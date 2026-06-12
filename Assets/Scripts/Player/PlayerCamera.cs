using UnityEngine;
using Util;

public class PlayerCamera : PlayerSystem
{
    private CameraBounds bounds;
    [SerializeField] private Transform cameraFollowPoint;
    public CameraFocusPoint FocusPoint { get; private set; }

    void Start()
    {
        bounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update()
    {
        Camera camera = Camera.main;
        Transform cameraTransform = camera.transform;

        if (FocusPoint)
        {
            float x = FocusPoint.position.x;
            float y = FocusPoint.position.y;
            float z = cameraTransform.position.z;
            cameraTransform.position = new(x,y,z);
        }
        else
        {

            float halfHeight = camera.orthographicSize;
            float halfWidth = camera.aspect * halfHeight;

            float leftBound = bounds.Left + halfWidth;
            float rightBound = bounds.Right - halfWidth;
            float topBound = bounds.Top - halfHeight;
            float bottomBound = bounds.Bottom - halfHeight;

            float x = cameraFollowPoint.position.x;
            x = Mathf.Clamp(x, leftBound, rightBound);
            float y = cameraFollowPoint.position.y;
            y = Mathf.Clamp(y, bottomBound, topBound);
            float z = cameraTransform.position.z;
            cameraTransform.position = new Vector3(x, y, z);
        }


    }

    public void SetCameraFocusPoint(CameraFocusPoint point)
    {
        FocusPoint = point;
    }
}
