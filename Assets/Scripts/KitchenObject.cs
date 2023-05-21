using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    private IKitchenObjectParent parent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return this.kitchenObjectSo;
    }

    public void SetParent(IKitchenObjectParent parent)
    {
        // If new parent already have have a kitchen object, we need to clear it
        if (parent.HasKitchenObject())
        {
            Debug.LogError("Parent already has a kitchen object, will try to replace the kitchen object.");
            parent.clearKitchenObject();
        }

        parent.SetKitchenObject(this);
        this.parent = parent;
        
        PositionSelf(parent);
    }

    public IKitchenObjectParent GetParent()
    {
        return this.parent;
    }

    private void PositionSelf(IKitchenObjectParent parent)
    {
        transform.parent = parent.GetKitchenObjectHoldingPointTransform();
        Debug.Log($"KitchenObject: Ready to position myself to parent {parent}");
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf()
    {
        parent.clearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }

        plateKitchenObject = null;
        return false;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent
        parent)
    {
        Debug.Log("KitchenObject: SpawnKitchenObject: Ready to spawn new kitchen object and position to new parent");
        Debug.Log(kitchenObjectSO.prefab);
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetParent(parent);

        return kitchenObject;
    }
}