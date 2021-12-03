using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomiseZombie : MonoBehaviour
{
    public SkinnedMeshRenderer zombieRenderer;
    public int clothes, skin, pants;
    public Material[] clothMat, skinMat, pantsMat ,defaultMaterials;
    // Start is called before the first frame update
    void Start()
    {
        
        zombieRenderer = this.gameObject.GetComponent<SkinnedMeshRenderer>();
        defaultMaterials = zombieRenderer.materials;
        clothes = Random.Range(0, clothMat.Length);
        skin = Random.Range(0, skinMat.Length);
        pants = Random.Range(0, pantsMat.Length);
        defaultMaterials[0] = clothMat[clothes];
        defaultMaterials[1] = skinMat[skin];
        defaultMaterials[2] = pantsMat[pants];
        zombieRenderer.materials = defaultMaterials;


    }
}
