using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> spawnedPlateVisualGameObjectList;

    private void Awake()
    {
        spawnedPlateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.HandlePlateSpawned += PlatesCounter_HandlePlateSpawned;
        platesCounter.HandlePlateRemoved += PlatesCounter_HandlePlateRemoved;
    }
    
    private void PlatesCounter_HandlePlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * spawnedPlateVisualGameObjectList.Count, 0);
        
        spawnedPlateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
    
    private void PlatesCounter_HandlePlateRemoved(object sender, EventArgs e)
    {
        GameObject plateGameObject = spawnedPlateVisualGameObjectList[spawnedPlateVisualGameObjectList.Count - 1];
        spawnedPlateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
}
