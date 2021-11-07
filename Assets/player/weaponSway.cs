using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSway : MonoBehaviour
{

    public weaponCore core;

    public Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;
    public Quaternion def;
    private float timer;
    public Vector3 weaponRootPos;
    // Start is called before the first frame update
    void Start()
    {
        weaponRootPos = this.transform.localPosition; //change this to root pos when the game 
    }

    // Update is called once per frame
    void Update()
    {
        if(!core.doWeaponSway)
        {
            return;
        }

        Quaternion def = transform.localRotation;
        float factorX = (Input.GetAxis("Mouse Y")) * core.swayAmount;
        float factorY = -(Input.GetAxis("Mouse X")) * core.swayAmount;
        //float factorZ = -Input.GetAxis("Vertical") * core.swayAmount;
        float factorZ = 0 * core.swayAmount;


      
            factorX = Mathf.Clamp(factorX , -core.swayClamp , core.swayClamp);
            factorY = Mathf.Clamp(factorY, -core.swayClamp, core.swayClamp);
            factorZ = Mathf.Clamp(factorZ ,-core.swayClamp , core.swayClamp);

        Quaternion Final = Quaternion.Euler(def.x + factorX, def.y + factorY, def.z + factorZ);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Final, (Time.time * core.swaySmoothing));
        handleWeaponBob();
    }

    void handleWeaponBob()
    {
        if(!core.doWeaponBob)
        {
            return;
        }

        if(core.moveCore.isMove)
        {
            if(core.moveCore.grounded) 
            {
                timer += Time.deltaTime;
                Vector3 newWeaponPos = new Vector3(core.bobXAmp * Mathf.Sin(timer * core.bobFreq), core.bobYAmp * Mathf.Sin(timer * core.bobFreq * 2), 0);
                this.transform.localPosition = newWeaponPos + weaponRootPos;

            }

        }
        else
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition , weaponRootPos , core.bobSmoothing);
        }


    }
}
