using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableObstruction : MonoBehaviour, IBuyable, IInteractable
{

    public bool AutoInteract { get; set; } = false;

    [SerializeField]
    private int cost;

    public void Interact(InteractController controller){
        if(Buy(controller.GetComponent<MoneyController>())) {
            Destroy(this.gameObject);
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

    public string GetInteractText() {
        return "Interact to buy " + gameObject.name + " for $" + cost + ".00";
    }
}
