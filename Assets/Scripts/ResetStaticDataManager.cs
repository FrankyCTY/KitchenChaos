using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        // Clean up static data such as static event
        // Not cleaning up static instance property, because that reference is destroyed when the instance is destroyed
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
