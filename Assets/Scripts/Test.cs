using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform centralObj;

    public float angle;
    public Vector3 initialDirFromCentralToOrbitingObj;
    public float radius;
    public float degreesPerSecond = 10;

    private void Start()
    {
        // Normalising the vector, which limits its magnitude to 1. This basically limits the vector to directional information only, and while you don’t need to do this, I’m doing it so that I can multiply the direction vector by the radius variable, allowing me to change it later.
        // This direction is used to set up the initial tilt of the orbiting object's orbit relative to the central object.
        initialDirFromCentralToOrbitingObj = (centralObj.transform.position - transform.position).normalized;
        // This example uses relative radius between the orbiting object and the central object
        radius = Vector3.Distance(centralObj.transform.position, transform.position);
    }

    private void Update()
    {
        angle += degreesPerSecond * Time.deltaTime;
        if (angle > 360)
        {
            angle -= 360;
        }
        
        // (z-axis) (0,0,1)
        Vector3 forwardDirection = Vector3.forward;
        // Define the orbit: Distance from the orbit object to the orbiting object in certain direction (forward = 0, 0, 1)
        Vector3 orbit = forwardDirection * radius;
        
        // Look Rotation basically turns a direction vector into a Quaternion angle.
        Quaternion tiltAmount = Quaternion.LookRotation(initialDirFromCentralToOrbitingObj);
        Quaternion angleOfRotation = Quaternion.Euler(0, angle, 0);
        
        // Multiply the Quaternion direction angle (which is the amount of tilt) by the angle of rotation (the angle float variable)
        // to combine the two angles.
        Quaternion tiltedRotation = tiltAmount * angleOfRotation;
        // We are tilting and rotating the orbit which is the orbiting object follows
        // Instead of tilting and rotating the orbiting object
        Vector3 tiltedRotatedOrbit = tiltedRotation * orbit;
        transform.position = centralObj.transform.position + tiltedRotatedOrbit;
    }
}