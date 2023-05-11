using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Quaternion.Euler(0, 90, 0) * transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
