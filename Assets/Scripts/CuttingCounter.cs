using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen object here
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                player.clearKitchenObject();
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            Debug.Log("HIHIHIHI there is it!");
            // There is already a Kitchen Object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
            }
            else
            {
                Debug.Log("Can I pick it up?");
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                clearKitchenObject();
            }
        }
    }

    // Given user holding a kitchen object, then they place it in the cutting counter
    // Then interact alternate triggered to cut the kitchen object
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            // There is a kitchen object here
            // Destroy current kitchen object
            // Then replace it with the cut kitchen object
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
        // Do nothing, as the cutting counter does not have any kitchen object 
    }
}
