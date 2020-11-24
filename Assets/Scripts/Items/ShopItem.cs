using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour, IBuyable, IPickable
{

    public int cost;
    public bool permanentStock = true;
    
    // Start is called before the first frame update
    public void Interact(InteractController controller){
        if(Buy(controller.GetComponent<MoneyController>())) {
            Pickup(controller.GetComponent<GearController>());
        }
    }


    public bool Buy(MoneyController controller) {
        if (cost <= controller.GetMoney())
        {
            controller.DeductMoney(cost);
            print(controller.GetMoney());
            return true;
        }
        else
            return false;
    }

    public void Pickup(GearController controller) {
        if(permanentStock) 
        {
            GameObject shopItem = Instantiate(gameObject, gameObject.transform.parent);
            shopItem.name = gameObject.name;
        }

        //TODO maybe switch WeaponScript out with more generic? idk
       controller.SwitchWeapon(GetComponent<WeaponScript>());
       Destroy(this);
    }

    public string GetInteractText() {
        string itemName = gameObject.name;
        if (GetComponent<RangedAttack>() != null)
            itemName = GetComponent<RangedAttack>().gunName;

        return "Interact to buy " + itemName + " for $" + cost + ".00";
    }
}
