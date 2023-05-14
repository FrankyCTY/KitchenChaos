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
            HandleNoKitchenObjectOnCounter(player);
            return;
        }

        HandleKitchenObjectOnCounter(player);
    }

    private void HandleNoKitchenObjectOnCounter(Player player)
    {
        if (!player.HasKitchenObject()) return;

        playerPutObjectOnCounter(player);
    }

    private void HandleKitchenObjectOnCounter(Player player)
    {
        if (!player.HasKitchenObject())
        {
            playerPickUpObjectOnCounter(player);
            return;
        }

        bool isPlayerHoldingPlate = player.GetKitchenObject().TryGetPlate(out PlateKitchenObject playerPlate);
        if (isPlayerHoldingPlate)
        {
            TryAddIngredientToPlayerPlate(playerPlate);
            return;
        }

        bool isPlateOnThisCounter = GetKitchenObject().TryGetPlate(out PlateKitchenObject plateOnCounter);
        if (isPlateOnThisCounter)
        {
            TryAddIngredientToCounterPlate(plateOnCounter, player);
        }
    }

    private void TryAddIngredientToPlayerPlate(PlateKitchenObject playerPlate)
    {
        if (playerPlate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
        {
            GetKitchenObject().DestroySelf();
        }
    }

    private void TryAddIngredientToCounterPlate(PlateKitchenObject counterPlate, Player player)
    {
        if (counterPlate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().DestroySelf();
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