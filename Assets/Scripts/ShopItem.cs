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
                FindObjectOfType<GridDisplay>().UpgradeSize(6);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
