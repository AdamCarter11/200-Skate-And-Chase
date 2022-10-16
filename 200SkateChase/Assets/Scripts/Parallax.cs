using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    /*
    *       To change the sprites they are displayed in:
    *           in the inspector, under the spriterenderer component
    *           there is an order in layer value, the more negative it is
    *           The further back it will be renderered (0 is default, -1 would be behind that)
    */

    //Depth controls the speed (closer to 0, the faster it goes)
    [SerializeField] float depth = 1;
    private Playerv2 player;

    //used to set the background to move according to the player
    private void Awake() {
        player = GameObject.Find("Player").GetComponent<Playerv2>();
    }

    void FixedUpdate()
    {
        //what actually moves the sprites
        float realVel = player.velocity.x / depth;
        Vector2 pos = transform.position;
        pos.x -= realVel * Time.fixedDeltaTime;

        //resets the sprites position when getting too far
        if(pos.x < - 40){
            pos.x = 40;
        }
        transform.position = pos;
    }
}
