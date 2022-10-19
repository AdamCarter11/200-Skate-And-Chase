using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RandomMovementObstacle : MonoBehaviour
{
    //controls how fast the obstacle moves
    //[HideInInspector] public static float randObsSpeed;
    public float randomObsSpeed;
    //public Vector3 randomDirection = Vector3.zero; // (0,0,0)
    public Vector3 randomDirection = Vector3.left;
    bool moving = false;
    Vector3 destination;

    void Start()
    {
        
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        //what actually moves the obstacle (may wanna consider moving to fixed update in the future)
        //transform.Translate(Vector3.left * randObsSpeed * Time.deltaTime, Space.World);
        if(!moving)
        {
            StartCoroutine(changedirection()); // changes direction
        }

        destination = transform.position + randomDirection * 2;//direction * distance, to get the destination
        //transform.position += randomDirection * randomObsSpeed;
        transform.position=Vector3.Lerp(transform.position, destination, Time.deltaTime);
    }

    IEnumerator changedirection()
    {
        moving = true;
        yield return new WaitForSeconds(1);
        randomDirection.x = Random.Range(-5, 2);
        //randomDirection.y = Random.Range(-1, 2);
        moving = false;

    }

}
