using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwableBehaviour : MonoBehaviour
{
    public bool throwableEnabled;
    public int throwTime, throwStep;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(throwableEnabled)
        {
            throwStep++;
            if(throwStep >= throwTime)
            {
                doThrowableEffect();
            }
        }
    }

    void doFire()
    {
        // make fire here
    }

    void doThrowableEffect()
    {
        if (this.gameObject.GetComponent<weaponStats>())
        {
            switch (this.gameObject.GetComponent<weaponStats>().itemType)
            {
                case weaponStats.useType.fire:
                    doFire();
                    break;
                case weaponStats.useType.explode:

                    break;
                case weaponStats.useType.attract:

                    break;
            }
        }

        // makeExplosion
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(throwableEnabled)
        {
            doThrowableEffect();
            //make explosion
        }
    }
}

