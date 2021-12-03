using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EZCameraShake;
using UnityEngine.SceneManagement;

public class moveCont : MonoBehaviour
{

    /// </summary>
    /// this uses dani's code as a base and builds upon it , get the code here https://github.com/DaniDevy/FPS_Movement_Rigidbody
    /// 
    [Header("Assignables")]
    //Assingables
    public Transform playerCam, camContainer;
    public Transform bodyRoot;
    public Transform orientation;
    public plyStats stats;

    public Transform respawnPoint;
    public weaponCore wepCore;

    //Other
    private Rigidbody rb;

    [Header("Player stats")]
    public int HP, maxHP;



    //basics

    public enum moveState { none, planar, vault, slide };
    public moveState state;

    [Header("Functional Options")]
    public bool debug;
    public bool canHeadbob;
    public bool useRaw;

    [Header("Basics")]

    public Vector3 downVelocity;
    private float gConst = -9.8f;
    private float gravMulti = 100;
    private Vector3 lastGroundedpos;
    private Transform ply;
    public bool isMove;
    private GameObject groundCheck;
    public bool damagable;
    public bool enableSprint;


    //Rotation and look

    private float xRotation;
    public float sensitivity = 50f;
    private float sensMultiplier = 1f;
    public float baseFOV, zoomedFOV;


    [Header("movement stuff")]
    //Movement
    public float moveSpeed;
    public float speedBase, scaledSpeed;
    public float maxSpeed;
    public float speedModifier;
    public float speedCap;
    public float speedFloor;
    public bool grounded;
    public LayerMask whatIsGround;
    public bool wasSliding;
    private int wasSlideCount;
    public int wasSlideThres;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    [Header("Crouch & slide stuff")]
    //Crouch & Slide
    public float crouchScale;
    private Vector3 playerScale;
    public bool crouchBoy;

    [Header("Jump stuff")]
    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce;
    public int maxJumps;
    private int noOfJumps;
    public string lastInput;

    [Header("Movement Input")]
    //Input
    public float xRaw, yRaw; //raw input
    public float xInt, yInt; // interpolated input
    public float x, y; //edited input
    public float xInputIntThreshold, yInputIntThreshold;
    private float xPrevious, yPrevious;
    public float xScaledInput, yScaledInput;

    [Header("Keybinding Input")]
    public bool jumping, crouching, tapSprint;

    [Header("sliding stuff")]
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector, slideOrientation;
    private float fakeMomentum;
    public float slideBaseSpeed, jumpSlideBaseSpeed, slideThres, slideEndSpeed;
    public float slopeSlideSped, slopeAngleThres;
    public float slideScale;
    public float slJumpUpForce, slJumpFwForce;


    [Header("Headbob Parameters")]
    public AnimationCurve headbobCurve;
    public float headbobStartThreshold;
    public float headbobYOffset;
    public float headBobWalkSpeed;
    public float headBobWalkAmount;
    public float headBobCrouchSpeed;
    public float headBobCrouchAmount;
    public float headBobSrpintSpeed;
    public float headBobSprintAmount;
    public float defaultYpos = 0;
    private float timer;






    public bool airGround;

    [Header("Damage Stuff")]
    public int slowStacksStep, slowStacksThres;
    public int slowStacks, maxSlowStacks;
    public bool slowdownAfterDamage;
    public float slowDamageModifier; //modifier
    public float maxSlowDamageModifier; //base slow value
    public float slowModifierStep, slowModifierMaxTime; //amount of time in slow
    public AnimationCurve slowModifierCurve; //curve
    public float grappleSprintSpeed;
    public int dashStep, dashTime;
    public bool disableCounterMovement;
    public int counterMoveStep, counterMoveThres;
    public float dodgeSpeed;
    public int refreshStep, refreshThres;
    public string previousInput;
    public bool freeLook;
    public float crouchTime;
    public float currentScale;

    [Header("Dodge stuff")]
    public bool brokeDodgeCooldown;
    public int brokeDodgeStep, brokeDodgeThres;

    [Header("Dash stuff")]
    public bool breakGrappleCooldown;
    public int breakGrappleStep, breakGrappleThres;

    [Header("Stamina Stuff")]
    public float currentStamina, maxStamina;
    public float staminaRecovValue;
    public AnimationCurve staminaCurve;
    public bool stamOnCooldown;
    public int stamCooldownCount, stamCooldownThres;
    public float staminaDodgeCost;
    public bool panicSprint;
    public float panicSprintMulti,panicSprintCost;


