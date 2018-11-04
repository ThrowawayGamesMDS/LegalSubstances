using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikDayNight : MonoBehaviour {
    public GameObject sun;
    public GameObject moon;
    public GameObject Bloodmoon;
    bool FinalNight = true;
    bool night = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //normaql night
        if ((sun.GetComponent<look>().CanMove == false)&& (moon.GetComponent<look>().CanMove == false)&&(FinalNight == false))
        {
            if (night == false)
            {
                night = true;
                moon.GetComponent<look>().go();
                }
            else
            {
                night = false;
                sun.GetComponent<look>().go();
            }
        }
        if ((sun.GetComponent<look>().CanMove == false) && (Bloodmoon.GetComponent<look>().CanMove == false) && (FinalNight == true))
        {
            if (night == false)
            {
                night = true;
                Bloodmoon.GetComponent<look>().go();
            }
            else
            {
                night = false;
                sun.GetComponent<look>().go();
            }
        }
        //blood night 
    }
}
