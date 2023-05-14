using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<HandleIngredientAddedEventArgs> HandleIngredientAdded;

    public class HandleIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSo;
    }
    
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    
    private List<KitchenObjectSO> kitchenObjectSOList;

    public void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo)
    {
        if (!IsValidKitchenObjectSO(kitchenObjectSo))
        {
            Debug.Log("Not a valid ingredient");
            return false;
        }
        
        if (kitchenObjectSOList.Contains(kitchenObjectSo))
        {
            Debug.Log("Already has this type");
            return false;
        }
        
        kitchenObjectSOList.Add(kitchenObjectSo);
        
        HandleIngredientAdded?.Invoke(this, new HandleIngredientAddedEventArgs
        {
            kitchenObjectSo = kitchenObjectSo
        });
        return true;
    }

    private bool IsValidKitchenObjectSO(KitchenObjectSO kitchenObjectSo)
    {
        return validKitchenObjectSOList.Contains(kitchenObjectSo);
    }
}
