using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCircle : MonoBehaviour
{
    public float circleRadius , coneHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 testpoint = new Vector3(Random.insideUnitCircle.x *circleRadius, 0 , Random.insideUnitCircle.y * circleRadius);

        Physics.Raycast(testpoint + this.transform.position, Vector3.up);
        Debug.DrawRay(testpoint + this.transform.position, Vector3.up , Color.blue);
    }
}
