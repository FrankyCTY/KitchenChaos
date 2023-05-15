using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManger_HandleRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManger_HandleRecipeCompleted;
        
        UpdateVisual();
    }

    private void DeliveryManger_HandleRecipeCompleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManger_HandleRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        removeRecipeItemsFromUI();

        renderRecipeItemsOnUI();
    }

    private void removeRecipeItemsFromUI()
    {
        foreach (Transform child in container)
        {
            // We don't want to remove the recipeTemplate as we still need to instantiate it
            // But will this cause multiple empty recipeTemplate?
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void renderRecipeItemsOnUI()
    {
        foreach (RecipeSO waitingRecipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform instantiatedRecipeTemplate_Trxm = Instantiate(recipeTemplate, container);
            instantiatedRecipeTemplate_Trxm.gameObject.SetActive(true);
            
            // Update recipe name text
            instantiatedRecipeTemplate_Trxm.GetComponent<RecipeTemplateUI>().SetRecipeSO(waitingRecipeSO);
        } 
    }
}
