using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //the obstacle to be spawned
    [SerializeField] GameObject obstacle;

    //controls how frequently the obstalce spawns (its a range)
    [HideInInspector] public static float minSpawnTime;
    [HideInInspector] public static float maxSpawnTime;

    void Start()
    {
        //gives us a default (useful for testing purposes)
        if(minSpawnTime == 0 || maxSpawnTime == 0){
            minSpawnTime = 2;
            maxSpawnTime = 4;
        }

        //starts the spawning of the obstacles
        StartCoroutine(SpawnObstacle());
    }

    IEnumerator SpawnObstacle(){
        //using a while true loop so that it does spawns forever
        while(true){
            //picks a random value between the specified range to be used to control when to spawn the object
            float spawnTime = Random.Range(minSpawnTime,maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            GameObject tempObs = Instantiate(obstacle, transform.position, Quaternion.identity);
        }
    }

}
