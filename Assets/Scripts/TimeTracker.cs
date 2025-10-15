using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeTracker : MonoBehaviour
{
    // Public variable to assign a UI Text component to display the time
    public TextMeshProUGUI timeText;

    private float startTime;
    private bool isRunning = false;

    void Start()
    {
        // Automatically start the timer when the game starts
        StartTimer();
    }

    void Update()
    {
        // Update the timer if it is running
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;
            DisplayTime(elapsedTime);
        }
    }

    // Method to start the timer
    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    // Method to stop the timer
    public void StopTimer()
    {
        isRunning = false;
    }

    // Method to reset the timer
    public void ResetTimer()
    {
        startTime = Time.time;
        isRunning = false;
        DisplayTime(0);
    }

    // Method to format and display the time on the UI
    private void DisplayTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}