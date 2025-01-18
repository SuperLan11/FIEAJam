using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private string upgradeText;
    private AudioSource buySfx;
    [SerializeField] private string upgrade;

    // the number represents the total number of tiles
    private int[] sizeUpgrades = { 5, 6, 9, 12 };
    private static int curSizeIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        buySfx = GetComponent<AudioSource>();
        transform.GetComponentInChildren<TextMeshProUGUI>().text = upgradeText + cost;
    }

    public void BuyItem()
    {        
        if (MoneyCounter.money >= cost)
        {
            MoneyCounter.money -= cost;
            cost += 5;
            transform.GetComponentInChildren<TextMeshProUGUI>().text = upgradeText + cost;
            if(buySfx != null)
                buySfx.Play();

            if(upgrade == "size")
            {
                if(curSizeIdx < sizeUpgrades.Length)
                    curSizeIdx++;
                FindObjectOfType<GridDisplay>().UpgradeSize(sizeUpgrades[curSizeIdx]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
