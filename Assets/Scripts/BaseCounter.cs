using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;
    
    private KitchenObject kitchenObject;
    
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    
    public virtual void InteractAlternate(Player player)
    {
        Debug.LogError("BaseCounter.InteractAlternate();");
    }
    
    public Transform GetKitchenObjectHoldingPointTransform()
    {
        return this.counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void clearKitchenObject()
    {
        this.kitchenObject = null;
        // // Clear the rendered kitchen object on the counter top point
        // // Potentially move to a dedicated counterTopPoint script
        // this.ClearChildrenObjectsInCounterTopPoint();
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }

    // Will be obsolete as we use `DestroySelf()` in the KitchenObject MonoBehavior class
    private void ClearChildrenObjectsInCounterTopPoint()
    {
        for (int i = 0; i < this.counterTopPoint.childCount; i++)
        {
            // Get the child at index i
            Transform child = this.counterTopPoint.GetChild(i);

            // Destroy the child object
            GameObject.Destroy(child.gameObject);
        }
    }
}
