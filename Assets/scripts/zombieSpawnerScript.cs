using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieSpawnerScript : MonoBehaviour
{
    public GameObject zombie;
    public bool isActive;
    public int zombiePool , currentAmount;
    public float timeStep, timeBetweenSpawns;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            timeStep += Time.deltaTime;

            if(timeStep >= timeBetweenSpawns)
            {
                timeStep = 0;
                trySpawnZombie();
            }
        }
    }

    public void activateSpawner()
    {
        isActive = true;
    }

    public void trySpawnZombie()
    {
        if(zombiePool > 0)
        {
            zombiePool--;
            Instantiate(zombie, this.transform.position, Quaternion.identity);
        }

        if(zombiePool <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
