using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    // don't initialize money in start so it persists over different days
    private static int _money = 10;
    public static int profit;
    public static int money
    {
        set
        {
            // commented this because it makes it harder to do the money roll effect
            //moneyCounter.text = "$" + value.ToString();
            _money = value;
        }
        get => _money;
    }
    private static TextMeshProUGUI moneyCounter;

    // Start is called before the first frame update
    void Start()
    {
        moneyCounter = GetComponent<TextMeshProUGUI>();        
        moneyCounter.text = "$" + money.ToString();
    }

    public static void MakePurchase(int cost)
    {
        money -= cost;
        moneyCounter.text = "$" + money.ToString();
    }

    public IEnumerator MoneyRoll(float rollTime, int startMoney, int endMoney)
    {        
        // the more money you get, the faster the numbers go up
        yield return new WaitForSeconds(rollTime/(endMoney-startMoney));
        int newMoney = int.Parse(moneyCounter.text.Substring(1)) + 1;        
        // might have a bug if you send a second coaster but the money from first coaster hasn't finished tallying 
        moneyCounter.text = "$" + newMoney.ToString();
        if (newMoney < endMoney)
            StartCoroutine(MoneyRoll(rollTime, startMoney, endMoney));
        else
            money = endMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
