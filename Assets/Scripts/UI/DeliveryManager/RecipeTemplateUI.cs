using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTemplateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconsContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;
        
        removeIconsContainerChildren();
        
        renderIconsInIconsContainer(recipeSO);
    }
    
    private void removeIconsContainerChildren()
    {
        foreach (Transform child in iconsContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void renderIconsInIconsContainer(RecipeSO recipeSO)
    {
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform instantiatedIconTemplate_Trxm = Instantiate(iconTemplate, iconsContainer);
            instantiatedIconTemplate_Trxm.gameObject.SetActive(true);
            instantiatedIconTemplate_Trxm.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
