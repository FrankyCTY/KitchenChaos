using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    [FormerlySerializedAs("cuttingKitchenObjectSOArray")] [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttinProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen object here
            if (player.HasKitchenObject())
            {
                // Check if from obj is valid and can be transformed via recipe
                var kitchenObjectOnPlayerHand = player.GetKitchenObject();
                if (HasMatchingRecipe(kitchenObjectOnPlayerHand.GetKitchenObjectSO()))
                {
                    playerPutObjectToThisCounter(player, kitchenObjectOnPlayerHand);

                    cuttinProgress = 0;
                }
                // If no valid recipe found, skip.
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
                // Player is carrying something
            }
            else
            {
                // Player is not carrying anything
                playerPickUpObjectOnThisCounter(player);
            }
        }
    }

    // Given user holding a kitchen object, then they place it in the cutting counter
    // Then interact alternate triggered to cut the kitchen object
    public override void InteractAlternate(Player player)
    {
        // Only start cutting if:
        // 1. The cutting counter has a kitchen object
        // 2. The kitchen object has a corresponding cutting recipe
        if (HasKitchenObject() && HasMatchingRecipe(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttinProgress++;
            // There is a kitchen object here
            // Destroy current kitchen object
            // Then replace it with the cut kitchen object
            Debug.Log(GetKitchenObject());
            KitchenObjectSO cuttingResult = GetObjectWithRecipe(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(cuttingResult, this);
        }
        // Do nothing, as the cutting counter does not have any kitchen object 
    }

    private bool HasMatchingRecipe(KitchenObjectSO forInputObject)
    {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.fromObject == forInputObject)
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectSO GetObjectWithRecipe(KitchenObjectSO fromObject)
    {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.fromObject == fromObject)
            {
                return cuttingRecipeSO.toObject;
            }
        }

        Debug.LogError(
            $"GetObjectWithRecipe: Can't find any matching cuttingKitchenObj for {fromObject}, returning null and might cause exception");
        return null;
    }

    private void playerPutObjectToThisCounter(Player player, KitchenObject kitchenObject)
    {
        Debug.Log(
            "CuttingCounter: Interact: Counter has no object AND player is carrying a kitchen object, put object to the counter");
        kitchenObject.SetParent(this);
        player.clearKitchenObject();
    }

    private void playerPickUpObjectOnThisCounter(Player player)
    {
        GetKitchenObject().SetParent(player);
        clearKitchenObject();
    }
}