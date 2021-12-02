using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieAnimationController : MonoBehaviour
{
    public zombieAI zCore;
    public Animator zombieAnimator;
    bool suspendAnim ,climbDebounce;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!suspendAnim)
        {
            if (!zCore.isMove) callIdleAnimation();

            float topTierSpeedThres = ((Mathf.Clamp(zCore.speedTopBracket, 0, 100)) / 100) * zCore.speedinUse;
            if (zCore.currentSpeed > topTierSpeedThres)
            {
                callSprintAnimation();
            }
            else
            {
                callcallRunAnimation();
            }
        }
        
    }

    public void callIdleAnimation()
    {
        if (getAnimator()) zombieAnimator.Play("idle");

      
    }

    public void callClimbStartAnimation()
    {
        if(!climbDebounce)
        {
            climbDebounce = true;
            suspendAnim = true;
            if (getAnimator()) zombieAnimator.Play("climbstart");
        }
        
    }

    public void callcallRunAnimation()
    {
        if (getAnimator()) zombieAnimator.Play("run");
    }

    public void callSprintAnimation()
    {
        if (getAnimator()) zombieAnimator.Play("sprint");
    }

    public void callAttackAnimation()
    {
        suspendAnim = true;
        if (Random.Range(0, 2) == 1)
        {
            if (getAnimator()) zombieAnimator.Play("attack1");
        }
        else
        {
            if (getAnimator()) zombieAnimator.Play("attack2");
        }

    }

    public void unsuspendAnim()
    {
        suspendAnim = false;
        climbDebounce = false;
    }

    bool getAnimator()
    {
        if (zombieAnimator)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
