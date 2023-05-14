using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSo;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchecnObjectSOGameObjectList;

    private void Start()
    {
        plateKitchenObject.HandleIngredientAdded += PlateKitchenObject_HandleIngredientAdded;

        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchecnObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_HandleIngredientAdded(object sender,
        PlateKitchenObject.HandleIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchecnObjectSOGameObjectList)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSo == e.kitchenObjectSo)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}