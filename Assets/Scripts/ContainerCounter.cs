using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler HandlePlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO _kitchenObjectSo;


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSo.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            HandlePlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}