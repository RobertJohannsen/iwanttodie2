





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class plyStats : MonoBehaviour
{
    public int plyHPMax, plyHP;
    public int currentScene;
    public bool isTenk;
    public int lockedHP;
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if(plyHPMax == 0)
        {
            plyHPMax = 1;
        }
        plyHP = plyHPMax;
    }

    // Update is called once per frame
    void Update()
    {
       

        plyHP = Mathf.Clamp(plyHP, 0, 100);
        if(plyHP == 0)
        {
            SceneManager.LoadScene(currentScene);
        }
    }

    private void LateUpdate()
    {
        if (isTenk)
        {
            if (plyHP != lockedHP)
            {
                plyHP = lockedHP;
            }
        }
    }
}
