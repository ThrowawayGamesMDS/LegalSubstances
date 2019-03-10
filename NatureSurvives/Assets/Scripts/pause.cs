using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour {

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool Pause;

    // Use this for initialization
    void Start() {
        //Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            if (Pause)
            {
                Resume();                
            }
            else
            {
                PauseGame();
            }

        }
        
    }
    void PauseGame()
    {
        Pause = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
    }

    //public void DeactivateMenu()
    public void Resume()
    {
        Pause = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
    }

    public void reset()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void quit()
    {
        Application.Quit();
    }

    public void paused (){
        Pause = true;
     
    }

    public void Unpaused()
    {
        Pause = false;
       
    }




}

