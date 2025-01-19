using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    public TMP_Text clockText;
    private int startHour = 9;
    private int startMinute = 0;
    private int endHour = 17;
    private int endMinute = 0;
    private float elapsedTime = 0f;
    private float totalTime = 120f; // 2 minutes in real time
    //private float totalTime = 4f; // 4 seconds in real time
    private float updateInterval = 120f / (8f * 12f); // 5 minutes in game time converted to real time
    private float nextUpdateTime = 0f;

    void Update() {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= nextUpdateTime) 
        {
            float timePercent = Mathf.Clamp(elapsedTime / totalTime, 0f, 1f);
            int currentHour = Mathf.FloorToInt(Mathf.Lerp(startHour, endHour, timePercent));
            int currentMinute = Mathf.FloorToInt(Mathf.Lerp(startMinute, endMinute, timePercent) / 5) * 5;

            string period = currentHour >= 12 ? "PM" : "AM";
            currentHour = currentHour % 12;
            if (currentHour == 0) currentHour = 12;

            string formattedTime = string.Format("{0:D2}:{1:D2} {2}", currentHour, currentMinute, period);

            if (clockText != null) 
            {
                clockText.text = formattedTime;                
            }

            nextUpdateTime += updateInterval;
        }

        if (elapsedTime >= totalTime) {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            FindObjectOfType<DayPanel>().EndDay();
            enabled = false;
        }
    }
}