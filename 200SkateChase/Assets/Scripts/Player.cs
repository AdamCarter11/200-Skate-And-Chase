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
            }
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            isHoldingJump = false;
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
                pos.y = groundHeight;
                isGrounded = true;
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
