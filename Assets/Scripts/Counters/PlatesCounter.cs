using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
  public event EventHandler HandlePlateSpawned;
  
  [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
  private float spawnPlatTimer;
  private const float spawnPlatTimerMax = 4f;
  private int platesSpawnedAmount;
  private int platesSpawnedAmountMax = 4;

  private void Update()
  {
    spawnPlatTimer += Time.deltaTime;
    if (spawnPlatTimer > spawnPlatTimerMax)
    {
      spawnPlatTimer = 0f;

      if (platesSpawnedAmount < platesSpawnedAmountMax)
      {
        platesSpawnedAmount++;
        
        HandlePlateSpawned?.Invoke(this, EventArgs.Empty);
      }
    }
  }
}
