using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Pick up Gold Coin";
    }

    public void Interact(InteractController interactController)
    {
        interactController.InteractableConsumed(gameObject);
        Destroy(gameObject);
    }
}
