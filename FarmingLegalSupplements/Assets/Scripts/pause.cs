using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    public void paused (){
        pauseButton.SetActive(true);
        Time.timeScale = 0f;
        Pause = true;
    }

    public void Unpaused()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(false);
        Pause = false;
    }
}

