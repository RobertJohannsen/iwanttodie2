using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodsplat : MonoBehaviour
{
    public GameObject[] bloodsplatPrefab;
    public int splatCount, splatThres , splatLossy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
      //  if(!collision.gameObject.GetComponent<bloodsplat>())
      //  {
      //      splatCount++;
      //      Debug.Log("splat");

       //     int whatSplat = Random.Range(0, 2);
       //     float splatSize = Random.Range(0, 5);

       //     var bloodsplat = Instantiate(bloodsplatPrefab[whatSplat], new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z), Quaternion.identity);
       //     bloodsplat.transform.localRotation = Quaternion.Euler(90, this.transform.rotation.z, 0);
        //    bloodsplat.transform.localScale = new Vector3(splatSize, splatSize, splatSize);

       //     Destroy(this.gameObject);

       // }



    }
}
