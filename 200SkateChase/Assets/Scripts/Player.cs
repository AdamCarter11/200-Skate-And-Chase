using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Jumping variables
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
    private KeyCode[] keys = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D};
    private KeyCode[] sequence = new KeyCode[3];
    private int whichKeys1, whichKeys2, whichKeys3;
    int sequenceCounter = 0;

    void Start()
    {
        
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
            print("Key pressed");
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
            }
            if(pos.y <= groundHeight){
                //Register that you have touched the ground (reset jump)
                pos.y = groundHeight;
                isGrounded = true;
                sequenceCounter = 0;
            }
        }

        if(isGrounded){
            float velocityRatio = velocity.x / maxVelocity;
            acceleration = maxAccel * (1 - velocityRatio);
            velocity.x += acceleration * Time.fixedDeltaTime;
            
            if(velocity.x >= maxVelocity){
                velocity.x = maxVelocity;
            }
        }
        transform.position = pos;


    }
}
