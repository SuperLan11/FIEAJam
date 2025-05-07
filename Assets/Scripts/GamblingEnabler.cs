using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamblingEnabler : MonoBehaviour
{
    [SerializeField] GameObject gamblingBox;
    public void Start()
    {
        if (PlayerPrefs.GetInt("gamble") == 1)
            {
                gamblingBox.SetActive(true);
            }
    }
}