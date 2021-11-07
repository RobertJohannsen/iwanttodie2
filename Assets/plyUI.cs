using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class plyUI : MonoBehaviour
{

    public TextMeshProUGUI ammoText, healthText;
    public weaponCore core;
    public plyStats stats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        ammoText.text = core.currentAmmo + " || " + core.ammoPool;
        if (core.plyInv.weaponRoot.transform.GetChild(1).tag == "hands") ammoText.text = "";
        if (core.plyInv.weaponRoot.transform.GetChild(1).GetComponent<ignoreUI>()) ammoText.text = "";
        healthText.text = ""+stats.plyHP;
    }
}