    void Awake()
    {
        currentStamina = maxStamina;
        maxHP = 100;
        HP = 100;
        rb = GetComponent<Rigidbody>();
        defaultYpos = playerCam.transform.localPosition.y;
    }

    void Start()
    {
        //baseFOV = playerCam.gameObject.GetComponent<Camera>().fieldOfView;
        ply = this.gameObject.transform;
        playerScale = transform.localScale;
        EnableCursor();

        //Why do this in code , uhhhhh because I can :3

        groundCheck = new GameObject("groundCheck");

        //camDolly.transform.parent = camContainer;
        //Camera.main.transform.parent = camDolly.transform;

        groundCheck.transform.parent = orientation;
        groundCheck.transform.localPosition = new Vector3(0, (ply.GetComponent<CapsuleCollider>().height / -2), 0);
        

    }

    private void FixedUpdate()
    {
        crouchingState();
        handleStamina();
        refreshInputs();
        disableCounter();
        handleMovePly();
        handleSlowStacks();
    }

    void handleStamina()
    {
        if (stamOnCooldown)
        {
            stamCooldownCount++;

            if (stamCooldownCount >= stamCooldownThres)
            {
                stamCooldownCount = 0;
                stamOnCooldown = false;
            }
        }
        else
        {
            if (!brokeDodgeCooldown)
            {
                staminaRecovValue = staminaCurve.Evaluate(currentStamina / maxStamina);
                currentStamina += staminaRecovValue;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
            else
            {
                brokeDodgeStep++;

                if (brokeDodgeStep >= brokeDodgeThres)
                {
                    brokeDodgeStep = 0;
                    brokeDodgeCooldown = false;
                    stamOnCooldown = true;
                }
            }

        }

        if(currentStamina == 0)
        {
            panicSprint = false;
        }

        if (breakGrappleCooldown)
        {
            breakGrappleStep++;
            if (breakGrappleStep >= breakGrappleThres)
            {
                breakGrappleStep = 0;
                breakGrappleCooldown = false;
            }
        }
    }
    void refreshInputs()
    {
        refreshStep++;
        
        if(refreshStep >= refreshThres)
        {
            refreshStep = 0;
            previousInput = "";
        }
    }

    void disableCounter()
    {
        if(disableCounterMovement)
        {
            counterMoveStep++;
        }

        if(counterMoveStep >= counterMoveThres)
        {
            counterMoveStep = 0;
            disableCounterMovement = false;
        }
    }

    void handleSlowStacks()
    {
        if (slowStacks > 0)
        {
            slowStacksStep++;
        }

        if (slowStacksStep >= slowStacksThres)
        {
            slowStacksStep = 0;
            slowStacks--;
            slowStacks = Mathf.Clamp(slowStacks, 0, maxSlowStacks);
        }

        float slowStacksTime = (float)slowStacks / (float)maxSlowStacks;

        slowDamageModifier = slowModifierCurve.Evaluate(slowStacksTime);
    }

    private void Update()
    {
      

        isMove = rb.velocity.magnitude > 0.07f ? true : false;


        //moveSpeed = speedCap - slowDamageModifier;
        RaycastHit placePoint;
        Physics.Raycast(playerCam.position, playerCam.forward, out placePoint, 1000, whatIsGround);
        MyInput();
        Look();
        showDebug();
        handleHeadbob();
        calculateAimCone();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    /// this uses dani's code as a base and builds upon it , get the code here https://github.com/DaniDevy/FPS_Movement_Rigidbody
    /// 
    void showDebug()
    {
        if (debug)
        {
            if ((int)rb.velocity.magnitude != 0)
                Debug.Log(rb.velocity.magnitude + " / " + maxSpeed);
        }
    }

    public void takeDamage( int attackDamage)
    {
        if(damagable)
        {
            
            stats.plyHP -= attackDamage;
            slowdownAfterDamage = true;
            slowModifierStep = 0;
            slowStacks++;
            slowStacksStep = 0;
        }
        CameraShaker.Instance.ShakeOnce(15f, 2f, .1f, 0.5f);


    }

    public void fireShake()
    {
        CameraShaker.Instance.ShakeOnce(3f, 1f, .1f, 0.5f);
    }

    private void MyInput()
    {
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        xInt = Input.GetAxis("Horizontal");
        yInt = Input.GetAxis("Vertical");
        yInputIntThreshold = Mathf.Clamp(yInputIntThreshold,-1 , 1);
        xInputIntThreshold = Mathf.Clamp(xInputIntThreshold, -1, 1);
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();

        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        tapSprint = Input.GetKeyDown(KeyCode.LeftShift);

        //playerCam.transform.GetChild(0).gameObject.GetComponent<Camera>().fieldOfView = baseFOV + camZoomMod - camZoomedMod;
        //Crouching

        if (Input.GetKeyDown(KeyCode.Escape))
            // put the scene code here , scenecoood
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
            DisableCursor();
        if (Input.anyKeyDown)
            EnableCursor();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            breakGrapple();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            panicSprint = false ;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (previousInput == "A")
            {
                dodgeHorizontal();
            }
            previousInput = "A";
        }  
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (previousInput == "D")
            {
                dodgeHorizontal();
            }
            previousInput = "D";
        }

        if (Input.GetKeyDown(KeyCode.G))
            freeLook = true;
        if (Input.GetKeyUp(KeyCode.G))
            freeLook = false;

        if (!useRaw) 
        {
            xScaledInput = Mathf.Clamp(xInt / xInputIntThreshold, -1, 1);
            yScaledInput = Mathf.Clamp(yInt / yInputIntThreshold, -1, 1);

            switch (xRaw)
            {
                case 1:
                    x = xScaledInput;
                    break;
                case -1:
                    x = xScaledInput;
                    break;
                case 0:
                    x = 0;
                    break;
            }

            switch (yRaw)
            {
                case 1:
                    y = yScaledInput;
                    break;
                case -1:
                    y = yScaledInput;
                    break;
                case 0:
                    y = 0;
                    break;
            }

            if ((xPrevious != xRaw) && (xRaw != 0))
            {
                x = xRaw;
            }

            if ((yPrevious != yRaw) && (yRaw != 0))
            {
                y = yRaw;
            }

            xPrevious = xRaw;
            yPrevious = yRaw;
        }
        else
        {
            x = xRaw;
            y = yRaw;
        }

        /// switch between raw input and interpolated input
        /// scaling input ? (faster to max)
        /// raw / new scale = percentage 
        /// percentage max = 1 
        /// 
    }

