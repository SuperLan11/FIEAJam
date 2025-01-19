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
        earningsLabel.text = "You earned $" + (MoneyCounter.money - MoneyCounter.dayStartMoney);
        StartCoroutine(WaitToLoad());
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(5);
        int money = MoneyCounter.money;
        if (dayNum == 3)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            dayNum++;
            MoneyCounter.money = money;
            GameObject.Find("MoneyCounter").GetComponent<TextMeshProUGUI>().text = "$" + MoneyCounter.money;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
