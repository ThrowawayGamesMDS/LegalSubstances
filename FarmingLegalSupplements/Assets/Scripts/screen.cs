using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screen : MonoBehaviour {
    public Dropdown veiwingsize;
    public Dropdown screenview;

    // Use this for initialization
    void Start () {
        if (Screen.fullScreen == true)
        {
            veiwingsize.value = 0;
        }
        else
        {
            veiwingsize.value = 1;
        }
       
        switch (Screen.height)
        {
            case 1200:
                screenview.value = 0;
                break;
            case 900:
                screenview.value = 1;
                break;
            case 1024:
                screenview.value = 2;
                break;
            case 600:
                screenview.value = 3;
                break;
            default:
                break;
        }
    }
	
	public void screenVeiw()
    {
        switch (veiwingsize.value)
        {
            case 0:
                Screen.fullScreen = true;
                break;

            case 1:
                Screen.fullScreen = false;
                break;

            default:
                break;
        }
    }

    public void screensize()
    {
        switch (screenview.value)
        {
            case 0:
                Screen.SetResolution(1920,1200,Screen.fullScreen);
                break;

            case 1:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;

            case 2:
                Screen.SetResolution(1280, 1024, Screen.fullScreen);
                break;

            case 3:
                Screen.SetResolution(800, 600, Screen.fullScreen);
                break;

            default:
                break;
        }
    }
}
