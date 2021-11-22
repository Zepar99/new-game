using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class Stopwatch : MonoBehaviour
{
    public bool stopWatchActive = true;
    public float currentTime;
    public Text currentTimeText;
    public highscore high;

    void Update()
    {
        if (stopWatchActive == true)
        {
            currentTime = currentTime - Time.deltaTime;
            if (currentTime <= 0)
            {
                stopTime();
            }
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00") + ":" + time.Milliseconds.ToString("000");
    }
    public void stopTime()
    {
        stopWatchActive = false;
        high.Update();

    }

}
