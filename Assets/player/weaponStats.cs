using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class weaponStats : MonoBehaviour
{
    [Header("assignables")]
    public string weaponName;
    public Animator weaponAnimator;
    public GameObject weaponMesh;
    public GameObject weaponBarrel;
    public GameObject muzzleFlash;
    public GameObject weaponCollider;

    public enum equipType { firearm, melee, singleConsumable, consumable ,throwable }
    public equipType weaponType;

    public enum useType { zoomies , hp , tenk , fire , attract , explode}
    public useType itemType;

    [Header("status")]
    public int currentWeaponID;
    public int weaponAmmoID;
    public bool canFullAuto;
    public bool isThrown;

    [Header("assignable behaviour")]
    public int shotCount;
    public float baseAimConeAccuracy, moveAimConeAccuracy;
    public int ejectFrames, totalCycleTime;
    public float maxShotDistance;
    public int magCapacity, ammoPool;
    
    public int bashDuration;
    public int weaponDamage;
    public float bashDistance, bashForce;
    
    public enum ReloadType { mag, single };
    public ReloadType reloadType;
    public int reloadTime, reloadInsertTime;

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
    public int currentAmmo;

    [Header("Melee")]
    public int meleeDuration;
    public float meleeDistance, meleeForce;

    [Header("Consume")]
    public int healAmount;
    public float speedBuff;
    public int timeToConsumue;


    public void Awake()
    {
        weaponAnimator = this.gameObject.GetComponent<Animator>();
    }
}
