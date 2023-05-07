using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter 
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSo;

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
                this.clearKitchenObject();
            }
        }
    }
}