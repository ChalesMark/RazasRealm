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

    void Start() {
        interactText = Camera.main.transform.Find("Canvas").Find("InteractText").GetComponent<Text>();
        interactText.enabled = false;
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
            interactText.enabled = true;
            interactText.text = interactable.GetInteractText();
            checkForInput = true;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        interactText.enabled = false;
        interactable = null;
        checkForInput = false;
    }

    public GameObject GetEntity() {
        return gameObject;
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
