using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSo;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return this._kitchenObjectSo;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        Debug.Log($"--------------- {kitchenObjectParent}");
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Parent already has a kitchen object, will try to replace the kitchen object.");
            kitchenObjectParent.clearKitchenObject();
        }

        kitchenObjectParent.SetKitchenObject(this);
        this.positionThisKitchenObject(kitchenObjectParent);
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return this.kitchenObjectParent;
    }

    private void positionThisKitchenObject(IKitchenObjectParent kitchenObjectParent)
    {
        Debug.Log($"---------positionThisKitchenObject {kitchenObjectParent}");
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
}