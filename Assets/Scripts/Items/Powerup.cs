using UnityEngine;

public enum PowerupType { money, health, ammo, armor,key }
public class Powerup : MonoBehaviour, IPickable
{
    
    public bool AutoInteract { get; set; } = true;
    public int value;
    public PowerupType powerupType;

    public void Interact(InteractController controller) {
        Pickup(controller.GetComponent<GearController>());
    }

    public void Pickup(GearController controller) {
        switch (powerupType)
        {
            case PowerupType.money:
                controller.GetComponent<MoneyController>().AddMoney(value);
                Destroy(gameObject);
                break;
            case PowerupType.key:
                if (!controller.GetComponent<MoneyController>().HasKey())
                {
                    controller.GetComponent<MoneyController>().SetKey(true);
                    Destroy(gameObject);
                }
                break;
            case PowerupType.health:
                controller.GetComponent<HealthController>().Heal(value);
                Destroy(gameObject);
                break;
            case PowerupType.ammo:
                //TODO move ShouldPickup to IAttackable and check that instance on the weapon as it is guaranteed to be on a weapon
                RangedAttack attack = controller.weapon.GetComponent<RangedAttack>();
                if (attack != null && attack.ShouldPickup())
                {
                   attack.GetAmmo(value);
                   Destroy(gameObject);
                }
                break;                    
        }
    }

    public string GetInteractText() {
        return "";
    }
}

