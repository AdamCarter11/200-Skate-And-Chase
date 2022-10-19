using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Playerv2 player;
    private Text distText, timeText;
    private int minutes, seconds, milliseconds;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Playerv2>();
        distText = GameObject.Find("DistanceText").GetComponent<Text>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
    }

    void Update()
    {
        distText.text = "Distance: " + Mathf.FloorToInt(player.dist).ToString() + "m";

        // Time text calculation
        minutes = (int)(Time.timeSinceLevelLoad / 60f) % 60;
        seconds = (int)Time.timeSinceLevelLoad % 60;
        milliseconds = (int)(Time.timeSinceLevelLoad * 1000f) % 1000 / 10;
        timeText.text = String.Format("Time: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        //timeText.text = "Time: "+ minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
    }
}
