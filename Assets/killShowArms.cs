using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killShowArms : MonoBehaviour
{
    public MeshRenderer upperL, upperR, lowerL, lowerR, handL, handR;
    public bool armsEnabled;
    // Start is called before the first frame update
    void Start()
    {
        armsEnabled = false;
    }

    void Update()
    {
       
        if(this.transform.parent.parent)
        {
            armsEnabled = true;
        }
        else
        {
            armsEnabled = false;
        }
            
        upperL.enabled = armsEnabled;
        lowerL.enabled = armsEnabled;
        handL.enabled = armsEnabled;
        upperR.enabled = armsEnabled;
        lowerR.enabled = armsEnabled;
        handR.enabled = armsEnabled;
    }
    public void turnArms()
    {
        armsEnabled = !armsEnabled;
    }
}
