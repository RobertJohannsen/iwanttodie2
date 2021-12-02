using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieSoundController : MonoBehaviour
{
    public float footstepMaxPitch, footstepMinPitch;
    public float currentFootstepPitch;
    public float minSpeed, maxSpeed;
    public AudioSource footstepSource;
    public AudioSource idleSource;
    public AudioSource attackSource;
    public zombieAI zCore;
    public int randomStep, randomIdle, randomAlert ,randomHit;
    public AudioClip[] footstep;
    public AudioClip[] idleSounds;
    public AudioClip[] alertSounds;
    public AudioClip[] hitSounds;
    public int idleStep, idleThres, idleMin;
    public bool alertDebounce ,idleDebounce;

    // Start is called before the first frame update
    void Start()
    {
        zCore = GetComponentInParent<zombieAI>();
        idleThres = idleMin + Random.Range(0, 200);
    }


    private void FixedUpdate()
    {
        idleStep++;
        if ((idleStep == idleThres) && zCore.audioState == zombieAI.soundState.idle)
        {
            assignIdleSound();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (zCore.currentSpeed > minSpeed)
        {
            footstepSource.enabled = true;
        }
        else
        {
            footstepSource.enabled = false;
        }

       

        if ((zCore.audioState == zombieAI.soundState.inrange) && !alertDebounce)
        {
            alertDebounce = true;
            assignAlertSound();
        }

        if ((zCore.audioState == zombieAI.soundState.idle) && !idleDebounce)
        {
            idleDebounce = true;
            assignIdleSound();
        }

        if (zCore.audioStateChanged)
        {
            Debug.Log("changed");
            alertDebounce = false ;
            idleDebounce = false;
        }
    }

    public void playFootstep()
    {
        if (footstepSource.enabled)
        {
            randomStep = Random.Range(0, 4);
            footstepSource.clip = footstep[randomStep];
            footstepSource.Play();
        }

    }

    public void assignIdleSound()
    {
        randomIdle = Random.Range(0, idleSounds.Length);
        idleSource.clip = idleSounds[randomIdle];
        idleSource.Play();
    }

    public void assignAlertSound()
    {
        randomAlert = Random.Range(0, alertSounds.Length);
        idleSource.clip = alertSounds[randomAlert];
        idleSource.Play();
    }

    public void playAttackSound()
    {
        randomHit = Random.Range(0, hitSounds.Length);
        attackSource.clip = hitSounds[randomHit];
        attackSource.Play();
    }
}
