using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            HandlePlayerHasObject(player);
        }
    }

    private void HandlePlayerHasObject(Player player)
    {
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}