using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class croshairBehaviour : MonoBehaviour
{
    public float baseX ,maxX , curX , scaleX , scaleY;
    public Transform nxC , xC , nyC , yC;
    public weaponCore wepCore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nxC.localScale = new Vector3(scaleX, scaleY, 1);
        nyC.localScale = new Vector3(scaleX, scaleY, 1);
        xC.localScale = new Vector3(scaleX, scaleY, 1);
        yC.localScale = new Vector3(scaleX, scaleY, 1);
    }

     void LateUpdate()
    {
        curX = wepCore.getGunAccuracyNow(baseX, maxX);
        updateCrosshair();
    }

    void updateCrosshair()
    {
       
        curX = Mathf.Clamp(curX, baseX, maxX);
        nxC.localPosition = new Vector3(-curX, 0, 0);
        xC.localPosition = new Vector3(curX, 0, 0);
        nyC.localPosition = new Vector3(0, -curX, 0);
        yC.localPosition = new Vector3(0, curX, 0);
    }
}

