using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    //controls how fast the obstacle moves
    [HideInInspector] public static float obsSpeed;

    void Start()
    {
        //gives us a default (again, useful for testing purposes)
        if(obsSpeed == 0){
            obsSpeed = 15;
        }
        //destroys the object after six seconds (when it goes off screen)
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        //what actually moves the obstacle (may wanna consider moving to fixed update in the future)
        transform.Translate(Vector3.left * obsSpeed * Time.deltaTime, Space.World);
    }
}
