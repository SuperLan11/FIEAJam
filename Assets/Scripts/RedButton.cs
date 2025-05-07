using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    private AudioSource buttonSfx;
    public GridDisplay cart;

    private AudioSource[] screamSfxs;

    // Start is called before the first frame update
    void Start()
    {
        screamSfxs = FindObjectOfType<GridDisplay>().GetComponents<AudioSource>();
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

        int randIdx = Random.Range(0, screamSfxs.Length);        
        screamSfxs[randIdx].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
