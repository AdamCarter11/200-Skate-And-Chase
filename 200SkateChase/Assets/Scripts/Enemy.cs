using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    [SerializeField] float minSpawnTime, maxSpawnTime;

    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    IEnumerator SpawnObstacle(){
        while(true){
            float spawnTime = Random.Range(minSpawnTime,maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
            GameObject tempObs = Instantiate(obstacle, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {

    }
}
