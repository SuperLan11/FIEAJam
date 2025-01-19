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
    public TextMeshProUGUI unlockLabel;
    private GameObject endDayPanel;

    public static int dayNum = 1;
    private int moneyDisplay = 0;
    public bool endOfDay = false;


    public int[] unlockDays;
    public Sprite[] unlockSprites;
    public GameObject[] unlockPrefabs;
    public GameObject customerImagePrefab;
    public GameObject customersParent;

    // Start is called before the first frame update
    void Start()
    {
        dayLabel = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        earningsLabel = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        endDayPanel = gameObject;
    }

    public void EndDay()
    {
        endOfDay = false;
        FindObjectOfType<MouseHandler>().dragInProgress = false;
        endDayPanel.GetComponent<Image>().enabled = true;
        dayLabel.alpha = 1f;
        dayLabel.text = "Day " + dayNum + " is over";
        earningsLabel.alpha = 1f;
        unlockLabel.alpha = 1f;
        customersParent.SetActive(true);
        foreach (Transform t in customersParent.transform)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < unlockDays.Length; i++)
        {
            List<GameObject> unlocks = new();
            if (unlockDays[i] == dayNum)
            {
                GameObject obj = Instantiate(customerImagePrefab, customersParent.transform);
                obj.GetComponent<Image>().sprite = unlockSprites[i];
                unlocks.Add(unlockPrefabs[i]);
            }
            
            Line.instance.Unlock(unlocks);
        }
        
        
        //earningsLabel.text = "You earned $" + profit;
        StartCoroutine(RollMoneyResult(2, MoneyCounter.profit));
        MoneyCounter.profit = 0;
        StartCoroutine(WaitToLoad());

        FindObjectOfType<GridDisplay>().ResetShape();
    }

    public IEnumerator RollMoneyResult(float rollTime, int profit)
    {
        // the more money you get, the faster the numbers go up
        yield return new WaitForSeconds(rollTime / profit);
        moneyDisplay++;
        earningsLabel.text = "You earned $" + moneyDisplay.ToString();
        int newMoney = int.Parse(earningsLabel.text.Substring(12));
        if(newMoney < profit)
            StartCoroutine(RollMoneyResult(rollTime, profit));
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(4);
        int money = MoneyCounter.money;
        if (dayNum == 4 && PlayerPrefs.GetInt("endless") == 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            CleanScene();
            endDayPanel.GetComponent<Image>().enabled = false;
            dayLabel.alpha = 0f;            
            earningsLabel.alpha = 0f;
            unlockLabel.alpha = 0f;
            customersParent.SetActive(false);
            GameObject.Find("shiftClock").GetComponent<Clock>().enabled = true;
            GameObject.Find("shiftClock").GetComponent<Clock>().ResetClock();            

            dayNum++;            
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
