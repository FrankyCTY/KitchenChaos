using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSo;

    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return this._kitchenObjectSo;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        this.clearCounter = clearCounter;
        if (clearCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a kitchen object, will try to replace the kitchen object.");
            clearCounter.clearKitchenObject();
        }

        clearCounter.SetKitchenObject(this);
        this.positionThisKitchenObject(clearCounter);
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }

    private void positionThisKitchenObject(ClearCounter clearCounter)
    {
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
}