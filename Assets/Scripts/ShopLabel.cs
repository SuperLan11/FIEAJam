using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopLabel : MonoBehaviour
{
    private TextMeshProUGUI shopLabel;

    // Start is called before the first frame update
    void Start()
    {
        shopLabel = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SwapShopLabel()
    {
        if (shopLabel.text == "Open Shop")
        {
            shopLabel.text = "Close Shop";
        }
        else if (shopLabel.text == "Close Shop")
        {
            shopLabel.text = "Open Shop";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
