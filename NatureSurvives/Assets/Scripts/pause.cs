using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour {

    public static pause m_sPauseHandle;
    [SerializeField] private GameObject pauseMenuUI;
    public bool m_bPlayerPaused;

    // Use this for initialization
    void Start() {
        //Time.timeScale = 0f;
    }

    private void Awake()
    {
        if (m_sPauseHandle == null)
        {
            m_sPauseHandle = this;
            m_bPlayerPaused = false;
        }
    }

    // Update is called once per frame
    void Update() {

        //if (Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) && PlacementHandler.m_sPHControl.m_ePlayerState != PlacementHandler.PlayerStates.PLACING) // Player can't pause whilst placing buildings
        if (Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Space))
        {
            if (m_bPlayerPaused)
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
        m_bPlayerPaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
    }

    //public void DeactivateMenu()
    public void Resume()
    {
        m_bPlayerPaused = false;
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
        m_bPlayerPaused = true;
     
    }

    public void Unpaused()
    {
        m_bPlayerPaused = false;
       
    }




}

