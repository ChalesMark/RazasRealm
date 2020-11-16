using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    [SerializeField]
    private int amount = 0;
    private Text moneyUI;

    private void Awake()
    {
        moneyUI = Camera.main.transform.Find("Canvas").Find("MoneyText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {        
        moneyUI.text = "$" + amount + ".00";
    }

    public int GetMoney() {
        return amount;
    }

    public void DeductMoney(int deduction) {
        amount -= deduction;
    }

    public void AddMoney(int adding) {
        amount += adding;
    }
}
