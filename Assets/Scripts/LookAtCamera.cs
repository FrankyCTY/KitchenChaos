using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;
    
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                // Rotates the transform so the forward vector points at target's current position (origin)
                // Instead of syncing/aligning their forward axis, it force this object to rotate to look at the camera's origin
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                /// Imagine this object's position (transform.position) = 0, 0, 0
                /// Camera's position (Camera.main.position) = 1,0,1
                /// transform.position - Camera.main.transform.position = -1, 0, -1 -> Which is the direction where the camera
                /// looking at that will see through this object.
                ///
                /// Look inverted:
                /// Instead of from object seeing to the camera
                /// We basically look at the inverted direction :)
                Vector3 displacementFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + displacementFromCamera);
                break;
            case Mode.CameraForward:
                // Making an object face a target direction: Make object looks in the camera (not look at)
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                return;
        }
    }
}
