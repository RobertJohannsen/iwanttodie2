using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorBehaviour : MonoBehaviour
{
    public float maxDoorOpenAngle;
    public bool doorOpen;
    public bool doorOpenLeft;
    public Transform hinge;
    public BoxCollider doorCollider;
    public MeshRenderer mesh;
    public int doorHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        doorCollider.isTrigger = doorOpen;
        mesh.enabled = !doorOpen;
        if(doorHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void openDoor()
    {
        doorOpen = !doorOpen;
    }
}
