using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponAudioLibrary : MonoBehaviour
{
    public AudioSource fire ,sfx ,empty;
    public AudioClip weaponFire, reloadEject, reloadInsert, chargePull, chargeRelease, emptySound, bash ,equipMag , impact;

    public void playFire()
    {
        if (!weaponFire) return;
        fire.clip = weaponFire;
        if(fire.clip) fire.Play();
    }

    public void playReloadInsert()
    {
        if (!reloadInsert) return;
        sfx.clip = reloadInsert;
        if (sfx.clip) sfx.Play();

    }
    public void playReloadEject()
    {
        if (!reloadEject) return;
        sfx.clip = reloadEject;
        if (sfx.clip) sfx.Play();

    }
    public void playClick()
    {
        if (!emptySound) return;
        empty.clip = emptySound;
        if (empty.clip) empty.Play();

    }
    public void playchargePull()
    {
        if (!chargePull) return;
        sfx.clip = chargePull;
        if (sfx.clip) sfx.Play();

    }
    public void playchargeRelease()
    {
        if (!chargeRelease) return;
        sfx.clip = chargeRelease;
        if (sfx.clip) sfx.Play();

    }

    public void playEquipMag()
    {
        if (!equipMag) return;
        sfx.clip = equipMag;
        if (sfx.clip) sfx.Play();
    }

    public void playBash()
    {
        if (!bash) return;
        sfx.clip = bash;
        if (sfx.clip) sfx.Play();
    }

    public void playImpact()
    {
        if (!impact) return;
        sfx.clip = impact;
        if (sfx.clip) sfx.Play();
    }
  






}
