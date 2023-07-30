using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public GameObject retryHUD;
    public PlayerController playerController;
    private float startTime;
    private bool isTimerRunning;
    private string formattedTime;

    public Text timerStopText;

    void Start()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            float timeElapsed = Time.time - startTime;
            string minutes = ((int)timeElapsed / 60).ToString("00");
            string seconds = (timeElapsed % 60).ToString("00");
            string milliseconds = ((int)(timeElapsed * 100) % 100).ToString("00");

            formattedTime = $"{minutes}:{seconds}:{milliseconds}";
            timerText.text = formattedTime;
        }
    }

    public void StopTimer()
    {
        Debug.Log("StopTimer() called");
        isTimerRunning = false;
        timerStopText.text = $"Time survived: {formattedTime}";
    }
    public string FormattedTime
    {
        get { return formattedTime; }
    }
    public float TimeElapsed
    {
        get
        {
            return Time.time - startTime;
        }
    }
}
