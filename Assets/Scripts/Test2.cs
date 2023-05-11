using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Test2 : MonoBehaviour
{
    [FormerlySerializedAs("objectToOrbit")] [SerializeField] private Transform centralObj;
    
    void Update()
    {
        transform.RotateAround(centralObj.transform.position, Vector3.up, 90 * Time.deltaTime);
    }
}
