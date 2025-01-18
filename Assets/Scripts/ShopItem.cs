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

    private void AppendCart()
    {
        GridDisplay cartGroup = FindObjectOfType<GridDisplay>();

        if (cartGroup.visibleCarts < cartGroup.cartSprites.Length)
        {
            cartGroup.cartSprites[cartGroup.visibleCarts].enabled = true;
            cartGroup.visibleCarts++;
        }

        string curShape = cartGroup.shape;
        string newShape = "";

        for (int i = 0; i < curShape.Length; i++)
        {
            // add an extra X just before new line
            if (curShape[i] == '\n' || i == curShape.Length - 1)
                newShape += 'X';
            newShape += curShape[i];
        }
        cartGroup.shape = newShape;
        cartGroup.ResetShape();
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

            GridDisplay cartGroup = FindObjectOfType<GridDisplay>();

            if (upgrade == "Enlarge Cart")
            {
                // don't make size upgrade go out of bounds
                /*if (upgradeLevel >= sizeUpgrades.Length)                    
                    cartGroup.UpgradeSize(sizeUpgrades[sizeUpgrades.Length-1]);
                else
                    cartGroup.UpgradeSize(sizeUpgrades[upgradeLevel]);*/

                cartGroup.UpgradeHeight();                
            }
            else if (upgrade == "Extend Cart")
            {                                
                AppendCart();
            }
            else if (upgrade == "Line Capacity")
            {                
                Line.instance.AddZone();
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