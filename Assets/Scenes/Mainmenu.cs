using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 

public class Mainmenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      
    }
    
    public void Tutorial()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

    }


    public void Credits()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);

    }




    public void QuitGame ()

    {
        Application.Quit();
    }
}
