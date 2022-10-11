using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float depth = 1;
    private Player player;

    private void Awake() {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float realVel = player.velocity.x / depth;
        Vector2 pos = transform.position;
        pos.x -= realVel * Time.fixedDeltaTime;

        if(pos.x < - 40){
            pos.x = 40;
        }
        transform.position = pos;
    }
}
