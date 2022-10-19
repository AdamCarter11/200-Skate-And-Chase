using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Playerv2 : MonoBehaviour
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
    [SerializeField] float crouchTime;
    [SerializeField] float crouchPosY;
    [SerializeField] float crouchLerpCoef;
    [HideInInspector] public float dist;
    [HideInInspector] public Vector2 velocity;
    private bool inCombo = false;
    private bool isCrouching = false;
    private bool isGrounded;
    private bool isHoldingJump; 
    private float holdJumpTimer = 0;
    private float crouchTimer = 0;

    //sequence (combo) variables
    [Header("Sequence Variables")]
    private KeyCode[] keys = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D};
    private KeyCode[] sequence = new KeyCode[3];
    //Fixed combo sequence
    private KeyCode[] Combo_JumpH = { KeyCode.Q, KeyCode.W, KeyCode.E };
    private KeyCode[] Combo_JumpL = { KeyCode.A, KeyCode.W, KeyCode.D };
    private KeyCode[] Combo_Crouch = { KeyCode.Q, KeyCode.S, KeyCode.E };
    private int whichKeys1, whichKeys2, whichKeys3;
    int sequenceCounter = 0;

    //Combo effect variables
    [Header("Combo effect variables")]
    [SerializeField] Camera cam;
    [SerializeField] float zoomIn, zoomOut;
    private float startinCamSize;
    private Vector3 startCamPos;
    //Damage variables
    [SerializeField] float knockbackVal;
    [SerializeField] float knockBackSpeed;
    [SerializeField] float moveForwardVal;
    [SerializeField] float moveForwardSpeed;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] Text comboText;
    private bool hit = false, hitTimer = false, landed = false, comboMove = false;
    Vector2 tempVec;
    private int combCount = 0;
    [SerializeField] GameObject enemyObj;

    //get hit sound and components
    public AudioClip hurtSound;
    private AudioSource aS;

    void Start()
    {
        //initially setting the cameras size and position
        //(we will be changing later so we want to have a default to fall back on)
        startinCamSize = cam.orthographicSize;
        startCamPos = cam.transform.position;
        //get audio source for hit sound
        aS = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        //all the jumping logic 
        //jumpingFunc();

        //all the logic behind keeping track of the players trick and combo
        //comboFunc();

        // Combo function that will be called automatically by combo trigger
        comboHandlerFunc();

        //logic behind what happens when the player gets hit
        damageFunc();

        //progress function (move forward)
        moveForward();

        //GameOver logic
        checkGameOver();

        //win logic
        checkWin();
    }

    private void FixedUpdate() {
        //activateJump();
        jumpHandler();
        crouchHandler();
        cameraHandler();
    }

    //Checks for collision with obstacle (NOTICE HOW ITS TRIGGER)
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Obs")){
            //effects the velocity display
            velocity.x = Mathf.Lerp(velocity.x, -5f, 2);

            //used for the camera shake
            cam.GetComponent<ScreenShake>().TriggerShake();
            cam.GetComponent<ScreenShake>().initialPos = cam.transform.position;

            //used to represent being hit
            tempVec = transform.position;
            hit = true;
            combCount = 0;
            comboText.color = Color.black;
            comboText.text = "Combo: " + combCount;
            StartCoroutine(JumpDelay());

            // if in combo reset combo cam
            inCombo = false;


            //creates particle effect and then destroys the obstacle that hit the player
            Instantiate(explosionEffect, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            //plays sound of player getting hit
            aS.PlayOneShot(hurtSound);

        }

    }

    //used to prevent the player from jumping for a little bit after getting hit
    IEnumerator JumpDelay(){
        hitTimer = true;
        yield return new WaitForSeconds(.5f);
        hitTimer = false;
    }

    /*      UPDATE FUNCTIONS
    *
    */
    void jumpingFunc(){
        //triggering the jump
        Vector2 pos = transform.position;
        float groundDist = Mathf.Abs(pos.y-groundHeight);
        if(isGrounded || groundDist <= jumpGroundThreshold){
            if(Input.GetKeyDown(KeyCode.Space) && !hitTimer){
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
    }

    void comboFunc(){
        //trigger quick time event (combo/trick system)
        if(Input.GetKeyDown(sequence[sequenceCounter])){
            //print("Key pressed");
            sequenceCounter++;
            print(sequenceCounter);
            if(sequenceCounter == sequence.Length){
                //sequence met
                print("COMBO ACTIVATED");
                combCount++;
                if(combCount >= 1){
                    float tempColorVal = combCount * .1f;;
                    print(tempColorVal);
                    if(tempColorVal > 1){
                        tempColorVal = 1;
                    }
                    comboText.color = new Color(tempColorVal, tempColorVal, 0);
                }
                else{
                    comboText.color = Color.black;
                }
                comboText.text = "Combo: " + combCount;
                tempVec = transform.position;
                landed = true;
                sequenceCounter = 0;
            }
        }
        else if(!Input.GetKeyDown(KeyCode.Space) && Input.anyKeyDown){
            sequenceCounter = 0;
            combCount = 0;
            comboText.color = Color.black;
            comboText.text = "Combo: " + combCount;
            print("Reset sequence");
        }
    }

    // Take 3 keys in and identify which combo to trigger
    void comboHandlerFunc()
    {
        if (inCombo)
        {
            //trigger quick time event (combo/trick system) and record input one by one using sequence
            if (Input.GetKeyDown(keys[0]) || Input.GetKeyDown(keys[1]) || Input.GetKeyDown(keys[2]) || Input.GetKeyDown(keys[3]) || Input.GetKeyDown(keys[4]) || Input.GetKeyDown(keys[5]))
            {
                if (Input.GetKeyDown(keys[0]))
                    sequence[sequenceCounter] = keys[0];
                else if (Input.GetKeyDown(keys[1]))
                    sequence[sequenceCounter] = keys[1];
                else if (Input.GetKeyDown(keys[2]))
                    sequence[sequenceCounter] = keys[2];
                else if (Input.GetKeyDown(keys[3]))
                    sequence[sequenceCounter] = keys[3];
                else if (Input.GetKeyDown(keys[4]))
                    sequence[sequenceCounter] = keys[4];
                else if (Input.GetKeyDown(keys[5]))
                    sequence[sequenceCounter] = keys[5];
                sequenceCounter++;
                print(sequenceCounter);

                if (sequenceCounter == sequence.Length)
                {
                    bool comboMatch = true;
                    // Check if match any record
                    if (sequence[0] == Combo_JumpH[0] && sequence[1] == Combo_JumpH[1] && sequence[2] == Combo_JumpH[2])
                    {
                        //print("Jump High Combo Activated");
                        float groundDist = Mathf.Abs(transform.position.y - groundHeight);
                        if (isGrounded || groundDist <= jumpGroundThreshold)
                        {
                            isGrounded = false;
                            velocity.y = jumpVelocity * 1.5f;
                        }
                    }
                    else if (sequence[0] == Combo_JumpL[0] && sequence[1] == Combo_JumpL[1] && sequence[2] == Combo_JumpL[2])
                    {
                        //print("Jump Low Combo Activated");
                        float groundDist = Mathf.Abs(transform.position.y - groundHeight);
                        if (isGrounded || groundDist <= jumpGroundThreshold)
                        {
                            isGrounded = false;
                            velocity.y = jumpVelocity * 0.7f;
                        }
                    }
                    else if (sequence[0] == Combo_Crouch[0] && sequence[1] == Combo_Crouch[1] && sequence[2] == Combo_Crouch[2])
                    {
                        //print("Crouch Combo Activated");
                        isCrouching = true;
                        crouchTimer = 0f;
                    }
                    else
                    {
                        comboMatch = false;
                    }

                    if (comboMatch)
                    {
                        combCount++;
                        if (combCount >= 1)
                        {
                            float tempColorVal = combCount * .1f; ;
                            print(tempColorVal);
                            if (tempColorVal > 1)
                            {
                                tempColorVal = 1;
                            }
                            comboText.color = new Color(tempColorVal, tempColorVal, 0);
                        }
                        else
                        {
                            comboText.color = Color.black;
                        }
                        comboText.text = "Combo: " + combCount;
                        tempVec = transform.position;
                        //landed = true;
                        comboMove = true;
                    }
                    inCombo = false;
                    sequenceCounter = 0;
                }
            }
            else if (Input.anyKeyDown)
            {
                sequenceCounter = 0;
                combCount = 0;
                comboText.color = Color.black;
                comboText.text = "Combo: " + combCount;
                print("Reset sequence");
            }
        }
        
    }

    void damageFunc(){
        if (hit){
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(tempVec.x - knockbackVal, transform.position.y, transform.position.z), Time.deltaTime * knockBackSpeed);
            if (transform.position.x == tempVec.x - knockbackVal){
                hit = false;
            }
        }
    }

    void moveForward(){
        if (comboMove){
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(tempVec.x+moveForwardVal, transform.position.y, transform.position.z), Time.deltaTime*moveForwardSpeed);
            if(transform.position.x == tempVec.x + moveForwardVal){
                comboMove = false;
            }
        }
    }

    public void InitCombo(){
        if(isGrounded && !isCrouching)
        {
            inCombo = true;
            print("Perform the combo now!");
        }
    }

    void checkGameOver(){
        if(transform.position.x < -18){
            print("GAME OVER");
            SceneManager.LoadScene("GameOver");
        }
    }

    void checkWin(){
        if(transform.position.x >= enemyObj.transform.position.x){
            //win stats
            print("YOU WIN");
            int tempWinCount = PlayerPrefs.GetInt("Wins");
            tempWinCount++;
            PlayerPrefs.SetInt("Wins", tempWinCount);
            SceneManager.LoadScene("WinScreen");
        }
    }

    /*     FIXED UPDATE FUNCTIONS
    *
    */

    void jumpHandler()
    {
        Vector2 pos = transform.position;
        if (!isGrounded)
        {
            pos.y += velocity.y * Time.fixedDeltaTime;
            velocity.y += gravity * Time.fixedDeltaTime;
            //Time.timeScale = .3f;
            if (pos.y <= groundHeight)
            {
                //Register that you have touched the ground (reset jump)
                pos.y = groundHeight;
                isGrounded = true;
                sequenceCounter = 0;
            }
        }
        else
        {
            float velocityRatio = velocity.x / maxVelocity;
            acceleration = maxAccel * (1 - velocityRatio);
            velocity.x += acceleration * Time.fixedDeltaTime;

            if (velocity.x >= maxVelocity)
            {
                velocity.x = maxVelocity;
            }
        }
        transform.position = pos;
    }
    void crouchHandler()
    {
        if (isCrouching)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, crouchPosY, Time.deltaTime * crouchLerpCoef), transform.position.z);
            crouchTimer += Time.fixedDeltaTime;
            if (crouchTimer >= crouchTime)
            {
                Debug.Log("Crouch Ended");
                crouchTimer = 0;
                isCrouching = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0f, Time.deltaTime * crouchLerpCoef), transform.position.z);
        }
    }

    void cameraHandler()
    {
        // UI Purpose
        dist += velocity.x * Time.fixedDeltaTime;

        if (inCombo)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, .3f, Time.deltaTime * 4);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, Time.deltaTime * zoomIn);
            cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x, Time.deltaTime * zoomIn), 0, -10);
        }
        else
        {
            Time.timeScale = 1;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, startinCamSize, Time.deltaTime * zoomOut);
            cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, 0, Time.deltaTime * zoomOut), 0, -10);
            cam.transform.position = new Vector3(cam.transform.position.x, startCamPos.y, cam.transform.position.z);
        }
    }

    void activateJump(){
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
            cam.transform.position = new Vector3(cam.transform.position.x, startCamPos.y, cam.transform.position.z);
        }
        transform.position = pos;
    }
}
