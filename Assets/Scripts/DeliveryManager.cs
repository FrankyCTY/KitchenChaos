using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;
    
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    // We want to create random list for more fun periodically
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        // spawnRecipeTimer is not set initially, which will cause the generation of teh first waitingRecipeSO to be added to the list.
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (!IsWaitingRecipeSOListFull())
            {
                RecipeSO newWaitingRecipeSO =
                    recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                
                waitingRecipeSOList.Add(newWaitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private bool IsWaitingRecipeSOListFull()
    {
        return waitingRecipeSOList.Count >= waitingRecipeMax;
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        bool hasDeliveredTheCorrectObj = false;
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            bool hasSameIngredientAmount = waitingRecipeSO.kitchenObjectSOList.Count ==
                                           plateKitchenObject.GetKitchenObjectSOList().Count;

            if (!hasSameIngredientAmount)
            {
                continue;
            }

            hasDeliveredTheCorrectObj = TryToDeliverPlateKitchenObj(plateKitchenObject, waitingRecipeSO, i);
            if (hasDeliveredTheCorrectObj)
            {
                Debug.Log("Delivery succeeded!");
                break;
            }
        }

        // Not matches found!
        // Player did not deliver a correct recipe
        if (!hasDeliveredTheCorrectObj)
        {
            OnRecipeFail?.Invoke(this, EventArgs.Empty);
            Debug.Log("Player did not deliver a correct recipe");
        }
    }

    private bool TryToDeliverPlateKitchenObj(PlateKitchenObject plateKitchenObject, RecipeSO waitingRecipeSO,
        int waitingRecipeSoIdx)
    {
        // Check to ensure every ingredients on the recipe matches the ingredients on the plate
        bool allIngredientMatched = ValidateRecipeIngredients(waitingRecipeSO.kitchenObjectSOList,
            plateKitchenObject.GetKitchenObjectSOList());
        if (!allIngredientMatched)
        {
            Debug.Log("Has unmatched ingredients!");
            return false;
        }

        AfterDeliverySuccess(waitingRecipeSoIdx);
        return true;
    }

    private void AfterDeliverySuccess(int waitingRecipeSoIdx)
    {
        Debug.Log("Player delivered the correct recipe!");
        waitingRecipeSOList.RemoveAt(waitingRecipeSoIdx);
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }

    private bool ValidateRecipeIngredients(List<KitchenObjectSO> recipeIngredients,
        List<KitchenObjectSO> plateIngredientList)
    {
        foreach (KitchenObjectSO recipeIngredient in recipeIngredients)
        {
            bool ingredientFound = plateIngredientList.Any(plateIngredient => plateIngredient == recipeIngredient);

            if (!ingredientFound)
            {
                return false;
            }
        }

        return true;
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}