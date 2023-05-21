using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public new static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    
    public event EventHandler<IHasProgress.HandleProgressChangedEventArgs> HandleProgressChanged;
    public event EventHandler HandleCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

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

        var kitchenObjectOnPlayerHand = player.GetKitchenObject();
        if (!HasMatchingRecipe(kitchenObjectOnPlayerHand.GetKitchenObjectSO())) return;

        // Has matching recipe. Can place recipe on the counter
        playerPutObjectToThisCounter(player, kitchenObjectOnPlayerHand);

        var objectOnThisCounter = GetObjectSoOnThisCounter();
        var cuttingRecipeSO = GetCuttingRecipeFromObject(objectOnThisCounter);

        HandleProgressChanged?.Invoke(this, new IHasProgress.HandleProgressChangedEventArgs()
        {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
    }

    private void HandleKitchenObjectOnCounter(Player player)
    {
        if (!player.HasKitchenObject())
        {
            playerPickUpObjectOnThisCounter(player);
            return;
        }

        TryAddIngredientToPlayerPlate(player);
    }

    private void TryAddIngredientToPlayerPlate(Player player)
    {
        if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;

        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
        {
            GetKitchenObject().DestroySelf();
        }
    }

    // Given user holding a kitchen object, then they place it in the cutting counter
    // Then interact alternate triggered to cut the kitchen object
    public override void InteractAlternate(Player player)
    {
        // Only start cutting if:
        // 1. The cutting counter has a kitchen object
        // 2. The kitchen object has a corresponding cutting recipe
        var objectOnThisCounter = GetObjectSoOnThisCounter();
        if (HasKitchenObject() && HasMatchingRecipe(objectOnThisCounter))
        {
            cuttingProgress++;

            var cuttingRecipeSO = GetCuttingRecipeFromObject(objectOnThisCounter);
            DispatchCuttingEvents(cuttingRecipeSO);

            if (HasCutCompleted(cuttingRecipeSO))
            {
                ReplaceObjOnCounterWithCutObject();
            }
        }
        // Do nothing, as the cutting counter does not have any kitchen object 
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

    private void DispatchCuttingEvents(CuttingRecipeSO cuttingRecipeSO)
    {
        HandleCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        HandleProgressChanged?.Invoke(this, new IHasProgress.HandleProgressChangedEventArgs()
        {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
    }

    private bool HasCutCompleted(CuttingRecipeSO cuttingRecipeSO)
    {
        return cuttingProgress >= cuttingRecipeSO.cuttingProgressMax;
    }

    private void ReplaceObjOnCounterWithCutObject()
    {
        // There is a kitchen object here
        // Destroy current kitchen object
        // Then replace it with the cut kitchen object
        Debug.Log(GetKitchenObject());
        KitchenObjectSO cuttingResult = ToCutKitchenObject(GetKitchenObject().GetKitchenObjectSO());
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(cuttingResult, this);
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

    private KitchenObjectSO ToCutKitchenObject(KitchenObjectSO fromObject)
    {
        var cuttingRecipeSo = GetCuttingRecipeFromObject(fromObject);

        if (cuttingRecipeSo is not null)
        {
            return cuttingRecipeSo.toObject;
        }

        return null;
    }

    private void playerPutObjectToThisCounter(Player player, KitchenObject kitchenObject)
    {
        Debug.Log(
            "CuttingCounter: Interact: Counter has no object AND player is carrying a kitchen object, put object to the counter");
        kitchenObject.SetParent(this);
        player.clearKitchenObject();

        cuttingProgress = 0;
    }

    private void playerPickUpObjectOnThisCounter(Player player)
    {
        GetKitchenObject().SetParent(player);
        clearKitchenObject();
    }

    private CuttingRecipeSO GetCuttingRecipeFromObject(KitchenObjectSO fromObject)
    {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray)

        {
            if (cuttingRecipeSO.fromObject == fromObject)
            {
                return cuttingRecipeSO;
            }
        }

        Debug.LogError(
            $"GetCuttingRecipeFromObject: Can't find any matching cuttingKitchenObj for {fromObject}, returning null and might cause exception");

        return null;
    }
}