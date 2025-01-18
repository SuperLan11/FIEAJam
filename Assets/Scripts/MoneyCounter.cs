using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [System.NonSerialized] public static int money = 0;
    private static TextMeshProUGUI moneyCounter;

    // Start is called before the first frame update
    void Start()
    {
        moneyCounter = FindObjectOfType<MoneyCounter>().GetComponent<TextMeshProUGUI>();
        SetMoney(30);
    }

    public static void SetMoney(int newMoney)
    {
        money = newMoney;
        moneyCounter.text = "Money: " + newMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
