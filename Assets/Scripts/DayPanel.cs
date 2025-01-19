using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayPanel : MonoBehaviour
{
    private TextMeshProUGUI dayLabel;
    private TextMeshProUGUI earningsLabel;
    private GameObject endDayPanel;

    public static int dayNum = 1;
    private int moneyDisplay = 0;

    // Start is called before the first frame update
    void Start()
    {
        dayLabel = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        earningsLabel = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        endDayPanel = gameObject;
    }

    public void EndDay()
    {        
        endDayPanel.GetComponent<Image>().enabled = true;
        dayLabel.alpha = 1f;
        dayLabel.text = "Day " + dayNum + " is over";
        earningsLabel.alpha = 1f;
        int profit = (MoneyCounter.money - MoneyCounter.dayStartMoney);
        //earningsLabel.text = "You earned $" + profit;
        StartCoroutine(RollMoneyResult(2, profit));
        StartCoroutine(WaitToLoad());
    }

    public IEnumerator RollMoneyResult(float rollTime, int profit)
    {
        // the more money you get, the faster the numbers go up
        yield return new WaitForSeconds(rollTime / profit);
        moneyDisplay++;
        earningsLabel.text = "You earned $" + moneyDisplay.ToString();
        int newMoney = int.Parse(earningsLabel.text.Substring(12)) + 1;        
        if(newMoney < profit)
            StartCoroutine(RollMoneyResult(rollTime, profit));
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(4);
        int money = MoneyCounter.money;
        if (dayNum == 3)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            CleanScene();
            endDayPanel.GetComponent<Image>().enabled = false;
            dayLabel.alpha = 0f;            
            earningsLabel.alpha = 0f;
            GameObject.Find("shiftClock").GetComponent<Clock>().enabled = true;            
            dayNum++;
            //GameObject.Find("MoneyCounter").GetComponent<TextMeshProUGUI>().text = "$" + MoneyCounter.money;
        }
    }

    private void CleanScene()
    {
        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            Destroy(monster.gameObject);
        }
        Line.instance.queue.Clear();
        FindObjectOfType<GridDisplay>().ResetShape();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
