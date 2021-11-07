using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponAnimationControls : MonoBehaviour
{
    public weaponCore core;
    // Start is called before the first frame update
    void Start()
    {
        core = this.GetComponentInParent<weaponCore>();
    }

    // Update is called once per frame
   public void stopBash()
    {
        Debug.Log("stopped");
        core.startBash = false;
    }
}
