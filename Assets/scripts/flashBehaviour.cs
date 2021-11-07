using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashBehaviour : MonoBehaviour
{
    private int existCount;
    public int existTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0 , 90 , 0));
        existCount++;

        if(existCount >= existTime)
        {
            Destroy(this.gameObject);
        }
    }
}
