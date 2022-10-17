using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObstacle : MonoBehaviour
{
    //controls how fast the obstacle moves
    [HideInInspector] public static float obsSpeed2;
    Rigidbody2D shape;
    [SerializeField] float velo;
    void Start()
    {
        shape = GetComponent<Rigidbody2D>();
        shape.velocity = (Vector2.left + Vector2.up) * velo;

        //gives us a default (again, useful for testing purposes)
        /*if (obsSpeed2 == 0)
        {
            obsSpeed2 = 15;
        }*/
        //destroys the object after six seconds (when it goes off screen)
        Destroy(gameObject, 6f);
       
    }

    void Update()
    {
        //what actually moves the obstacle (may wanna consider moving to fixed update in the future)
       // transform.Translate(Vector3.Angle() * obsSpeed2 * Time.deltaTime, Space.World);
        
    }
}
