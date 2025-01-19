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
                Debug.Log("Loot Box");
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
}