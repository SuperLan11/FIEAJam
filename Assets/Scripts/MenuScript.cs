using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Toggle endlessToggle;
    public Toggle gambleToggle;
    public void PlayGame()
    {
        SceneManager.LoadScene("Dev");
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
}
