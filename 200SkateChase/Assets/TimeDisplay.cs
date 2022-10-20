using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        // Time text calculation
        float matchTime = PlayerPrefs.GetFloat("GameTime", 0f); ;
        int minutes = (int)(matchTime / 60f) % 60;
        int seconds = (int)matchTime % 60;
        int milliseconds = (int)(matchTime * 1000f) % 1000 / 10;
        timeText.text = String.Format("Match Time: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
