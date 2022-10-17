using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2Controller : MonoBehaviour
{
    //the obstacle to be spawned
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject obstacle2;


    //controls how frequently the obstalce spawns (its a range)
    [HideInInspector] public static float minSpawnTime;
    [HideInInspector] public static float maxSpawnTime;
    float MouseClickInterval = 3;
   


    void Start()
    {
        /*gives us a default (useful for testing purposes)
        if (minSpawnTime == 0 || maxSpawnTime == 0)
        {
            minSpawnTime = 2;
            maxSpawnTime = 4;
        }*/

        // StartCoroutine(SpawnObstacle());
    }
    void Update()
    {
        MouseClickInterval += Time.deltaTime;
        //starts the spawning of the obstacles
        //mouse lcick spawns objects at every 2 seconds interval
        if (Input.GetMouseButtonDown(0) && MouseClickInterval>2)
        {
           
            print("MOUSE DOWN ");
            GameObject tempObs = Instantiate(obstacle, transform.position, Quaternion.identity);
            
            MouseClickInterval = 0;

        } else if (Input.GetMouseButtonDown(1) && MouseClickInterval > 2)
        {

            print("MOUSE DOWN 2 ");
            GameObject tempObs = Instantiate(obstacle2, transform.position, Quaternion.identity);
           
            MouseClickInterval = 0;

        }
    }
    IEnumerator SpawnObstacle()
    {
        //using a while true loop so that it does spawns forever
        while (true)
        {
            //picks a random value between the specified range to be used to control when to spawn the object
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            
            //
        }
    }

}
