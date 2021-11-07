using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ammoUI : MonoBehaviour
{
    public moveCont core;
    public TextMeshProUGUI mag, pool;

    // Update is called once per frame
    void Update()
    {
        mag.text = ""+core.wepCore.currentAmmo;
        pool.text = "" + core.wepCore.ammoPool;

    }
}
