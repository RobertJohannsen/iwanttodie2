using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodbath : MonoBehaviour
{
    public int bloodIntensity;
    public GameObject bloodSpawner;
    // Start is called before the first frame update
    void Start()
    {
        bloodExplosion(bloodIntensity);
    }

    void bloodExplosion(int intensity)
    {
        for(int i = 0; i < intensity; i++)
        {
            var bloodSpawn = Instantiate(bloodSpawner, this.transform.position, Quaternion.identity);
            bloodSpawn.transform.rotation = Quaternion.Euler(0, Random.Range(-180, 180), Random.Range(0, 90));
            bloodSpawn.GetComponent<Rigidbody>().AddForce(bloodSpawn.transform.forward * 50);
        }
    }
}
