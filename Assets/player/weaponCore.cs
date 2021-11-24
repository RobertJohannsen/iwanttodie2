using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCore : MonoBehaviour
{
    [Header("assignables")]
    public weaponLibrary weaponLib;
    public gunAnimationController gunAnimationCont;
    public GameObject weaponBarrel ,bulletHit ,throwPosition ,dropPosition;
    public GameObject bulletWound;
    public moveCont moveCore;
    public GameObject muzzleFlash;
    public plyInventory plyInv;
    public GameObject handsEmpty;
    public LayerMask shootLayerMask;
    public enum equipType {firearm , melee , singleConsumable , consumable ,throwable}
    public equipType weaponType;
    public plyStats stats;

    [Header("status")]
    public int currentWeaponID;
    public int weaponAmmoID;
    public bool triggerDown , shootReady;
    public bool startBash;
    public bool hasShot;
    public bool doWeaponSway;
    public bool doWeaponBob;
    public bool canFullAuto;
    public bool reloading;
    public bool hasPumped;
    public bool startThrow;
    public bool inMelee;

    [Header("assignable behaviour")]
    public int shotCount;
    public float baseAimConeAccuracy, moveAimConeAccuracy ;
    public int ejectFrames, totalCycleTime;
    public float maxShotDistance;
    public float interactDistance;
    public int magCapacity , ammoPool;
    public int bashDuration;
    public int meleeDuration;
    public int weaponDamage;
    public float bashDistance , bashForce;
    public float meleeDistance , meleeForce;
    public enum ReloadType {mag , single};
    public ReloadType reloadType;
    public int reloadTime, reloadInsertTime;
    public float swayAmount, swaySmoothing ,swayClamp;
    public float bobSmoothing, bobXAmp , bobYAmp , bobFreq;
    public Vector3 bobOffset;
    public bool obtusePump;
    public float weaponThrowForce , weaponSpinForce;
    

    [Header("Recoil")]
    public float recoilAmount;
    public float moveDevTime;
    public float moveDevReturnTime;

    [Header("Ejection")]
    public GameObject casing;
    public GameObject ejectionPort;
    public float ejectionForce;
    public float ejectionSpin;

    [Header("View Behaviour")]
    public float finalAimCone, currentAimCone;
    public int elapseCycleTime;
    public int currentAmmo;
    public int bashStep;
    public int meleeStep;
    public RaycastHit interactHit;

    public enum reState { no, eject, insert }
    public reState gunState;
    public int reloadStep;
    private Vector3 forwardVector;

    
    public enum useType { zoomies, hp, tenk, fire, attract, explode }
    public useType itemType;

    public int healAmount;
    public float speedBuff;
    public int timeToConsumue;
    public int tenkStep, speedStep;
    public int tenkDuration, speedDuration;
    public bool isZoomies;

    [Header("Grab stuff")]
    public bool isGrabbing;
    public bool dropObj;
    public float grabPosDistMulti;
    public Vector3 grabPos;
    public GameObject grabObj;
    public float grabProffiency;
    public float throwForce;

    private float currentHitDistance;

    [Header("bash stuff")]
    public int  bashScore , bashThres , bashMax;
    public int reduceBashCount , reduceBashThres;
    public int bashCost;
    public bool bashOnCooldown;
    public int bashCooldownCount, bashCooldownThres;
    //bashscore is the value added when you bash , bashThres , if the bashscore is over bashthres then allow bash but enter bash cooldown slow
    ///bashscore will gradually reduce overtime , if the bashscore is over the thres then bash cooldown is 


    



    /// <summary>
    ///  no -> eject -> insert -> no
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        startBash = false;
        currentAmmo = magCapacity;
    }

    // Update is called once per frame
    void Update()
    {

            triggerDown = Input.GetKeyDown(KeyCode.Mouse0);

        if (canFullAuto) triggerDown = Input.GetKey(KeyCode.Mouse0);

        if (weaponType == equipType.consumable) triggerDown = Input.GetKey(KeyCode.Mouse0);


        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            handleLeftClickUp();
        }




        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            bash();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(weaponType == equipType.firearm)
            {
                reload();
            }
            
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            fireInteract();
        }


        handleWeaponType();


        Debug.DrawRay(plyInv.fakeBarrel.transform.position, plyInv.fakeBarrel.transform.forward , Color.red);

            if (isGrabbing)
        {
            if(Input.GetKey(KeyCode.E))
            {
                 grabPos = Camera.main.transform.position + Camera.main.transform.forward.normalized * grabPosDistMulti;

                grabObj.GetComponent<Rigidbody>().velocity = (grabPos - grabObj.transform.position) * grabProffiency;
            }

            if (Input.GetKeyDown(KeyCode.F)) // throw object
            {
               
                    grabObj.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce);

                    grabObj = null;
                    isGrabbing = false;
                
                
                   
                

               
                
            }
            if (Input.GetKeyUp(KeyCode.E)) // drop object
            {
                grabObj = null;
                isGrabbing = false;
            }

        }

        if (!isGrabbing)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                startThrow = true;
                gunAnimationCont.callThrowStartAnimation();
            }

            if (startThrow)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startThrow = false;
                    gunAnimationCont.callEquipAnimation();
                }
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                if (startThrow)
                {
                    throwCurrentWeapon();
                }

            }
        }



        
      


        if (plyInv.InventoryReferenceSlot[plyInv.selectedSlot].currentWeaponID == 0) return;

        plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponStats>().ammoPool = ammoPool;
        plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponStats>().currentAmmo = currentAmmo;

        if(inMelee) Debug.DrawRay(weaponBarrel.transform.position, weaponBarrel.transform.forward, Color.red);





    }

    public void FixedUpdate()
    {
        reduceBash();
        handleRecoil();
        doTenk();
        doZoomies();
        gunBash();
        handleReloadType();
    }

    void reduceBash()
    {
        if (!bashOnCooldown)
        {
            if (bashScore > 0)
            {
                reduceBashCount++;

                if (reduceBashCount >= reduceBashThres)
                {
                    reduceBashCount = 0;
                    bashScore--;
                    bashScore = Mathf.Clamp(bashScore, 0, bashMax);
                }
            }
        }
        else
        {
            bashCooldownCount++;

            if (bashCooldownCount >= bashCooldownThres)
            {
                bashCooldownCount = 0;
                bashOnCooldown = false;
            }
        }
    }
    public void handleLeftClickUp()
    {
        switch (weaponType)
        {
            case equipType.singleConsumable:
                    useConsumable();
                break;
            case equipType.consumable:
                break;
            case equipType.throwable:

                throwCurrentWeapon();
                break;

        }
    }
    public void handleWeaponType()
    {
        switch (weaponType)
        {
            case equipType.firearm:
                if (gunState == reState.no)
                {
                    tryFireWeapon();
                    cycleCycle();
                }
                break;

            case equipType.melee:
                doMelee();
                if(Input.GetKeyDown(KeyCode.Mouse0)) 
                {
                    gunAnimationCont.callShootAnimation();
                    tryMeleeAttack();
                }
              
                break;
            case equipType.consumable:
                if(triggerDown)
                {
                    gunAnimationCont.callShootAnimation();
                }
                else
                {
                    gunAnimationCont.weaponAnimator.StopPlayback();
                }
                break;
            case equipType.throwable:
                if(triggerDown)
                {
                    gunAnimationCont.callThrowStartAnimation();
                }
                break;
                
        }
       
    }

    public void useConsumable()
    {
        if(itemType == useType.hp) healPly(healAmount);
        if (itemType == useType.tenk) becomeTenk();
        if(itemType == useType.zoomies) becomeSpeed();
    }

    public void healPly(int healAmount)
    {
        stats.plyHP += healAmount;
    }

    public void becomeTenk()
    {
        stats.isTenk = true;
        stats.lockedHP = stats.plyHP;
        tenkStep = 0;

    }

    public void becomeSpeed()
    {
        isZoomies = true;
        moveCore.speedModifier = speedBuff;
        speedStep = 0;
    }

    public void doTenk()
    {
        if(stats.isTenk)
        {
            tenkStep++;

            if(tenkStep >= tenkDuration)
            {
                stats.isTenk = false;

            }
        }
    }

    public void doZoomies() 
    {
        if(isZoomies)
        {
            speedStep++;
            if(speedStep >= speedDuration)
            {
                isZoomies = false;
                moveCore.speedModifier = 0;
            }
        }
    }
 
    public void tryMeleeAttack()
    {
        inMelee = true;
    }

    public void doMelee()
    {
        if (inMelee)
        {
            meleeStep++;
            meleeStep = Mathf.Clamp(meleeStep, 0, meleeDuration) ;

            if (meleeStep == meleeDuration)
            {
                meleeStep = 0;
                inMelee = false;
            }
        }
    }
    public void pickupThing()
    {
        interactHit.collider.gameObject.GetComponentInParent<Animator>().enabled = true;
        plyInv.InventoryReferenceSlot[plyInv.selectedSlot] = interactHit.collider.gameObject.GetComponentInParent<weaponStats>();
        if(interactHit.collider.gameObject)
        {
            interactHit.collider.gameObject.transform.parent.position = new Vector3(9999999, 999999, 99999);
        }
       

        plyInv.weaponRoot.transform.GetChild(1).GetComponent<Animator>().enabled = true;
        plyInv.scrollUpdateDetected();
    }
    public void bash()
    {
        if(!bashOnCooldown)
        {
            bashScore += bashCost;
            bashScore = Mathf.Clamp(bashScore, 0, bashMax);
            gunAnimationCont.callBashAnimation();
            startBash = true;
        }
        if (bashScore > bashThres)
        {
            bashOnCooldown = true;
        }
    }

    #region core
    public void throwCurrentWeapon()
    {
        if (plyInv.InventoryReferenceSlot[plyInv.selectedSlot].currentWeaponID == 0) return;

        var throwable = Instantiate(plyInv.weaponRoot.transform.GetChild(1).gameObject , throwPosition.transform.position , Quaternion.identity);
        throwable.GetComponent<Animator>().enabled = false;
        throwable.GetComponent<BoxCollider>().enabled = true;
        throwable.GetComponent<Rigidbody>().AddForce(weaponThrowForce * Camera.main.transform.forward);
        throwable.gameObject.transform.GetComponentInChildren<weaponColliderBehaviour>().weaponThrown();
        Debug.Log("did thing");

        if(weaponType == equipType.throwable)
        {
            if (throwable.GetComponent<throwableBehaviour>()) throwable.GetComponent<throwableBehaviour>().throwableEnabled = true;
        }

        throwable.GetComponent<Rigidbody>().angularVelocity = new Vector3(weaponSpinForce, weaponSpinForce, throwable.GetComponent<Rigidbody>().angularVelocity.z);
        removeWeaponReference();
        plyInv.scrollUpdateDetected();
    }

    public void discardCurrentWeapon()
    {
        if (plyInv.InventoryReferenceSlot[plyInv.selectedSlot].currentWeaponID == 0) return;
        
        var throwable = Instantiate(plyInv.weaponRoot.transform.GetChild(1).gameObject, dropPosition.transform.position, Quaternion.identity);
        throwable.GetComponent<Animator>().enabled = false;
        throwable.GetComponent<BoxCollider>().enabled = true;

        removeWeaponReference();
        plyInv.scrollUpdateDetected();
    }
   

    public void removeWeaponReference()
    {
        //first get reference to world script
        /// assign world value to world script
        /// delete world reference;
        /// delete old weapon;
        var weaponRef = plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponStats>();

        for (int f = 0; f < plyInv.weaponAmmoIDList.Count; f++)
        {
            if (plyInv.InventoryReferenceSlot[plyInv.selectedSlot].weaponAmmoID == plyInv.weaponAmmoIDList[f])
            {
                weaponRef.currentAmmo = plyInv.currentAmmoList[f];
                weaponRef.ammoPool = plyInv.ammopoolList[f];
                //world assigned to world;
                plyInv.weaponAmmoIDList.RemoveAt(f);
                plyInv.currentAmmoList.RemoveAt(f);
                plyInv.ammopoolList.RemoveAt(f);
                //delete world reference;
                Destroy(plyInv.weaponRoot.transform.GetChild(1).gameObject);
                plyInv.InventoryReferenceSlot[plyInv.selectedSlot] = handsEmpty.GetComponent<weaponStats>();
                //delete old weapon
            }
        }

    }


    public void fireInteract()
    {
        

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out interactHit, interactDistance))
        {
            Debug.Log(interactHit.collider.gameObject);
            if (interactHit.collider.gameObject.GetComponent<doorBehaviour>()) interactHit.collider.gameObject.GetComponent<doorBehaviour>().openDoor();

            if (interactHit.collider.gameObject.GetComponent<canGrab>())
            {
                if(interactHit.collider.gameObject.GetComponent<Rigidbody>())
                {
                    grabObj = interactHit.collider.gameObject;
                    isGrabbing = true;
                    dropObj = false;
                }
            }

            if(interactHit.collider.gameObject.GetComponentInParent<canPickup>())
            {
                pickupThing();
            }
        }
    }

    public void handleReloadType()
    {
        switch (reloadType)
        {
            case ReloadType.mag:
                switch (gunState)
                {
                    case reState.eject:
                        reloadStep++;
                        if (reloadStep >= reloadInsertTime)
                        {
                            gunState = reState.insert;
                            actuallyReload();
                        }

                        break;
                    case reState.insert:
                        reloadStep++;
                        if (reloadStep == reloadTime)
                        {
                            reloadStep = 0;
                            gunAnimationCont.callReloadEndAnimation();
                            gunState = reState.no;
                            reloading = false;
                        }
                        break;
                }
                break;
            case ReloadType.single:
                if (reloading)
                {
                    if(currentAmmo != magCapacity)
                    {
                        if(reloadStep == 0)
                        {
                            
                            gunAnimationCont.callReloadRackAnimation();
                        }

                        reloadStep++;
                        if(reloadStep == reloadInsertTime)
                        {
                            //putting shell in
                            actuallyReloadSingle();
                            hasPumped = false;
                        }

                        if (reloadStep == reloadInsertTime)
                        {
                            reloadStep = 0;
                        }

                       
                    }

                    if (currentAmmo == magCapacity || ammoPool <= 0)
                    {
                        gunAnimationCont.callReloadEndAnimation();
                        reloading = false;
                    }
                }
                break;
        }
       
    }

    public void setAimCone(float deviation)
    {
        currentAimCone = deviation;
    }

    public float getGunAccuracyNow(float baseAcc , float maxAcc)
    {
        finalAimCone = Mathf.Clamp(finalAimCone, 0, moveAimConeAccuracy);
        return ((finalAimCone/moveAimConeAccuracy)*(maxAcc - baseAcc));
    }

    public void handleRecoil()
    {
       if(moveCore.isMove)
        {
            finalAimCone = Mathf.Lerp(finalAimCone, moveAimConeAccuracy, moveDevTime);
        }
       else
        {
            finalAimCone = Mathf.Lerp(finalAimCone , baseAimConeAccuracy ,moveDevReturnTime);
        }
    }

    #endregion

    #region firearm 
    public void gunBash()
    {
        if(startBash)
        {
            if(gunState == reState.insert)
            {
              
                    reloadStep = 0;
                    gunAnimationCont.callReloadEndAnimation();
                    gunState = reState.no;
                
            }
            bashStep++;
            bashStep = Mathf.Clamp(bashStep, 0, bashDuration);
            RaycastHit bashHit;
          
            if(bashStep == bashDuration)
            {
                bashStep = 0;
                startBash = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(plyInv.fakeBarrel.transform.position + plyInv.fakeBarrel.transform.forward, bashDistance);
    }

    public void tryFireWeapon()
    {
        if (plyInv.InventoryReferenceSlot[plyInv.selectedSlot].currentWeaponID == 0) return;

        if (triggerDown && shootReady)
        {
            if (currentAmmo > 0)
            {
                if (gunState == reState.no)
                {
                    if(obtusePump)
                    {
                        if(hasPumped)
                        {
                            fireWeapon();
                            shootReady = false;
                            hasPumped = false;
                        }
                        else
                        {
                            //pump here 
                            gunAnimationCont.callReloadEndAnimation();
                            hasPumped = true;
                        }
                    }
                    else
                    {

                        fireWeapon();
                        //fire actual bullet
                        shootReady = false;
                        //do reload here
                    }


                }

            }

        }
    }
    public void fireWeapon()
    {
        moveCore.fireShake();
        reloading = false;
        currentAmmo--;
        plyInv.callFindAmmoReference();

         gunAnimationCont.callShootAnimation();
         var flash = Instantiate(muzzleFlash, plyInv.fakeBarrel.transform.position, Quaternion.identity);
         flash.transform.parent = plyInv.fakeBarrel.transform;
        finalAimCone += recoilAmount;

        for(int x = 0; x < shotCount ; x++)
        {
            forwardVector = Vector3.forward;
            float deviation = Random.Range(0f, finalAimCone);
            float angle = Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = Camera.main.transform.rotation * forwardVector;



            //Calculate Direction with Spread
            //Vector3 shootAfterSpreadDirection = weaponBarrel.transform.forward + new Vector3(-x, -y, -z);

            RaycastHit weaponHit;
            if (Physics.Raycast(Camera.main.transform.position, forwardVector, out weaponHit, maxShotDistance , shootLayerMask ,QueryTriggerInteraction.Ignore))
            {
               
                switch (weaponHit.collider.gameObject.tag)
                {
                    case "zombieHead":
                        spawnBulletWound(weaponHit);
                        weaponHit.collider.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(weaponDamage, 100);
                        break;
                    case "zombieBody":
                        spawnBulletWound(weaponHit);
                        weaponHit.collider.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(weaponDamage, 1);
                        break;
                }

                GameObject hitPoint = Instantiate(bulletHit, weaponHit.point, Quaternion.identity);
                hitPoint.transform.SetParent(weaponHit.transform);

            }
        }
       

        
      
        

    

    }

    public void spawnBulletWound(RaycastHit hitpoint)
    {
        GameObject bulletWound = Instantiate(this.bulletWound, hitpoint.point, Quaternion.identity);
        bulletWound.transform.SetParent(hitpoint.transform);
    }

    public void reload()
    {
        if (plyInv.InventoryReferenceSlot[plyInv.selectedSlot].currentWeaponID == 0) return;
        switch(reloadType)
        {
            case ReloadType.mag:
                testReloadMag();
                break;
            case ReloadType.single:
                testReloadSingle();
                break;
        }
    }

    public void testReloadSingle()
    {
        if(ammoPool > 0)
        {
            gunAnimationCont.callThrowStartAnimation();
            reloading = true;
        }
    }

    public void testReloadMag()
    {
        if (ammoPool - magCapacity > 0)
        {
            reloading = true;
            gunAnimationCont.callReloadAnimation();
            gunState = reState.eject;
        }
        else
        {
            if (ammoPool != 0)
            {
                currentAmmo = ammoPool;
                gunAnimationCont.callReloadAnimation();
                gunState = reState.eject;
                ammoPool = 0;
                plyInv.callFindAmmoReference();
            }
        }
    }

    public void actuallyReloadSingle()
    {
        currentAmmo++;
        ammoPool--;
        ammoPool = Mathf.Clamp(ammoPool, 0, 1000);
        plyInv.callFindAmmoReference();
    }

    public void actuallyReload()
    {
        currentAmmo = magCapacity;
        ammoPool -= magCapacity;
        ammoPool = Mathf.Clamp(ammoPool, 0, 1000);
        plyInv.callFindAmmoReference();
    }

   

    private void cycleCycle()
    {
        if (!shootReady)
        {
            elapseCycleTime++;
            elapseCycleTime = Mathf.Clamp(elapseCycleTime , 0 , totalCycleTime);

            if (elapseCycleTime == ejectFrames)
            {
                ejectCasing();
            }
                            //spawn bullet casing
            if (elapseCycleTime == totalCycleTime)
            {
                elapseCycleTime = 0;
                shootReady = true;
            }
        }
    }

    private void ejectCasing()
    {
        GameObject newCasing = Instantiate(casing, plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponStats>().ejectionPort.transform.localPosition, Quaternion.identity);
        newCasing.transform.position = plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponStats>().ejectionPort.transform.position;
        newCasing.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(ejectionForce * plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponStats>().ejectionPort.transform.forward);
        newCasing.transform.GetChild(0).GetComponent<Rigidbody>().angularVelocity = new Vector3(ejectionSpin , newCasing.transform.GetChild(0).GetComponent<Rigidbody>().angularVelocity.y , newCasing.transform.GetChild(0).GetComponent<Rigidbody>().angularVelocity.z);

        //Debug.Log("ejecting casing");
    }

    #endregion





}
