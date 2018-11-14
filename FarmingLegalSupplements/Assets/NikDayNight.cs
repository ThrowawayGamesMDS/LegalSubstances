using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikDayNight : MonoBehaviour {
    public GameObject sun;
    public GameObject moon;
    public GameObject Bloodmoon;
    Material sky;
    bool FinalNight = false;
    bool night = false;

    public int playeddays;
    // Use this for initialization
    void Start () {
        sky = RenderSettings.skybox;
        sky.SetFloat("_AtmosphereThickness", 1);

        playeddays = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //normaql night
        if ((sun.GetComponent<look>().CanMove == false)&& (moon.GetComponent<look>().CanMove == false)&&(FinalNight == false))
        {
            if (night == false)
            {
                night = true;
                sky.SetFloat("_AtmosphereThickness", 0);
                moon.GetComponent<look>().go();
                }
            else
            {
                playeddays++;
                night = false;
                sky.SetFloat("_AtmosphereThickness", 1);
                sun.GetComponent<look>().go();
            }
        }
        if ((sun.GetComponent<look>().CanMove == false) && (Bloodmoon.GetComponent<look>().CanMove == false) && (FinalNight == true))
        {
            if (night == false)
            {
                night = true;
                sky.SetFloat("_AtmosphereThickness", 0);
                Bloodmoon.GetComponent<look>().go();
            }
            else
            {
                playeddays++;
                night = false;
                sky.SetFloat("_AtmosphereThickness", 1);
                sun.GetComponent<look>().go();
            }
        }
        //blood night 
    }
}
