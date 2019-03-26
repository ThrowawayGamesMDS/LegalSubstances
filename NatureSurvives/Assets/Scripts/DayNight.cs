using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {
    public float m_fDayTime;
    public float m_fNightTime;
    public GameObject sun;
    public static bool isDay;
    private bool b_day;
    public static int DaysPlayed;
    public int playeddays;
    private bool counted;

    public int daytimer;
    public int nighttimer;
	// Use this for initialization
	void Start () {
        DaysPlayed = 0;
        counted = false;
        daytimer = Mathf.RoundToInt(m_fDayTime * 60);
        nighttimer = Mathf.RoundToInt(m_fNightTime * 60);

    }
	
	// Update is called once per frame
	void Update () {

        if(sun.transform.position.y < 0)
        {
            
            isDay = false;
            //sun.SetActive(false);
            if (counted)
            {                
                counted = !counted;
                NotificationManager.Instance.SetNewNotification("Test Notification: Night time, enemy is coming");
            }
        }
        else
        {
            
            if (!counted)
            {
                playeddays++;
                counted = !counted;
            }
            isDay = true;
            //sun.SetActive(true);
        }
        
        if(isDay)
        {
            transform.Rotate(((180 * Time.deltaTime) / daytimer), 0, 0);
        }
        if(!isDay)
        {
            transform.Rotate(((180 * Time.deltaTime) / nighttimer), 0, 0);
        }
        
        b_day = isDay;
        DaysPlayed = playeddays;
	}

    public void WonderMode()
    {
        transform.eulerAngles = new Vector3(180, 0, 0);
        DaysPlayed = 8;
        m_fDayTime = 9999999;
        isDay = false;
        //InvokeRepeating("invokeroony", 0.1f, 40.0f);
    }


    void invokeroony()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<EnemySpawner>().isinvoked = false;
    }
}
