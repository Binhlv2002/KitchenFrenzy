using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
   
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No kitchen obj here
            if (player.HasKitchenObject())
            {
                // player carrying sth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player not carrying sth
            }
        }
        else
        {
            // Has kitchen obj here
            if (player.HasKitchenObject())
            {
                // player carrying sth
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a Plate

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                    
                    
                }

                else
                {
                    // player is not  carrying  plate  but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter is holding plate
                    if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // player is not carrying sth
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
