using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum OBSTACLE_TYPE
{
    FLAT,
    BOUNCY,
    HORIZONTAL_LOOP
};

public class Obstacles : MonoBehaviour
{
    //controls how fast the obstacle moves
    [HideInInspector] public static float obsSpeed;

    //What Type of obstacle it is
    [SerializeField] OBSTACLE_TYPE obsType;
    Rigidbody2D shape;
    [SerializeField] float velo;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    //float horizontalTimer = 0f;
    //[SerializeField] float switchTime = 2f;


    void Start()
    {
        if (obsType == OBSTACLE_TYPE.BOUNCY)
        {
            shape = GetComponent<Rigidbody2D>();
            shape.velocity = (Vector2.left + Vector2.up) * velo;
        }

        //gives us a default (again, useful for testing purposes)
        if (obsType == OBSTACLE_TYPE.FLAT)
        {
            if (obsSpeed == 0)
            {
                obsSpeed = 15;
            }
        }
        //destroys the object after six seconds (when it goes off screen)
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        /*horizontalTimer += Time.fixedDeltaTime;
        if(horizontalTimer >= switchTime)
        {
            if (forward)
                forward = false;
            else
                forward = true;
            horizontalTimer = 0f;
        }*/


        //what actually moves the obstacle (may wanna consider moving to fixed update in the future)
        if (obsType == OBSTACLE_TYPE.FLAT)
        {
            transform.Translate(Vector3.left * obsSpeed * Time.fixedDeltaTime, Space.World);
        }
        else if (obsType == OBSTACLE_TYPE.BOUNCY)
        {
            //
        }
        else if (obsType == OBSTACLE_TYPE.HORIZONTAL_LOOP)
        {
            /* if (forward)
             {
                 transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x - 2.5f, velo), transform.position.y, transform.position.z);
             }
             else
             {
                 transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + 2.5f, velo), transform.position.y, transform.position.z);
             }*/
            transform.position = new Vector3(Mathf.Lerp(transform.position.x- amplitude, transform.position.x + amplitude, Mathf.PingPong(Time.time* frequency, 1)), transform.position.y, transform.position.z);
            transform.Translate(Vector3.left * velo * Time.fixedDeltaTime, Space.World);
        }
    }
}
