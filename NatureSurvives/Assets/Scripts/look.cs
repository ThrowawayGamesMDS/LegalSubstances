using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look : MonoBehaviour {
    public GameObject point;
    public bool CanMove = false;
    float speed;
    public float bestspeed;
    public Transform startingPoint;
    public float intensiy;
	// Use this for initialization
	void Start () {
        speed = bestspeed;

    }
	
	// Update is called once per frame
	void Update () {
        if (CanMove == true)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                go();
            }
                transform.RotateAround(point.transform.position,Vector3.right,speed * Time.deltaTime);
            
            gameObject.transform.LookAt(point.transform.position);
            

            if ((transform.position.y >= -9700) && (transform.position.y <= -6200))
            {
                transform.position = startingPoint.position;
                //speed = 500;
               gameObject.SetActive(false);
                CanMove = false;
            }
            else
            {
                speed = bestspeed;
            }

            if (transform.position.y <= -2000)
            {
               GetComponent<Light>().intensity = 0.0f;
            }
            if (intensiy >= GetComponent<Light>().intensity)
            {
                GetComponent<Light>().intensity = 0.003f*intensiy + GetComponent<Light>().intensity;
            }

        }
       
	}
    public void go()
    {
        gameObject.SetActive(true);
        CanMove = true;
    }
}
