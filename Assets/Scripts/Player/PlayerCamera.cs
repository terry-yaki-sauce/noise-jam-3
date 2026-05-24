using UnityEngine;

public class PlayerCamera : PlayerSystem
{
    void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        var x = transform.position.x;
        var y = transform.position.y;
        var z = cameraTransform.position.z;
        cameraTransform.position = new Vector3(x,y,z);
    }
}
