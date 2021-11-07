using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponAudioLibrary : MonoBehaviour
{
    public AudioSource fire , reloadInsert , click , reloadFinish , reloadEject;

    public void playFire()
    {
        if(fire.clip) fire.Play();
    }

    public void playReloadInsert()
    {
        if (reloadInsert.clip) reloadInsert.Play();

    }
    public void playClick()
    {
        if (click.clip) click.Play();

    }
    public void playReloadFinish()
    {
        if (reloadFinish.clip) reloadFinish.Play();

    }
    public void playReloadEject()
    {
        if (reloadEject.clip) reloadEject.Play();

    }






}
