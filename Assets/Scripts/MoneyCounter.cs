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
        money = 500;
        moneyCounter.text = "$500";
    }

    public IEnumerator MoneyRoll(float timePerNumber, int endMoney)
    {        
        yield return new WaitForSeconds(timePerNumber);
        int newMoney = int.Parse(moneyCounter.text.Substring(1)) + 1;
        // might have a bug if you send a second coaster but the money from first coaster hasn't finished tallying
        money = newMoney;        
        moneyCounter.text = "$" + newMoney.ToString();
        if(newMoney < endMoney)
            StartCoroutine(MoneyRoll(timePerNumber, endMoney));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
