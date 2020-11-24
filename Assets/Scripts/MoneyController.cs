using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    [SerializeField]
    private int amount = 0;

    [SerializeField]
    private bool key;

    private Text moneyUI;
    private Image keyUI;

    private void Awake()
    {
        moneyUI = Camera.main.transform.Find("Canvas").Find("MoneyText").GetComponent<Text>();
        keyUI = Camera.main.transform.Find("Canvas").Find("KeyGraphic").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {        
        moneyUI.text = "$" + amount + ".00";
        keyUI.enabled = key;
    }

    public int GetMoney() {
        return amount;
    }

    public void SetKey(bool keyBool)
    {
        key = keyBool;
    }

    public bool HasKey()
    {
        return key;
    }

    public void DeductMoney(int deduction) {
        amount -= deduction;
    }

    public void AddMoney(int adding) {
        amount += adding;
    }
}
