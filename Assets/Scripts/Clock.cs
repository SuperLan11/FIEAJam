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
    private float totalTime = 80f; // 2 minutes in real time
    //private float totalTime = 10f; // 4 seconds in real time
    private float updateInterval; // 5 minutes in game time converted to real time
    private float nextUpdateTime = 0f;

    void Start()
    {
        updateInterval = totalTime / (8f * 12f);
    }

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

        if (elapsedTime >= totalTime)
        {
            FindObjectOfType<DayPanel>().endOfDay = true;
            enabled = false;
        }
    }

    public void ResetClock() {
        elapsedTime = 0f;
        nextUpdateTime = 0f;
        enabled = true; // Re-enable the clock if it was disabled
    }
}