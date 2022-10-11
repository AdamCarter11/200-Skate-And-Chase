using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Player player;
    private Text distText;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distText = GameObject.Find("DistanceText").GetComponent<Text>();
    }

    void Update()
    {
        distText.text = Mathf.FloorToInt(player.dist) + "m";
    }
}
