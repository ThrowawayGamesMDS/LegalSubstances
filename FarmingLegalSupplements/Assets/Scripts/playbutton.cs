using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playbutton : MonoBehaviour {
    CursorLockMode wantedMode;
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
        SceneManager.LoadScene(1);
    }
    public void back()
    {
        SceneManager.LoadScene(0);
    }
    public void quit()
    {
        print("quit");

        Application.Quit();
    }
}
