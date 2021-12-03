using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponColliderBehaviour : MonoBehaviour
{
    private weaponCore core;
    private weaponStats stats;
    private bool isThrown;
    private void Awake()
    {
        core = this.gameObject.GetComponentInParent<weaponCore>();
        stats = this.gameObject.GetComponentInParent<weaponStats>();
    }

    public void Update()
    {
        stats = this.gameObject.GetComponentInParent<weaponStats>();
    }
    public void weaponThrown()
    {
        isThrown = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!core) { return; }
        if(!isThrown)
        {
            if (core.startBash)
            {


                switch (other.gameObject.tag)
                {
                    case "zombieHead":
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(stats.bashForce);
                        core.moveCore.slowStacks -= 2;
                        core.moveCore.slowStacks = Mathf.Clamp(core.moveCore.slowStacks, 0, core.moveCore.maxSlowStacks);
                        break;
                    case "zombieBody":
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(stats.bashForce);
                        core.moveCore.slowStacks -= 2;
                        core.moveCore.slowStacks = Mathf.Clamp(core.moveCore.slowStacks, 0, core.moveCore.maxSlowStacks);
                        break;
                }

            }
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {

        if (isThrown)
        {
            if(other.gameObject != null)
            {
                switch (other.gameObject.tag)
                {
                    case "zombieHead":
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(stats.weaponDamage, 100);
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(stats.bashForce);
                        break;
                    case "zombieBody":
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(stats.weaponDamage, 2.5f);
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(stats.bashForce);
                        break;
                }
            }
          

            isThrown = false;
        }
        else
        {
            if(!core) { return; }
            if (core.inMelee)
            {
                switch (other.gameObject.tag)
                {
                    case "zombieHead":
                        weaponAudioLibrary audioLib = this.GetComponentInParent<weaponAudioLibrary>();
                        if (audioLib)
                        {
                            audioLib.playImpact();
                        }

                        Instantiate(this.GetComponent<storeBloodSplat>().bloodFX, this.transform.position , Quaternion.identity);
                        if (core.currentWeaponID != 0)
                        {
                            other.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(stats.weaponDamage, 100);
                        }
                        else
                        {
                            other.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(stats.weaponDamage, 1);
                        }
                        
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(stats.bashForce);
                        break;
                    case "zombieBody":
                        Instantiate(this.GetComponent<storeBloodSplat>().bloodFX, this.transform.position, Quaternion.identity);
                        weaponAudioLibrary audioLib2 = this.GetComponentInParent<weaponAudioLibrary>();
                        if (audioLib2)
                        {
                            audioLib2.playImpact();
                        }
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(stats.weaponDamage, 1);
                        other.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(core.bashForce);
                        break;
                    case "zombieDoll":
                        Instantiate(this.GetComponent<storeBloodSplat>().bloodFX, this.transform.position, Quaternion.identity);
                        weaponAudioLibrary audioLib3 = this.GetComponentInParent<weaponAudioLibrary>();
                        if (audioLib3)
                        {
                            audioLib3.playImpact();
                        }
                        break;
                }
            }
        }
       

       
    }

    

}
