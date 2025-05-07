using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coaster : MonoBehaviour
{
    public static bool isLeaving = false;
    public static bool isReturning = false;
    public static float xSpeed = 5f;
    private Rigidbody myRig;

    // Start is called before the first frame update
    void Start()
    {
        myRig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isLeaving)
        {
            myRig.velocity = new Vector3(xSpeed, 0, 0);
        }
        else
        {
            //myRig.velocity = Vector3.zero;
        }
    }
}
