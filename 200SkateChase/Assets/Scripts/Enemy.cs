using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //the obstacle to be spawned
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject bouncyObs;
    [SerializeField] GameObject explosionObs;

    //controls how frequently the obstalce spawns (its a range)
    [HideInInspector] public static float minSpawnTime;
    [HideInInspector] public static float maxSpawnTime;

    //player for distance
    [SerializeField] Transform player;
    private float distance;

    Animator playerAnim;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        //gives us a default (useful for testing purposes)
        if(minSpawnTime == 0 || maxSpawnTime == 0){
            minSpawnTime = 2;
            maxSpawnTime = 4;
        }

        //starts the spawning of the obstacles
        StartCoroutine(SpawnObstacle());
    }

    /*void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= 16f)
            maxSpawnTime = 3;
        if (distance <= 10f)
            minSpawnTime = 1;
        if (distance <= 6f)
            maxSpawnTime = 2;
        if (distance <= 3f)
            maxSpawnTime = 1;

    }*/

    IEnumerator SpawnObstacle(){
        //using a while true loop so that it does spawns forever
        while(true){
            //picks a random value between the specified range to be used to control when to spawn the object
            float spawnTime = Random.Range(minSpawnTime,maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            int rando = Random.Range(0,2);
            if(rando == 0)
            {
                playerAnim.SetTrigger("Throw");
                GameObject tempObs = Instantiate(obstacle, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
                tempObs.transform.Rotate(0,0,90);
            }
            else if(rando == 1)
            {
                playerAnim.SetTrigger("Throw");
                GameObject tempObs = Instantiate(bouncyObs, transform.position, Quaternion.identity);
            }
            else if(rando == 2)
            {
                playerAnim.SetTrigger("Throw");
                //currently not being used
                GameObject tempObs = Instantiate(explosionObs, transform.position, Quaternion.identity);
            }
            
        }
    }

}
