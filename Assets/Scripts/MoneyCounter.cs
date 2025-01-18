using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{

    private static int _money;
    public static int money
    {
        set
        {
            moneyCounter.text = "$" + value.ToString();
            _money = value;
        }
        get => _money;
    }
    private static TextMeshProUGUI moneyCounter;

    // Start is called before the first frame update
    void Start()
    {
        moneyCounter = GetComponent<TextMeshProUGUI>();
        money = 500;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
