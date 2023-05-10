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
    }

    [SerializeField] private Mode mode;
    
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
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
                Vector3 directionFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + directionFromCamera );
                break;
            default:
                return;
        }
    }
}
