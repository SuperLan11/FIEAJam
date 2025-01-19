using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    private AudioSource buttonSfx;
    public GridDisplay cart;

    // Start is called before the first frame update
    void Start()
    {
        buttonSfx = GetComponent<AudioSource>();
    }

    public void Click()
    {
        if (!cart.canSend)
        {
            return;
        }
        buttonSfx.Play();
        Coaster.isLeaving = true;
        cart.Send(() => { });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
