using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int cost;
    private AudioSource buySfx;

    // Start is called before the first frame update
    void Start()
    {
        buySfx = GetComponent<AudioSource>();
    }

    private void BuyItem()
    {
        Debug.Log("mouse down");
        if (MoneyCounter.money >= cost)
        {
            MoneyCounter.SetMoney(MoneyCounter.money - cost);
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
            if(buySfx != null)
                buySfx.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
