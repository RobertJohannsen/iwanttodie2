using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponFireBehaviour : MonoBehaviour
{
    private weaponCore core;
    private weaponAudioLibrary audioLib;
    // Start is called before the first frame update
    private void Awake()
    {
        core = GameObject.FindGameObjectWithTag("bodyRoot").GetComponent<weaponCore>();
        audioLib = this.gameObject.GetComponent<weaponAudioLibrary>();
        
    }
    // Update is called once per frame
    public void resetFire()
    {
        if(core.currentAmmo - 1 > 0)
        {
            core.shootReady = true;
            core.elapseCycleTime = 0;
        }
       
    }
    public void killCurrentWeapon()
    {
        Debug.Log("killed obj");
        core.discardCurrentWeapon();
    }
    public void becomeTank()
    {
        core.becomeTenk();
    }

    public void becomeZoom()
    {
        core.becomeSpeed();
    }

    public void healPly()
    {
        core.healPly(core.healAmount);
    }
    
    public void startMeleeAttack()
    {
        core.inMelee = true;
    }

    public void endMeleeAttack()
    {
        core.inMelee = false;
    }



    public void playReloadClick()
    {
        audioLib.playClick();
    }

    public void playEjectSound()
    {
        audioLib.playReloadEject();
    }

  }
