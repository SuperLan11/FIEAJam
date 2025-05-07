using UnityEngine;
using UnityEngine.SceneManagement;

public class Gambling : MonoBehaviour
{
    public void Start()
    {
        if (PlayerPrefs.GetInt("gamble") == 1)
        {
            SceneManager.LoadScene("Gamble");
        }
    }

    private float gachapon(float roll) {
        if (roll <= 10) {
            return 1.0f; // legendary
        } else if (11 <= roll && roll <= 20) {
            return 0.8f; // epic
        } else if (21 <= roll && roll <= 50) {
            return 0.5f; // rare
        } else if (51 <= roll && roll <= 100) {
            return 0.2f; // common
        }
        return 0.0f; // default case
    }
}