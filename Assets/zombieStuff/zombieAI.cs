using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    [Header("Assignables")]
    public LineRenderer lr;
    public GameObject player ,eyes ,root , ragdoll ,head , body;
    public NavMeshAgent zombieAgent;
    public Vector3 overrideTarget;
    public LayerMask levelGeo;

    [Header("Status")]
    public bool isActive;
    public bool inRange, showPath;
    public bool stunned;
    public float currentSpeed;
    public float speedTopBracket;
    public bool isMove;
    public bool startBreaking;
    public bool didAttack;
    public int bleedStacks;
    public bool damageBreak;
    public float speedinUse;
    public bool showMesh;
    public bool canCollide;
    public bool hasFallen;

    public int shoveAmount, shoveThres;
    public float Hp, MaxHp;

    public float radiusFromTarget;

    public enum state { wander, chase, die };
    public state zombieType;

    [Header("Behaviour")]
    public AnimationCurve accelerationCurve;
    public int timeToMaxAcceleration, moveTimeElapsed;
    public float timeInTopSpeed, maxTimeInTopSpeed;
    public float distanceToPlayer;
    public float distanceToSlowdown;
    public float inRangeSlowdownSpeed;
    public float maxAcceleration;
    public float accelerationFloor = 3;
    public float baseSpeed;
    public float normalRunningTurningSpeed;
    public float inRangeOfPlayerTurningSpeed;
    public Vector3 storeVelocity;
    public int damageCeiling;
    public float bleedDamage;
    public int bleedStep, bleedTime;
    public int damageStunStep, damageStunTime;

    public AnimationCurve healthSlowdownCurve;
    public float healthSlowMultiplier;

    public float rotationSpeed;
    public float FOV, FOVradius, playerDetectionRadius;
    public int attackStep, attackWindUp;
    public float attackRange;
    public int damage;
    public float alertRadius;
    public int stunStep, stunDur;
    public int fallenStep, fallenTime;
    public float getUpSpeed;

    public float sleepyAlertRadius;
    public float sleepyAlertInSightRadius;

    public GameObject dummy;

    






    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Hp = MaxHp;
        zombieAgent = this.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        speedinUse = zombieAgent.speed;
        stunned = false;
        zombieAgent.updateUpAxis = false;
        zombieAgent.stoppingDistance = 1.25f;
        visualisePath();
    }

    // Update is called once per frame
    void Update()
    {
      
        distanceToPlayer = (this.transform.position - player.transform.position).magnitude;

      
        currentSpeed = zombieAgent.velocity.magnitude;

        isMove = currentSpeed == 0 ? false : true;

        if(!stunned)
        {
            if (zombieType == state.chase)
            {
                zombieMovementinChase();
                zombieAgent.destination = player.transform.position;
            }

            if (distanceToPlayer < sleepyAlertRadius)
            {
                startChase();
            }

            healthSlowdown();
        }

        if(shoveAmount >= shoveThres)
        {
            hasFallen = true;
            startFall();
        }
   
            zombieAgent.isStopped = stunned ;
        zombieAgent.isStopped = hasFallen;
        
    
        viewMesh();
        
        
        Debug.DrawRay(this.transform.position, this.transform.forward);
        //Debug.Log(zombieAgent.velocity.magnitude);
        checkBleedStacks();
        visualisePath();
        checkFOV();
        CheckForAttack();
        CheckForStun();
        inFall();
    }

    private void viewMesh()
    {
        body.GetComponent<MeshRenderer>().enabled = showMesh;
        head.GetComponent<MeshRenderer>().enabled = showMesh;

        body.GetComponent<CapsuleCollider>().enabled = canCollide;
        head.GetComponent<SphereCollider>().enabled = canCollide;
    }

    private void zombieMovementinChase()
    {
        if (didAttack)
        {
            zombieAgent.velocity = storeVelocity;
            storeVelocity = Vector3.zero;
            didAttack = false;

        }

        if ((this.transform.position - player.transform.position).magnitude < alertRadius)
        {
            isActive = true;
        }

        float topTierSpeedThres = ((Mathf.Clamp(speedTopBracket, 0, 100)) / 100) * speedinUse;
        if (currentSpeed > topTierSpeedThres)
        {
            attackStep = attackWindUp;
            timeInTopSpeed += Time.deltaTime;
            timeInTopSpeed = Mathf.Clamp(timeInTopSpeed, 0, maxTimeInTopSpeed);

            if ((distanceToPlayer < distanceToSlowdown) && (timeInTopSpeed == maxTimeInTopSpeed))
            {
                startBreaking = true;
            }
            root.transform.localRotation = Quaternion.Euler(22, 0, 0);

        }
        else
        {
            root.transform.localRotation = Quaternion.Euler(0, 0, 0);
            timeInTopSpeed = 0;
        }

        if (startBreaking)
        {
            if (currentSpeed > inRangeSlowdownSpeed)
            {
                speedinUse = inRangeSlowdownSpeed;
            }
            else
            {
                startBreaking = false;
                speedinUse = baseSpeed;
            }
        }

        if (isMove)
        {
            moveTimeElapsed++;
            moveTimeElapsed = Mathf.Clamp(moveTimeElapsed, 0, timeToMaxAcceleration);
        }
        else
        {
            moveTimeElapsed = 0;
        }

        if (distanceToPlayer < distanceToSlowdown)
        {
            zombieAgent.angularSpeed = inRangeOfPlayerTurningSpeed;
        }
        else
        {
            zombieAgent.angularSpeed = normalRunningTurningSpeed;
        }
        zombieAgent.acceleration = Mathf.Clamp((accelerationCurve.Evaluate(moveTimeElapsed / timeToMaxAcceleration)) * maxAcceleration, accelerationFloor, maxAcceleration);

    }

    private void healthSlowdown()
    {
        float temp = Hp / MaxHp;
        healthSlowMultiplier = healthSlowdownCurve.Evaluate(-temp + 1);
        zombieAgent.speed = speedinUse * healthSlowMultiplier;
    }

    private void startChase()
    {
        zombieType = state.chase;
    }

    private void LateUpdate()
    {
        if (Hp == 0)
        {
            GameObject doll = Instantiate(ragdoll, this.transform.position, Quaternion.identity);
            doll.GetComponent<Rigidbody>().velocity = zombieAgent.velocity;
            doll.transform.rotation = zombieAgent.transform.rotation;
          //  doll.transform.localRotation = Quaternion.Euler(new Vector3(zombieAgent.transform.rotation.x + 15, zombieAgent.transform.rotation.y , zombieAgent.transform.rotation.z));
            Destroy(this.gameObject);
        }
    }

    void checkFOV()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
        Vector3 targetDirFromEyes = player.transform.position - eyes.transform.position;
        RaycastHit eyeHit;

       

        if (targetDirFromEyes.magnitude < FOVradius)
        {
            Debug.DrawRay(eyes.transform.position, targetDirFromEyes, Color.cyan);
            if(Physics.Raycast(eyes.transform.position, targetDirFromEyes, out eyeHit , 100f))
            {
                if (eyeHit.collider.gameObject.GetComponent<moveCont>() && eyeHit.collider.gameObject)
                {
                    if (targetDir.magnitude < FOVradius)
                    {
                        Vector3 dir = player.transform.position - transform.position;
                        dir.y = 0;//This allows the object to only rotate on its y axis
                        Quaternion rot = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

                        if (angleToPlayer >= -FOV / 2 && angleToPlayer <= FOV / 2)
                        {
                            startChase();
                        }
                    


                    }
                }
            }

           

        }



       
    }

    void checkBleedStacks()
    {
        if(bleedStacks > 0)
        {
            bleedStep++;

                if(bleedStep == bleedTime)
                {
                bleedStep = 0;
                    Hp -= bleedDamage * bleedStacks;
                }
        }
    }

    void visualisePath()
    {
        if(showPath)
        {
            lr.positionCount = zombieAgent.path.corners.Length;
            lr.SetPositions(zombieAgent.path.corners);
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }

    #region old stuff
    void CheckForStun()
    {
        if(stunned)
        {
            stunStep++;
            stunStep = Mathf.Clamp(stunStep, 0, stunDur);

            if(stunStep == stunDur)
            {
                stunned = false;
                stunStep = 0;
            }
        }
    }
    public void takeDamage( int damage , float multiplier)
    {
        multiplier = multiplier > 0 ? multiplier : 1;

        int calculatedDamage = (int)(damage * multiplier);
        calculatedDamage = Mathf.Clamp(calculatedDamage, 0, damageCeiling);
        Hp -= calculatedDamage;
        Hp = Mathf.Clamp(Hp, 0, MaxHp);
        bleedStacks++;
        storeVelocity = zombieAgent.velocity;

        float topTierSpeedThres = ((Mathf.Clamp(speedTopBracket, 0, 100)) / 100) * speedinUse;
        if (zombieAgent.velocity.magnitude > topTierSpeedThres && Hp < 60)
        {
            zombieAgent.velocity = (-storeVelocity * 0.7f) + storeVelocity;
        }
      
        


    }

    public void doStun(float bashForce)
    {
        stunStep = 0;
        stunned = true;

        storeVelocity = zombieAgent.velocity;
        Vector3 storedVelocityDir = zombieAgent.velocity.normalized + zombieAgent.transform.position;
        int fallChance = Random.Range(0, 5);
        if((fallChance <= 2) || shoveAmount > shoveThres)
        {
           
        }
        else
        {
            //shoveAmount++;
            zombieAgent.velocity = (-zombieAgent.transform.forward * bashForce);
        }

    }

    void CheckForAttack()
    {
        inRange = false;
        if (((this.transform.position - player.transform.position).magnitude < attackRange) && !stunned)
        {
            inRange = true;
        }

        if(inRange)
        {
            attackStep++;
            attackStep = Mathf.Clamp(attackStep, 0, attackWindUp);
         
        }

        if (attackStep == attackWindUp && inRange)
        {
            attackStep = 0;
            doAttack();
        }
    }

    void startFall()
    {
        if(hasFallen)
        {
            if(dummy == null)
            {
                canCollide = false;
                showMesh = false;
                dummy = Instantiate(ragdoll, this.transform.position, Quaternion.identity);
                dummy.GetComponent<Rigidbody>().velocity = zombieAgent.velocity;
                dummy.transform.rotation = zombieAgent.transform.rotation;
            }
           
        }
    }

    void inFall()
    {
        if(hasFallen)
        {
            fallenStep++;
            if(fallenStep >= fallenTime)
            {
                dummy.transform.position = Vector3.Lerp(dummy.transform.position, this.transform.position, getUpSpeed);
                dummy.transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(dummy.transform.localRotation.y , this.transform.localRotation.y , getUpSpeed), 0));
;                
            }
        }
    }

    private void doAttack()
    {
        zombieAgent.velocity = storeVelocity;
        zombieAgent.velocity = Vector3.zero;
        player.GetComponent<moveCont>().takeDamage(damage);
        didAttack = true;
        
    }

  

    #endregion


}
