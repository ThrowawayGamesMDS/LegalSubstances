using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playbutton : MonoBehaviour {
    CursorLockMode wantedMode;
    public GameObject playmenu;
    public GameObject helpmenu;
    public GameObject controlsmenu;
    
    // Use this for initialization
    void Start () {
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

        playmenu.SetActive(false);
        helpmenu.SetActive(true);
        controlsmenu.SetActive(false);
    }
    public void back()
    {

        playmenu.SetActive(true);
        helpmenu.SetActive(false);
        controlsmenu.SetActive(false);
    }
    public void con()
    {

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
