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

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pause == false)
            {
                //pasue game
                //GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HouseController>() //player can't move
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
            //GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HouseController>() //player can't move
            pauseButton.SetActive(false);
           
        }
        else
        {
            pauseButton.SetActive(true);
            if (Time.timeScale !=0)
            {
                Pause = false;
            }
            
            
        }
    }

    public void paused (){
        
        Time.timeScale = 0f;
        Pause = true;
    }

    public void Unpaused()
    {
        Pause = false;
        Time.timeScale = 1f;
       
        
        
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

