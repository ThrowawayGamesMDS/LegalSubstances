using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playbutton : MonoBehaviour {
    CursorLockMode wantedMode;
    public GameObject playmenu;
    public GameObject helpmenu;
    public GameObject controlsmenu;

    public Camera camA, camB, camC;
    // Use this for initialization
    void Start () {
        camA.enabled = true;
        camB.enabled = false;
        camC.enabled = false;
        Cursor.lockState = wantedMode = CursorLockMode.None;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public void play(){
        SceneManager.LoadScene(2);
    }
    public void help()
    {
        camB.enabled = true;
        camA.enabled = false;
        camC.enabled = false;

        playmenu.SetActive(false);
        helpmenu.SetActive(true);
        controlsmenu.SetActive(false);
    }
    public void back()
    {
        camA.enabled = true;
        camB.enabled = false;
        camC.enabled = false;

        playmenu.SetActive(true);
        helpmenu.SetActive(false);
        controlsmenu.SetActive(false);
    }
    public void con()
    {
        camC.enabled = true;
        camA.enabled = false;
        camB.enabled = false;

        playmenu.SetActive(false);
        helpmenu.SetActive(false);
        controlsmenu.SetActive(true);
    }
    public void quit()
    {
        print("quit");

        Application.Quit();
    }
    
}
