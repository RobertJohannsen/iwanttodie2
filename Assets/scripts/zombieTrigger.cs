using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieTrigger : MonoBehaviour
{
    public bool hasTriggered;
    public zombieSpawnerScript[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
     
        if(!hasTriggered)
        {
            if (col.gameObject != null)
            {
                if (col.gameObject.tag == "Player")
                {
                    Debug.Log(col.gameObject);
                    hasTriggered = true;
                    for(int i = 0; i < spawners.Length; i++)
                    {
                        spawners[i].activateSpawner();
                    }
                }
            }
        }
        
    }
}
