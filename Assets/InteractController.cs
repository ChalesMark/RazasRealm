using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    private Text buyText;
    private bool checkForInput;
    private IInteractable interactable;

    void Start() {
        buyText = Camera.main.transform.Find("Canvas").Find("BuyText").GetComponent<Text>();
        buyText.enabled = false;
    }

    void Update() {
        if(checkForInput && interactable != null && Input.GetKeyDown(KeyCode.F)) {
            interactable.Interact(this);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<IInteractable>() != null) {
            interactable = other.GetComponent<IInteractable>();
            buyText.enabled = true;
            buyText.text = interactable.GetInteractText();
            print(buyText.text);
            checkForInput = true;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        buyText.enabled = false;
        interactable = null;
        checkForInput = false;
    }

    public GameObject GetEntity() {
        return gameObject;
    }


}
