using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler HandlePlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSo;


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);
            HandlePlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}