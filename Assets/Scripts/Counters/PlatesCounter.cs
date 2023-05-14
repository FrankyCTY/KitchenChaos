using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
  [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
  private float spawnPlatTimer;
  private const float spawnPlatTimerMax = 4f;

  private void Update()
  {
    spawnPlatTimer += Time.deltaTime;
    if (spawnPlatTimer > spawnPlatTimerMax)
    {
      KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this); 
    }
  }
}
