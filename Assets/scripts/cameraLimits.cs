using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLimits : MonoBehaviour
{
    public float limX, limY, limZ;
    public GameObject head, cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.localPosition = new Vector3(
            Mathf.Clamp(cam.transform.localPosition.x, -limX, limX),
            Mathf.Clamp(cam.transform.localPosition.y, -limY, limY),
            Mathf.Clamp(cam.transform.localPosition.z, -limZ, limZ)
            );
    }
}
