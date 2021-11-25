using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashBehaviour : MonoBehaviour
{
    private int existCount;
    public int existTime;
    public GameObject fakeBarrel,sparks, smoke ,smokePuff;


    // Start is called before the first frame update
    void Start()
    {
        fakeBarrel = GameObject.FindGameObjectWithTag("Player").GetComponent<moveCont>().wepCore.plyInv.fakeBarrel;
       GameObject sperk = Instantiate(sparks, this.transform.position, Quaternion.identity);
        sperk.transform.rotation = fakeBarrel.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0 , 90 , 0));
        existCount++;

        if(existCount >= existTime)
        {
            GameObject smeke = Instantiate(smoke, this.transform.position, Quaternion.identity);
            smeke.transform.parent = fakeBarrel.transform;
            GameObject smekePuff = Instantiate(smokePuff, this.transform.position, Quaternion.identity);
            smekePuff.transform.rotation = fakeBarrel.transform.rotation;
            Destroy(this.gameObject);
        }
    }
}