    #region Cursor Functions
    void EnableCursor()
    {
        state = moveState.planar;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DisableCursor()
    {
        state = moveState.none;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
    void dodgeHorizontal()
    {
        float prevInput = -1;
        if (previousInput == "A")
            prevInput = -1;
        if (previousInput == "D")
            prevInput = 1;

        if (currentStamina != 0)
        {
            deductStamina(staminaDodgeCost);
            disableCounterMovement = true;
            rb.AddForce(orientation.transform.right * prevInput * dodgeSpeed);
        }
        else
        {
            if(!brokeDodgeCooldown)
            {
                rb.AddForce(orientation.transform.right * prevInput * dodgeSpeed / 2);
                disableCounterMovement = true;
                brokeDodgeCooldown = true;
            }
           
        }
    }

    void deductStamina(float cost)
    {
        stamOnCooldown = true;
        currentStamina -= cost;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
    void breakGrapple()
    {
        if(currentStamina > 0 && !breakGrappleCooldown)
        {
            deductStamina(70);
            if (Random.Range(0, maxSlowStacks) + 1 <= slowStacks)
            {
                wepCore.discardCurrentWeapon();
                //lose weapon / zombie shake
            }
            disableCounterMovement = true;
            slideOrientation = orientation.transform.forward;
            rb.AddForce(slideOrientation * grappleSprintSpeed);
            slowStacks = 0;
            breakGrappleCooldown = true;
            panicSprint = true;
        }
       
    }
    void handleHeadbob()
    {
        if(canHeadbob)
        {
            if (!grounded) return;

            if(rb.velocity.magnitude > headbobStartThreshold && !crouching)
            {
                 
                timer += Time.deltaTime;
                playerCam.transform.localPosition = new Vector3(
                    playerCam.transform.localPosition.x
                    , groundCheck.transform.position.y + ((crouching ? headBobCrouchAmount : headBobWalkAmount) * Mathf.Sin(timer * (crouching ? headBobCrouchSpeed : headBobWalkSpeed))) -headbobYOffset
                    , playerCam.transform.localPosition.z);
            }
            else
            {
                playerCam.transform.localPosition = Vector3.Lerp(playerCam.transform.localPosition , new Vector3(
                   playerCam.transform.localPosition.x
                   , groundCheck.transform.position.y + defaultYpos
                   , playerCam.transform.localPosition.z) , 0.0001f)
                    ;
            }
        }
    }
    void crouchingState()
    {
        if(crouching)
        {
           // currentScale = Mathf.Lerp(playerScale.y, crouchScale, crouchTime);
            transform.localScale = new Vector3(1, crouchScale , 1);
        }
    }
    void calculateAimCone()
    {
        

        float speedRatio = rb.velocity.magnitude / maxSpeed;
        float aimConeCurrent = Mathf.Clamp(speedRatio,0 ,1) * wepCore.moveAimConeAccuracy;
        float finalAimCone = aimConeCurrent + wepCore.baseAimConeAccuracy;
        wepCore.setAimCone(finalAimCone);

    }
    void handleMovePly()
    {
        switch (state)
        {
            case moveState.planar:
                Movement();
                break;
        }
    }

    private void StartCrouch()
    {


    }


    private void StopCrouch()
    {
        currentScale = playerScale.y;
        wasSliding = true;
        crouchBoy = false;
        state = moveState.planar;
        transform.localScale = playerScale;
    }

    private void handleSlide()
    {
        //Determine slope dir (cross product of orientation.right and normal of surface)
        RaycastHit slopeHit;
        Physics.Raycast(groundCheck.transform.position, -transform.up, out slopeHit);

        float slopeAngle = Vector3.Angle(orientation.transform.up, slopeHit.normal);
        Vector3 slopeDir = Vector3.Cross(slopeHit.normal, -orientation.transform.right);
        Debug.DrawRay(slopeHit.point, slopeDir, Color.green);



        rb.AddForce(Vector3.down * Time.deltaTime * 100);

        /// extra jank
        /// basically if the slope angle is more than the threshold allow a slide and if a forward raycast from feet detects nothing
        /// build speed
        if (slopeAngle >= slopeAngleThres && !(Physics.Raycast(groundCheck.transform.position, orientation.transform.forward)))
        {
            rb.AddForce(slopeDir * slopeSlideSped);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //slide jump
            wasSliding = true;
            state = moveState.planar;
            rb.AddForce(slideOrientation * slJumpFwForce);
            rb.AddForce(Vector2.up * slJumpUpForce);
        }

    }


    private void Movement()
    {
        if (wasSliding)
        {
            wasSlideCount++;
            if (wasSlideCount > wasSlideThres)
            {
                wasSliding = false;
            }
        }

        //Extra gravity
        if (!grounded)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 1000);

        }

        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;


        //Counteract sliding and sloppy movement
        if ((grounded) && !disableCounterMovement)
        {

            CounterMovement(x, y, mag);
        }


        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping)
        {
            Jump();

        }

        this.maxSpeed = speedCap + speedModifier;

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;



        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 1f;
            multiplierV = 1f;
        }

