using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public static int amount = 0;
    Text moneyUI;
    // Start is called before the first frame update
    void Start()
    {
        moneyUI = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyUI.text = "$" + amount + ".00";
    }

    public static bool CanBuy(IBuyable buyable)
    {
        if (buyable.Cost <= amount)
        {
            amount -= buyable.Cost;
            return true;
        }
        else
            return false;
    }
}
