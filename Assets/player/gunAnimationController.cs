using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAnimationController : MonoBehaviour
{
    public plyInventory plyInv;
    public Animator weaponAnimator;

    public weaponAnimationLibrary weaponAnimLib;
    public weaponAudioLibrary audioLib;
    public string weaponName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void callShootAnimation()
    {
        if(getAnimator()) weaponAnimator.PlayInFixedTime("shoot" , 0 , 0.0f);
        if (getAudioLib()) audioLib.playFire();
    }

    public void callReloadAnimation()
    {

        if (getAnimator()) weaponAnimator.Play("reloadStart");
    }

    public void callReloadEndAnimation()
    {

        if (getAnimator()) weaponAnimator.Play("reloadEnd");
    }
    public void callBashAnimation()
    {

        if (getAnimator()) weaponAnimator.PlayInFixedTime("bash" , 0 , 0.0f);
    }

    public void callThrowStartAnimation()
    {
        if (getAnimator()) weaponAnimator.Play("throwStart");
    }

    public void callEquipAnimation()
    {
        if (getAnimator()) weaponAnimator.Play("equip");
    }

    public void callReloadRackAnimation()
    {
        if (getAnimator()) weaponAnimator.Play("reloadRack");
    }

    bool getAnimator()
    {
        if(plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<Animator>())
        {
            weaponAnimator = plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<Animator>();
            if(!weaponAnimator.enabled)
            {
                weaponAnimator.enabled = true;
            }
           
            return true;
        }
        else
        {
            return false;
        }
    }

    bool getAudioLib()
    {
        if (plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponAudioLibrary>())
        {
            audioLib = plyInv.weaponRoot.transform.GetChild(1).gameObject.GetComponent<weaponAudioLibrary>();

            return true;
        }
        else
        {
            return false;
        }
    }

 }
