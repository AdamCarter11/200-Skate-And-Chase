using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTriggerHandler : MonoBehaviour
{
    private Playerv2 player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Playerv2>();
    }

    void Update()
    {
        
    }

    // When the obstacle is close enough to the player then trigger combo prompt function
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obs"))
        {
            player.InitCombo();
        }

    }
}
