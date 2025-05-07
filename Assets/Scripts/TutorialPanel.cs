using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{    
    [SerializeField] private GameObject nextBtn;    

    public List<GameObject> upgrades = new List<GameObject>();    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowUpgrades()
    {
        Destroy(nextBtn.gameObject);
        Destroy(this.gameObject);
       
        foreach(GameObject upgrade in upgrades)
        {
            upgrade.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

