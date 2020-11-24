using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    private Text interactText;
    private bool checkForInput;
    private IInteractable interactable;
    private bool inTrigger;

    void Start()
    {
        interactText = Camera.main.transform.Find("Canvas").Find("InteractText").GetComponent<Text>();
        interactText.enabled = false;
    }

    void Update()
    {
        if (!inTrigger)
        {
            RayTraceInteract();
        }
        if (interactable != null && Input.GetKeyDown(KeyCode.F))
        {
            interactable.Interact(this);
            OnTriggerExit(null);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
        if (other.GetComponent<IInteractable>() != null)
        {
            interactable = other.GetComponent<IInteractable>();
            interactText.text = interactable.GetInteractText();
            interactText.enabled = true;
        }

        // Powerup pickup code
        Powerup powerup = other.GetComponent<Powerup>();
        if (powerup != null)
        {
            switch (powerup.powerupType)
            {
                case PowerupType.money:
                    GetComponent<MoneyController>().AddMoney(powerup.value);
                    Destroy(other.gameObject);
                    break;
                case PowerupType.key:
                    if (!GetComponent<MoneyController>().HasKey())
                    {
                        GetComponent<MoneyController>().SetKey(true);
                        Destroy(other.gameObject);
                    }
                    break;
                case PowerupType.health:
                    GetComponent<HealthController>().Heal(powerup.value);
                    Destroy(other.gameObject);
                    break;
                case PowerupType.ammo:
                    if (GetComponent<GearController>().weapon.GetComponent<RangedAttack>().ShouldPickup())
                    {
                        GetComponent<GearController>().weapon.GetComponent<RangedAttack>().GetAmmo(powerup.value);
                        Destroy(other.gameObject);
                    }
                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
        interactText.enabled = false;
        interactable = null;
    }

    private void RayTraceInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(
            new Vector3(this.transform.position.x, 0.5f, this.transform.position.z),
            this.transform.forward,
            out hit,
            3f
            ))
        {
            if (hit.transform.GetComponent<IInteractable>() != null && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                interactable = hit.transform.GetComponent<IInteractable>();
                interactText.text = interactable.GetInteractText();
                interactText.enabled = true;
            }
            return;
        }
        interactText.enabled = false;
        interactable = null;
    }

    public void InteractableConsumed(GameObject other)
    {
        OnTriggerExit(other.GetComponent<Collider>());
    }

    public void RefreshText(GameObject other)
    {
        interactText.text = other.GetComponent<IInteractable>().GetInteractText();
    }


}