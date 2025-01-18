using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    private AudioSource buttonSfx;

    // Start is called before the first frame update
    void Start()
    {
        buttonSfx = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {        
        buttonSfx.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
