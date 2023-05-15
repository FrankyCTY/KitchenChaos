using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.HandleIngredientAdded += PlateKitchenObject_HandleIngredientAdded;
    }

    private void PlateKitchenObject_HandleIngredientAdded(object sender, PlateKitchenObject.HandleIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Remove icons from the previous event
        foreach (Transform child in transform)
        {
            // We don't want to remove iconTemplate
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        
        foreach (KitchenObjectSO kitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList())
        {
            // We ensure to instantiate the icon template as a child of this transform
            // Because this transform (PlateIconsUI) has Grid Layout Group
            Transform instantiatedIconTemplate = Instantiate(iconTemplate, transform);
            instantiatedIconTemplate.gameObject.SetActive(true);
            instantiatedIconTemplate.GetComponent<IconTemplate>().SetIconSprite(kitchenObjectSo);
        }
    }
}
