using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;

                if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();

                    KitchenObjectSO friedObject = fryingRecipeSO.toObject;
                    KitchenObject.SpawnKitchenObject(friedObject, this);

                    Debug.Log($"StoveCounter: Update: From Frying to Fried!");
                    state = State.Fried;
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeFromObject(GetKitchenObject().GetKitchenObjectSO());
                }

                Debug.Log($"StoveCounter: Update: Frying timer has value {fryingTimer}");
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                if (burningTimer > burningRecipeSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();

                    KitchenObjectSO burningObject = burningRecipeSO.toObject;
                    KitchenObject.SpawnKitchenObject(burningObject, this);

                    Debug.Log($"StoveCounter: Update: From Fried to Burned!");
                    state = State.Burned;
                }

                Debug.Log($"StoveCounter: Update: Burning timer has value {burningTimer}");
                break;
            case State.Burned:
                break;
        }

        Debug.Log(state);
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
                    fryingRecipeSO = GetFryingRecipeFromObject(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
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

    private BurningRecipeSO GetBurningRecipeFromObject(KitchenObjectSO fromObject)
    {
        foreach (var burningRecipeSO in burningRecipeSOArray)

        {
            if (burningRecipeSO.fromObject == fromObject)
            {
                return burningRecipeSO;
            }
        }

        Debug.LogError(
            $"GetBurningRecipeFromObject: Can't find any matching burningKitchenObj for {fromObject}, returning null and might cause exception");

        return null;
    }
}