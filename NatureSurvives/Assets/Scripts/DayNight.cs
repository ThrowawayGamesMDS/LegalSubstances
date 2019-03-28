using System.Collections;
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
    //private float startHealth;
    //private Transform healthBarCanvasTransform;   

    public float sunAngleDay;
    public float sunAngleNight;
    public float sunTransformPositionY;
    public float maxSunTransformPositionY;

    public float angle = 0.0f;
    public Vector3 axis;
    public Vector3 euler;
    public Vector3 LocalEuler;

    //daytime = 10
    //night time  = 5


    // Use this for initialization
    void Start () {
        DaysPlayed = 0;
        counted = false;
        daytimer = Mathf.RoundToInt(m_fDayTime * 60);
        nighttimer = Mathf.RoundToInt(m_fNightTime * 60);

        sunTransformPositionY = sun.transform.position.y;
        //print("sunTransformPositionY = " + sunTransformPositionY);
        maxSunTransformPositionY = sunTransformPositionY;

        timerUI = FindObjectOfType<Slider>().gameObject;
        roundUI = GameObject.Find("RoundUI");
        


        //healthBarCanvasTransform = transform.Find("Health Bar");

        roundImage = roundUI.transform.GetChild(0).GetComponent<Image>();
        //healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
        //healthBarCanvasGameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        transform.rotation.ToAngleAxis(out angle, out axis);
        euler = transform.eulerAngles;
        LocalEuler = transform.localEulerAngles;

        sunTransformPositionY = sun.transform.position.y;

        

        if (maxSunTransformPositionY < sun.transform.position.y)
        {
            maxSunTransformPositionY = sunTransformPositionY;
            
        }
        else
        {
            //print("maxSunTransformPositionY = " + maxSunTransformPositionY);
            //print("maxSunTransformPositionX = " + sun.transform.position.x);
            //print("maxSunTransformPositionZ = " + sun.transform.position.z);
            //print("maxSunTransformRotationx = " + sun.transform.rotation.x);

            if (maxSunTransformPositionY > 0)
            {
                sunTransformPositionY = 2 * maxSunTransformPositionY - sun.transform.position.y;
            }
            
        }

        float sliderValue = -1 * euler.x;

        if (euler.x > 90)
        {
            euler.x = euler.x - 360;
        }

        timerUI.GetComponent<Slider>().value = -1 * euler.x;

        roundImage.fillAmount = (-1 * euler.x + 85 )/ 170;

        if (sun.transform.position.y < 0)
        {
            

            isDay = false;
            //sun.SetActive(false);
            if (counted)
            {                
                counted = !counted;
                NotificationManager.Instance.SetNewNotification("Test Notification: Night time, enemy is coming");

                //print("NIGHT TIME");
                //print("Transform Rotation X = " + transform.rotation.x);

                //print("Transform Rotation X = " + angle);
                //print("Transform Rotation Euler X = " + euler.x);
                //print("Transform Rotation localEuler X = " + LocalEuler.x);

            }
        }
        else
        {
            
            if (!counted)
            {
                playeddays++;
                counted = !counted;

                //print("DAY TIME");
                //print("Transform Rotation X = " + transform.rotation.x);

                //print("Transform Rotation X = " + angle);
                //print("Transform Rotation Euler X = " + euler.x);
                //print("Transform Rotation localEuler X = " + LocalEuler.x);

            }
            isDay = true;
            //sun.SetActive(true);

            
        }
        
        if(isDay)
        {
            //transform.Rotate(((Time.deltaTime) / daytimer), 0, 0);
            transform.Rotate(((180 * Time.deltaTime) / daytimer), 0, 0);
            sunAngleDay = (180 * Time.deltaTime) / daytimer;
        }
        if(!isDay)
        {
            //transform.Rotate(((Time.deltaTime) / nighttimer), 0, 0);
            transform.Rotate(((180 * Time.deltaTime) / nighttimer), 0, 0);
            sunAngleDay = (180 * Time.deltaTime) / nighttimer;
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
