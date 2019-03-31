﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI")]
    [SerializeField]
    public GameObject timerUI;
    public GameObject roundUI;
    private Image roundImage;
    public Vector3 euler;

    // Use this for initialization
    void Start () {

        DaysPlayed = 0;
        counted = false;
        daytimer = Mathf.RoundToInt(m_fDayTime * 60);
        nighttimer = Mathf.RoundToInt(m_fNightTime * 60);

        timerUI = FindObjectOfType<Slider>().gameObject;
        timerUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load("sunindicator", typeof(Sprite)) as Sprite;

        roundUI = GameObject.Find("RoundUI");      
        roundImage = roundUI.transform.GetChild(0).GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {

        UserInterface();

        if (sun.transform.position.y < 0)
        {
            isDay = false;

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

    void UserInterface()
    {
        euler = transform.eulerAngles;

        float sliderValue = -1 * euler.x;

        if (euler.x > 90)
        {
            euler.x = euler.x - 360;
        }

        timerUI.GetComponent<Slider>().value = -1 * euler.x;

        roundImage.fillAmount = (-1 * euler.x + 85) / 170;


        if ((-1 * euler.x) >= 85)
        {
            timerUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load("moonindicator", typeof(Sprite)) as Sprite;

        }
        if ((-1 * euler.x) <= -85)
        {
            timerUI.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load("sunindicator", typeof(Sprite)) as Sprite;

        }

        //if ((-1 * euler.x) >= 75)
        //{
        //    NotificationManager.Instance.SetNewNotification("Test Notification: Night time is near");
        //}
        //if ((-1 * euler.x) <= -75)
        //{
        //    NotificationManager.Instance.SetNewNotification("Test Notification: Day time is near");
        //}
    }

}
