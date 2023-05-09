using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingKitchenObjectSOArray;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen object here
            if (player.HasKitchenObject())
            {
                // Check if from obj is valid and can be transformed via recipe
                if (HasRecipeWithFromObj(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetParent(this);
                    player.clearKitchenObject();
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
                GetKitchenObject().SetParent(player);
                clearKitchenObject();
            }
        }
    }

    // Given user holding a kitchen object, then they place it in the cutting counter
    // Then interact alternate triggered to cut the kitchen object
    public override void InteractAlternate(Player player)
    {
        // Only start cuttiny if:
        // 1. The cutting counter has a kitchen object
        // 2. The kitchen object has a corresponding cutting recipe
        if (HasKitchenObject() && HasRecipeWithFromObj(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a kitchen object here
            // Destroy current kitchen object
            // Then replace it with the cut kitchen object
            Debug.Log(GetKitchenObject());
            KitchenObjectSO resultingKitchenObjectSo = GetObjectWithRecipe(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(resultingKitchenObjectSo, this);
        }
        // Do nothing, as the cutting counter does not have any kitchen object 
    }

    private bool HasRecipeWithFromObj(KitchenObjectSO fromObj)
    {
        foreach (var cuttingKitchenObj in cuttingKitchenObjectSOArray)
        {
            if (cuttingKitchenObj.fromObject == fromObj)
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectSO GetObjectWithRecipe(KitchenObjectSO fromObject)
    {
        foreach (var cuttingKitchenObj in cuttingKitchenObjectSOArray)
        {
            if (cuttingKitchenObj.fromObject == fromObject)
            {
                return cuttingKitchenObj.toObject;
            }
        }

        Debug.LogError($"GetObjectWithRecipe: Can't find any matching cuttingKitchenObj for {fromObject}, returning null and might cause exception");
        return null;
    }
}
