using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    
    public bool AutoInteract { get; set; } = false;
    public bool open;
    bool lastState;
    Animator Animator;
    BoxCollider boxCollider;

    public bool keyRequirement;
    public int moneyCost;
    public GameObject newEnemySpawnGroup;

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
                if (newEnemySpawnGroup != null)
                    GameObject.Find("WaveManager").GetComponent<WaveManager>().AddSpawns(newEnemySpawnGroup);
            }
        }
        else if (moneyCost <= moneyController.GetMoney())
        {
            moneyController.DeductMoney(moneyCost);
            open = true;
            if (newEnemySpawnGroup != null)
                GameObject.Find("WaveManager").GetComponent<WaveManager>().AddSpawns(newEnemySpawnGroup);
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
