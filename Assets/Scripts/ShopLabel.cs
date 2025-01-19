using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopLabel : MonoBehaviour
{
    private TextMeshProUGUI shopLabel;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        shopLabel = GetComponentInChildren<TextMeshProUGUI>();
        anim = GetComponentInParent<Animator>();
        anim.SetTrigger("tabPressClose");
        // could set the text initially to Close Shop, but this is slightly less confusing since tablet starts off screen
        SwapShopLabel();
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
