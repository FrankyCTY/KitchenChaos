using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSo;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool isTesting;

    private KitchenObject kitchenObject;

    private void Update()
    {
        if (this.isTesting && Input.GetKeyDown(KeyCode.T))
        {
            if (kitchenObject != null)
            {
                kitchenObject.SetKitchenObjectParent(this.secondClearCounter);
                this.clearKitchenObject();
            }
        }
    }

    public void Interact(Player player)
    {
        if (this.kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSo.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            // Give the object to the player
            this.kitchenObject.SetKitchenObjectParent(player);
            Debug.Log(kitchenObject.GetKitchenObjectParent());
        }
    }

    public Transform GetKitchenObjectFollowTransform()
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
        // Clear the rendered kitchen object on the counter top point
        // Potentially move to a dedicated counterTopPoint script
        this.ClearChildrenObjectsInCounterTopPoint();
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }

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