        /// the idea is to have a starting speed that is not 0
        /// so 0 is the idle speed
        /// but it does not go from 0 to speedFloor
        /// instead it goes to speedFloor immediately
        /// 

        if (rb.velocity.magnitude < speedFloor)
        {
            moveSpeed = scaledSpeed;
        }
        else
        {
            moveSpeed = speedBase;
        }

        // crouch movespeed
        if (grounded && crouching) multiplierV = 0.25f;
        multiplier -= slowDamageModifier;
        multiplier = Mathf.Clamp(multiplier, 0.01f, 100);
        if (panicSprint && currentStamina > 0)
        {
            deductStamina(panicSprintCost);
            multiplier = multiplier * panicSprintMulti;
            
        }
        if (y < 0)
        {
            multiplier = multiplier / 2f;
        }
        

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier );
    }

    private void Jump()
    {
        if (!wasSliding)
        {

           
            if (grounded)
            {
                noOfJumps = maxJumps;
            }

            if ((noOfJumps != 0) && readyToJump)
            {
                readyToJump = false;





                //Add jump forces
                rb.velocity += Vector3.up * jumpForce * 1.5f;

                //If jumping while falling, reset y velocity.
                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f)
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                else if (rb.velocity.y > 0)
                    rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
                noOfJumps--;

                //change states

                Invoke(nameof(ResetJump), jumpCooldown);


            }
        }
        else
        {
            wasSliding = true;
            state = moveState.planar;
            rb.AddForce(slideOrientation * slJumpFwForce);
            rb.AddForce(Vector2.up * slJumpUpForce);
        }





    }

    private void ResetJump()
    {
        readyToJump = true;

        //change states
        state = moveState.planar;

    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        
        if (!freeLook)
        {
            bodyRoot.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }
       
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;
        if (rb.velocity.magnitude >= 35) return;

        //Slow down sliding


        //Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
      //  int layer = other.gameObject.layer;
      //  if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

 



}

