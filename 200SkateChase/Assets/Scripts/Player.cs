using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Jumping variables
    [Header("Jump variables")]
    [SerializeField] float gravity;
    [SerializeField] float maxJumpHold;
    [SerializeField] float jumpGroundThreshold = 1;
    [SerializeField] float jumpVelocity;
    [SerializeField] float groundHeight;
    [SerializeField] float acceleration;
    [SerializeField] float maxAccel;
    [SerializeField] float maxVelocity;
    [HideInInspector] public float dist;
    [HideInInspector] public Vector2 velocity;
    private bool isGrounded;
    private bool isHoldingJump; 
    private float holdJumpTimer = 0;

    //sequence (combo) variables
    [Header("Sequence Variables")]
    private KeyCode[] keys = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D};
    private KeyCode[] sequence = new KeyCode[3];
    private int whichKeys1, whichKeys2, whichKeys3;
    int sequenceCounter = 0;

    //Combo effect variables
    [Header("Combo effect variables")]
    [SerializeField] Camera cam;
    [SerializeField] float zoomIn, zoomOut;
    private float startinCamSize;

    [SerializeField] float knockbackVal;
    [SerializeField] float knockBackSpeed;
    private bool hit = false;
    Vector2 tempVec;

    void Start()
    {
        startinCamSize = cam.orthographicSize;
    }

    void Update()
    {
        //triggering the jump
        Vector2 pos = transform.position;
        float groundDist = Mathf.Abs(pos.y-groundHeight);
        if(isGrounded || groundDist <= jumpGroundThreshold){
            if(Input.GetKeyDown(KeyCode.Space)){
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
                whichKeys1 = Random.Range(0,keys.Length);
                whichKeys2 = Random.Range(0,keys.Length);
                whichKeys3 = Random.Range(0,keys.Length);
                sequence[0] = keys[whichKeys1];
                sequence[1] = keys[whichKeys2];
                sequence[2] = keys[whichKeys3];
                //sequenceCounter = 0;
                //print(sequence.Length);
                print(sequence[0] + " " + sequence[1] + " " + sequence[2]);
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            isHoldingJump = false;
        }

        //trigger quick time event (combo/trick system)
        if(Input.GetKeyDown(sequence[sequenceCounter])){
            //print("Key pressed");
            sequenceCounter++;
            print(sequenceCounter);
            if(sequenceCounter == sequence.Length){
                //sequence met
                print("COMBO ACTIVATED");
                sequenceCounter = 0;
            }
        }
        else if(Input.anyKeyDown){
            sequenceCounter = 0;
            print("Reset sequence");
        }
        if(hit){
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(tempVec.x-knockbackVal, tempVec.y, transform.position.z), Time.deltaTime*knockBackSpeed);
            //transform.position = Vector3.Lerp(tempVec, new Vector2(tempVec.x-4, tempVec.y), Time.deltaTime * 2);
            if(transform.position.x <= tempVec.x - 4f){
                hit = false;
                print("FINISH HIT");
            }
        }
    }

    private void FixedUpdate() {
        dist += velocity.x * Time.fixedDeltaTime;
        //actually doing the jump
        Vector2 pos = transform.position;
        if(!isGrounded){
            if(isHoldingJump){
                holdJumpTimer += Time.fixedDeltaTime;
                if(holdJumpTimer >= maxJumpHold){
                    isHoldingJump = false;
                }
            }
            pos.y += velocity.y * Time.fixedDeltaTime;
            if(!isHoldingJump){
                velocity.y += gravity * Time.fixedDeltaTime;
                Time.timeScale = Mathf.Lerp(Time.timeScale, .3f, Time.deltaTime*4);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, Time.deltaTime*zoomIn);
                cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x, Time.deltaTime*zoomIn), 0, -10);
                //print(cam.orthographicSize);
                //Time.timeScale = .3f;
            }
            if(pos.y <= groundHeight){
                //Register that you have touched the ground (reset jump)
                pos.y = groundHeight;
                isGrounded = true;
                sequenceCounter = 0;
                Time.timeScale = 1;
                //cam.orthographicSize = 10;
                //cam.orthographicSize = Mathf.Lerp(5, startinCamSize, Time.deltaTime*zoomIn);
            }
        }

        if(isGrounded){
            float velocityRatio = velocity.x / maxVelocity;
            acceleration = maxAccel * (1 - velocityRatio);
            velocity.x += acceleration * Time.fixedDeltaTime;
            
            if(velocity.x >= maxVelocity){
                velocity.x = maxVelocity;
            }
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, startinCamSize, Time.deltaTime*zoomOut);
            cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, 0, Time.deltaTime*zoomOut), 0, -10);
        }
        transform.position = pos;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Obs")){
            //print("Collision");
            velocity.x = Mathf.Lerp(velocity.x, -5f, 2);
            cam.GetComponent<ScreenShake>().TriggerShake();
            cam.GetComponent<ScreenShake>().initialPos = cam.transform.position;
            hit = true;
            tempVec = transform.position;
        }
    }
}
