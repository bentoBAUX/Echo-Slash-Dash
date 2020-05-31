using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float MouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 1.2f;
    public float dstFromTargetR = 1f;
    public Vector2 MinMax_pitch = new Vector2(-40, 85);

    

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    void Start()
    {
        lockCursor = true;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }       
    }


    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, MinMax_pitch.x, MinMax_pitch.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - (transform.forward * dstFromTarget + (-transform.right) * dstFromTargetR);
        

    }
}
