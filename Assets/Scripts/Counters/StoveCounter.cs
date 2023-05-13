using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    private int fryingProgress = 0;
    private float fryingTimer;

    private void Update()
    {
        if (HasKitchenObject())
        {
            fryingTimer += Time.deltaTime;

            FryingRecipeSO fryingRecipeSO = GetFryingRecipeFromObject(GetKitchenObject().GetKitchenObjectSO());
            if (fryingTimer > fryingRecipeSO.fryingTimerMax)
            {
                // fried
                fryingTimer = 0f;
                Debug.Log($"StoveCounter: Update: Fried!");
                GetKitchenObject().DestroySelf();

                KitchenObjectSO friedObject = fryingRecipeSO.toObject;
                KitchenObject.SpawnKitchenObject(friedObject, this);
            }

            Debug.Log($"StoveCounter: Update: Frying timer has value {fryingTimer}");
        }
    }

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

    private void playerPutObjectToThisCounter(Player player, KitchenObject kitchenObject)
    {
        Debug.Log(
            "StoveCounter: Interact: Counter has no object AND player is carrying a kitchen object, put object to the counter");
        kitchenObject.SetParent(this);
        player.clearKitchenObject();
    }

    private bool HasMatchingRecipe(KitchenObjectSO forInputObject)
    {
        foreach (var fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.fromObject == forInputObject)
            {
                return true;
            }
        }

        return false;
    }

    private void playerPickUpObjectOnThisCounter(Player player)
    {
        GetKitchenObject().SetParent(player);
        clearKitchenObject();
    }

    private KitchenObjectSO GetObjectSoOnThisCounter()
    {
        var result = GetKitchenObject()?.GetKitchenObjectSO();

        if (result is not null)
        {
            return result;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeFromObject(KitchenObjectSO fromObject)
    {
        foreach (var fryingRecipeSO in fryingRecipeSOArray)

        {
            if (fryingRecipeSO.fromObject == fromObject)
            {
                return fryingRecipeSO;
            }
        }

        Debug.LogError(
            $"GetFryingRecipeFromObject: Can't find any matching fryingKitchenObj for {fromObject}, returning null and might cause exception");

        return null;
    }
}