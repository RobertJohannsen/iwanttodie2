using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyInventory : MonoBehaviour
{
    public weaponCore core;
    public GameObject weaponRoot;
    public int selectedSlot;
    public int slotCount;
    public List<weaponStats> InventoryReferenceSlot;
    public List<int> weaponAmmoIDList;
    public List<int> currentAmmoList;
    public List<int> ammopoolList;
    public GameObject fakeBarrel;
    public bool updateScroll;

    // Start is called before the first frame update
    void Start()
    {
        scrollUpdateDetected();
    }

    // Update is called once per frame
    void Update()
    {
        updateScroll = false;
        if (Input.mouseScrollDelta.y > 0) 
        { 
            selectedSlot++;
            updateScroll = true;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            
            selectedSlot--;
            updateScroll = true;
        }


        if(selectedSlot < 0) selectedSlot = slotCount;
        if(selectedSlot > slotCount) selectedSlot = 0;

        if(updateScroll)
        {
            scrollUpdateDetected();
        }

     
        
        
    }


    private void LateUpdate()
    {
        if (InventoryReferenceSlot.Count != 0)
        {
            fakeBarrel.transform.localPosition = InventoryReferenceSlot[selectedSlot].weaponBarrel.transform.localPosition;
        }
       
    }

    public void scrollUpdateDetected()
    {
        
        assignWorldWeaponAmmoID();
        setupWeapon();
    }


    /// <summary>
    /// weapon data structure 
    /// on weaponStats is the reference data container for all weapons
    /// it contains
    /// the mesh
    /// the stats
    /// the animator ?
    /// all these will then need to be assigned to the specific stats
    /// </summary>
    void setupWeapon()
    {
        if (InventoryReferenceSlot.Count == 0)
        {
            return;
        }

       
        
        if (weaponRoot.transform.childCount != 0 && weaponRoot.transform.childCount > 1)
        {
            Destroy(weaponRoot.transform.GetChild(1).gameObject);
        }

        GameObject mesh = InventoryReferenceSlot[selectedSlot].weaponMesh;
        GameObject tempMesh = Instantiate(mesh, weaponRoot.transform.position, Quaternion.identity, weaponRoot.transform);

        tempMesh.transform.localPosition = Vector3.zero;
        tempMesh.transform.localRotation = Quaternion.Euler(0, 0, 0);

        AssignWeapon();
        AssignWeaponAnimations();
    }
    void AssignWeapon()
    {
        
        core.weaponBarrel = InventoryReferenceSlot[selectedSlot].weaponBarrel;
        core.muzzleFlash = InventoryReferenceSlot[selectedSlot].muzzleFlash;
        core.currentWeaponID = InventoryReferenceSlot[selectedSlot].currentWeaponID;
        
        core.canFullAuto = InventoryReferenceSlot[selectedSlot].canFullAuto;



        core.shotCount = InventoryReferenceSlot[selectedSlot].shotCount;
        core.baseAimConeAccuracy = InventoryReferenceSlot[selectedSlot].baseAimConeAccuracy;
        core.moveAimConeAccuracy = InventoryReferenceSlot[selectedSlot].moveAimConeAccuracy;
        core.ejectFrames = InventoryReferenceSlot[selectedSlot].ejectFrames;
        core.totalCycleTime = InventoryReferenceSlot[selectedSlot].totalCycleTime;
        core.maxShotDistance = InventoryReferenceSlot[selectedSlot].maxShotDistance;

        core.magCapacity = InventoryReferenceSlot[selectedSlot].magCapacity;
        core.bashDuration = InventoryReferenceSlot[selectedSlot].bashDuration;
        core.weaponDamage = InventoryReferenceSlot[selectedSlot].weaponDamage;
        core.bashDistance = InventoryReferenceSlot[selectedSlot].bashDistance;
        core.bashForce = InventoryReferenceSlot[selectedSlot].bashForce;
        
        switch (InventoryReferenceSlot[selectedSlot].reloadType)
        {
            case weaponStats.ReloadType.mag:
                core.reloadType = weaponCore.ReloadType.mag;
                break;
            case weaponStats.ReloadType.single:
                core.reloadType = weaponCore.ReloadType.single;
               break;
        }

        switch (InventoryReferenceSlot[selectedSlot].weaponType)
        {
            case weaponStats.equipType.firearm:
                core.weaponType = weaponCore.equipType.firearm;
                break;
            case weaponStats.equipType.melee:
                core.weaponType = weaponCore.equipType.melee;
                core.meleeDistance = InventoryReferenceSlot[selectedSlot].meleeDistance;
                core.meleeDuration = InventoryReferenceSlot[selectedSlot].meleeDuration;
                core.meleeForce = InventoryReferenceSlot[selectedSlot].meleeForce;

                break;
            case weaponStats.equipType.singleConsumable:
                core.weaponType = weaponCore.equipType.singleConsumable;
                break;
            case weaponStats.equipType.consumable:
                core.weaponType = weaponCore.equipType.consumable;
                break;
        }
     
        core.reloadTime = InventoryReferenceSlot[selectedSlot].reloadTime;
        core.reloadInsertTime = InventoryReferenceSlot[selectedSlot].reloadInsertTime;
        core.recoilAmount = InventoryReferenceSlot[selectedSlot].recoilAmount;
        core.moveDevTime = InventoryReferenceSlot[selectedSlot].moveDevTime;
        core.moveDevReturnTime = InventoryReferenceSlot[selectedSlot].moveDevReturnTime;


        core.casing = InventoryReferenceSlot[selectedSlot].casing;
        core.ejectionPort = InventoryReferenceSlot[selectedSlot].ejectionPort; 
        core.ejectionForce = InventoryReferenceSlot[selectedSlot].ejectionForce;
        core.ejectionSpin = InventoryReferenceSlot[selectedSlot].ejectionSpin;
        core.elapseCycleTime = 0;
        
        core.bashStep = 0;

        core.healAmount = InventoryReferenceSlot[selectedSlot].healAmount;
        core.speedBuff = InventoryReferenceSlot[selectedSlot].speedBuff ;
        core.timeToConsumue = InventoryReferenceSlot[selectedSlot].timeToConsumue;
        core.bashCost = InventoryReferenceSlot[selectedSlot].bashCost;




    }
    void AssignWeaponAnimations()
    {
        if(!InventoryReferenceSlot[selectedSlot].weaponAnimator)
        {
            return;
        }
        core.gunAnimationCont.weaponAnimator = InventoryReferenceSlot[selectedSlot].weaponAnimator;
        core.gunAnimationCont.weaponName = InventoryReferenceSlot[selectedSlot].weaponName;
    }

    void assignWorldWeaponAmmoID()
    {
        if (weaponAmmoIDList.Count == 0)
        {
            weaponAmmoIDList.Add(InventoryReferenceSlot[selectedSlot].weaponAmmoID);
            currentAmmoList.Add(InventoryReferenceSlot[selectedSlot].currentAmmo);
            ammopoolList.Add(InventoryReferenceSlot[selectedSlot].ammoPool);
        }

        findAmmoReference();
    }

    public void callFindAmmoReference()
    {
        for (int f = 0; f < weaponAmmoIDList.Count; f++)
        {
            if (InventoryReferenceSlot[selectedSlot].weaponAmmoID == weaponAmmoIDList[f])
            {
                //find ammo
                currentAmmoList[f] = core.currentAmmo;
                ammopoolList[f] =  core.ammoPool;
                return;
            }
        }
    }

    void findAmmoReference()
    {
        for(int f = 0; f < weaponAmmoIDList.Count; f++)
        {
            if(InventoryReferenceSlot[selectedSlot].weaponAmmoID == weaponAmmoIDList[f])
            {
                //find ammo
                core.weaponAmmoID = weaponAmmoIDList[f];
                core.currentAmmo = currentAmmoList[f];
                core.ammoPool = ammopoolList[f];
                return;
            }
        }

        //does not find ammo create storage for data

        weaponAmmoIDList.Add(InventoryReferenceSlot[selectedSlot].weaponAmmoID);
        currentAmmoList.Add(InventoryReferenceSlot[selectedSlot].currentAmmo);
        ammopoolList.Add(InventoryReferenceSlot[selectedSlot].ammoPool);

        core.weaponAmmoID = weaponAmmoIDList[weaponAmmoIDList.Count-1];
        core.currentAmmo = currentAmmoList[currentAmmoList.Count-1];
        core.ammoPool = ammopoolList[ammopoolList.Count-1];



    }

}
