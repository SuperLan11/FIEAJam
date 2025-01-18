using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [System.NonSerialized] public int money = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetMoney(5);
    }

    public void SetMoney(int newMoney)
    {
        money = newMoney;
        GetComponent<TextMeshProUGUI>().text = "Money: " + newMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
