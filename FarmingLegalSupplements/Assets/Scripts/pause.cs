using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour {

    public bool Pause = false;
    public GameObject pauseButton;
    // Use this for initialization
    void Start() {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (Pause == false)
            {
                //pasue game
               
               
                paused();
            }
            else
            {
                Unpaused();
            }



        }

        if (Pause == false)
        {
            //pasue game
            
            pauseButton.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseButton.SetActive(true);
            Time.timeScale = 0f;


        }
    }

    public void paused (){
        Pause = true;
     
    }

    public void Unpaused()
    {
        Pause = false;
       
    }


public void reset()
{
    SceneManager.LoadScene(2);
}
public void quit()
{
    print("quit");

    Application.Quit();
}
}

