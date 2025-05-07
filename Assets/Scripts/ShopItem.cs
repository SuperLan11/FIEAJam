using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private int cost;
    private TextMeshProUGUI costLabel;    
    private AudioSource buySfx;
    private string upgrade;
    private TextMeshProUGUI levelLabel;    
        
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
            GridDisplay cartGroup = FindObjectOfType<GridDisplay>();
            if (upgrade == "Enlarge Cart" || upgrade == "Extend Cart") 
            {
                if (!cartGroup.canSend)
                {
                    return;
                }            
            }
            ColorBlock colors = GetComponent<Button>().colors;
            colors.pressedColor = Color.grey;

            MoneyCounter.MakePurchase(cost);
            if (upgrade != "Surprise Box") {
                cost += (int)(cost * 0.33f);
            } 
            costLabel.text = "$" + cost.ToString();

            if(buySfx != null)
                buySfx.Play();

            upgradeLevel++;
            levelLabel.text = "LV " + upgradeLevel;


            if (upgrade == "Enlarge Cart")
            {             
                cartGroup.Send(()=>
                {
                    cartGroup.UpgradeHeight();
                });                
            }
            else if (upgrade == "Extend Cart")
            {
                cartGroup.Send(() =>
                {
                    cartGroup.AppendCart();
                });
            }
            else if (upgrade == "Line Capacity")
            {                
                Line.instance.AddZone();
            }
            else if (upgrade == "Surprise Box")
            {
                Debug.Log("surprise box");
                Debug.Log(RollLootBox());
            }
            else if (upgrade == "Buy Disney")
            {
                Debug.Log("disney");
            }       
        }        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Button btn in GetComponentsInChildren<Button>())
        {
            if (btn.GetComponent<ShopItem>().cost >= MoneyCounter.money)
                btn.interactable = false;
            else
                btn.interactable = true;
        }
    }
    
    public string RollLootBox()
    {
        int roll = Random.Range(1, 100);
        if (roll <= 10) {
            return "Whoa! Legendary!"; // legendary
            //yellow
        } else if (11 <= roll && roll <= 20) {
            return "Epic!"; // epic
            //pink, aqua
        } else if (21 <= roll && roll <= 50) {
            return "Rare!"; // rare
            //blue, orange, purple
        } else if (51 <= roll && roll <= 100) {
            return "A common..."; // common
            //grey, red, green
        }
        return "roll test"; // default case
    }
}