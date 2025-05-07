using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Toggle endlessToggle;
    public Toggle gambleToggle;
    public void PlayGame()
    {
        SceneManager.LoadScene("LandonDev3");
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void EndlessMode()
    {
        PlayerPrefs.SetInt("endless", endlessToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void GambleMode()
    {
        PlayerPrefs.SetInt("gamble", gambleToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void Start() {
        PlayerPrefs.SetInt("endless", 0);
        PlayerPrefs.SetInt("gamble", 0);
        endlessToggle.onValueChanged.AddListener(delegate {
            EndlessMode();
        });
        gambleToggle.onValueChanged.AddListener(delegate {
            GambleMode();
        });
    }
}
