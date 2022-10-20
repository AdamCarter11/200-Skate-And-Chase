using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Player2Controller : MonoBehaviour
{
    //the obstacle to be spawned
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject obstacle2;
    [SerializeField] GameObject obstacle3;
    [SerializeField] Playerv2 player1;

    //sequence (combo) variables
    [Header("Sequence Variables")]
    private KeyCode[] keys = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2};
    private KeyCode[] sequence = new KeyCode[4];
    //Fixed combo sequence
    private KeyCode[] Combo_Still = { KeyCode.Mouse0, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse1 };
    private KeyCode[] Combo_Bouncy = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse0, KeyCode.Mouse2 };
    private KeyCode[] Combo_Platform = { KeyCode.Mouse1, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse0 };
    int sequenceCounter = 0;
    [SerializeField] Text comboDisplay;

    //controls how frequently the obstacle spawns (its a range)
    [HideInInspector] public static float minSpawnTime;
    [HideInInspector] public static float maxSpawnTime;

    //Similar combo system like player1
    //bool inCombo = false;
    [SerializeField] float cooldown = 2f;
    float cdTimer = 0f;

    bool canAttack = true;
   


    void Start()
    {
        comboDisplay.text = "";
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
        // Combo function that will be called automatically by combo trigger
        comboHandlerFunc();

        //cooldown += Time.deltaTime;
        //starts the spawning of the obstacles
        //mouse lcick spawns objects at every 2 seconds interval
        //if (Input.GetMouseButtonDown(0) && MouseClickInterval>2)
        //{
        //} else if (Input.GetMouseButtonDown(1) && MouseClickInterval > 2)
        //{            
        //}
    }

    IEnumerator attackDelay(){
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
    // Take 3 keys in and identify which combo to trigger
    void comboHandlerFunc()
    {
        //print(cdTimer + "/" + cooldown);
        //only trigger combo when cd is good
        /*
        if (cdTimer < cooldown)
            cdTimer += Time.fixedDeltaTime;
        else if (cdTimer > cooldown)
        {
            cdTimer = cooldown;
            sequenceCounter = 0;
        }
        */
        if (canAttack)
        {
            //trigger quick time event (combo/trick system) and record input one by one using sequence
            if (Input.GetKeyDown(keys[0]) || Input.GetKeyDown(keys[1]) || Input.GetKeyDown(keys[2]))
            {
                if (Input.GetKeyDown(keys[0])){
                    sequence[sequenceCounter] = keys[0];
                    comboDisplay.text += "L-";
                }
                    
                else if (Input.GetKeyDown(keys[1])){
                    sequence[sequenceCounter] = keys[1];
                    comboDisplay.text += "R-";
                }
                    
                else if (Input.GetKeyDown(keys[2])){
                    sequence[sequenceCounter] = keys[2];
                    comboDisplay.text += "M-";
                }
                sequenceCounter++;
                print(sequenceCounter);

                if (sequenceCounter == sequence.Length)
                {
                    //bool comboMatch = true;
                    // Check if match any record
                    if (sequence[0] == Combo_Still[0] && sequence[1] == Combo_Still[1] && sequence[2] == Combo_Still[2] && sequence[3] == Combo_Still[3])
                    {
                        //print("Spawn flat obstacle");
                        cdTimer = 0f;
                        GameObject tempObs = Instantiate(obstacle, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
                        tempObs.transform.Rotate(0,0,90);
                        comboDisplay.text = "";
                        StartCoroutine(attackDelay());
                    }
                    else if (sequence[0] == Combo_Bouncy[0] && sequence[1] == Combo_Bouncy[1] && sequence[2] == Combo_Bouncy[2] && sequence[3] == Combo_Bouncy[3])
                    {
                        //print("Spawn bouncy obstacle");
                        cdTimer = 0f;
                        GameObject tempObs = Instantiate(obstacle2, transform.position, Quaternion.identity);
                        comboDisplay.text = "";
                        StartCoroutine(attackDelay());
                    }
                    else if (sequence[0] == Combo_Platform[0] && sequence[1] == Combo_Platform[1] && sequence[2] == Combo_Platform[2] && sequence[3] == Combo_Platform[3])
                    {
                        //print("Spawn Platform obstacle");
                        GameObject tempObs = Instantiate(obstacle3, transform.position, Quaternion.identity);
                        comboDisplay.text = "";
                        cdTimer = 0f;
                        StartCoroutine(attackDelay());
                    }
                    else
                    {
                        // Bad combo input
                        print("Reset player 2 sequence");
                        //comboMatch = false;
                        cdTimer = cooldown - 0.5f;
                        comboDisplay.text = "";
                    }
                    //inCombo = false;
                    sequenceCounter = 0;
                    //StopCoroutine(comboTimer());
                }
            }
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
