using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {
    public float daynightSpeed;
    public GameObject sun;
    public static bool isDay;
    private bool b_day;
    public static int DaysPlayed;
    public int playeddays;
    private bool counted;
	// Use this for initialization
	void Start () {
        DaysPlayed = 0;
        counted = false;
	}
	
	// Update is called once per frame
	void Update () {

        if(sun.transform.position.y < 0)
        {
            isDay = false;
            if(counted)
            {
                counted = !counted;
            }
        }
        else
        {
            if(!counted)
            {
                playeddays++;
                counted = !counted;
            }
            isDay = true;
        }
        transform.Rotate(daynightSpeed * Time.deltaTime, 0, 0);
        b_day = isDay;
        DaysPlayed = playeddays;
	}
}
