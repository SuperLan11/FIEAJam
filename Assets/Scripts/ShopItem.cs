using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    private int cost;
    private TextMeshProUGUI costLabel;    
    private AudioSource buySfx;
    private string upgrade;
    private TextMeshProUGUI levelLabel;
    [SerializeField] private GameObject cartPrefab;

    // the number represents the total number of tiles
    private int[] sizeUpgrades = { 8, 12, 16, 25 };
    private int upgradeLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        buySfx = GetComponent<AudioSource>();
        costLabel = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        // excludes $ character
        cost = int.Parse(costLabel.text.Substring(1));
        upgrade = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        levelLabel = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void BuyItem()
    {        
        if (MoneyCounter.money >= cost)
        {
            MoneyCounter.money -= cost;
            cost += 5;
            costLabel.text = "$" + cost.ToString();

            if(buySfx != null)
                buySfx.Play();

            upgradeLevel++;
            levelLabel.text = "LV " + upgradeLevel;

            if (upgrade == "Enlarge Cart")
            {
                // don't make size upgrade go out of bounds
                if (upgradeLevel >= sizeUpgrades.Length)                    
                    FindObjectOfType<GridDisplay>().UpgradeSize(sizeUpgrades[sizeUpgrades.Length-1]);
                else
                    FindObjectOfType<GridDisplay>().UpgradeSize(sizeUpgrades[upgradeLevel]);
            }
            else if (upgrade == "Extend Cart")
            {
                Debug.Log("more carts");                
                //Instantiate(cartPrefab, )
            }
            else if (upgrade == "Line Capacity")
            {
                Debug.Log("more line");
            }
            else if (upgrade == "Antighost")
            {
                Debug.Log("antighost");
            }
            else if (upgrade == "Buy Disney")
            {
                Debug.Log("disney");
            }
            else if (upgrade == "More Rides")
            {
                Debug.Log("more rides");
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}