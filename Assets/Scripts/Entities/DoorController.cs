using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public bool open;
    bool lastState;
    Animator Animator;
    BoxCollider boxCollider;

    public bool keyRequirement;
    public int moneyCost;

    void Start()
    {
        open = false;
        lastState = false;
        Animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastState != open)
        {
            lastState = open;
            Animator.SetBool("open", lastState);
            boxCollider.enabled = !open;
        }
    }

    public void Interact(InteractController interactController)
    {
        MoneyController moneyController = interactController.gameObject.GetComponent<MoneyController>();
        if (keyRequirement)
        {
            if (moneyController.HasKey())
            {
                moneyController.SetKey(false);
                open = true;
            }
        }
        else if (moneyCost <= moneyController.GetMoney())
        {
            moneyController.DeductMoney(moneyCost);
            open = true;
        }
    }

    public string GetInteractText()
    {
        if (keyRequirement)
            return "This door requires a key!";
        else
            return "Spend $"+moneyCost+" to open this door";
    }
}
