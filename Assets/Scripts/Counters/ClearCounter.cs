using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen object here
            if (player.HasKitchenObject())
            {
                playerPutObjectOnCounter(player);
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            // There is already a Kitchen Object here
            if (player.HasKitchenObject())
            {
                // Object on the counter BUT the Player is carrying something, do nothing
            }
            else
            {
                playerPickUpObjectOnCounter(player);
            }
        }
    }

    private void playerPickUpObjectOnCounter(Player player)
    {
        Debug.Log("Object on the counter AND the Player is not carrying anything, pick it up.");
        GetKitchenObject().SetParent(player);
        clearKitchenObject();
    }

    private void playerPutObjectOnCounter(Player player)
    {
        Debug.Log("Nothing on the counter AND the player is carrying an object, put it on the counter.");
        var kitchenObjectOnHand = player.GetKitchenObject();
        kitchenObjectOnHand.SetParent(this);

        player.clearKitchenObject();
    }
}