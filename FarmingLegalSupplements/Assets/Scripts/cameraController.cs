﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    public float scrollSpeed;
    public Vector3 m_vec2CursorPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //scrolling on x axis

        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(scrollSpeed * Time.deltaTime, 0, 0);
        }
        //scrolling on y axis
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(0, 0, -scrollSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(0, 0, scrollSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            scrollSpeed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            scrollSpeed = 20.0f;
        }


        /*
        if (Input.mousePosition.x < 100 || Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-scrollSpeed * Time.deltaTime,0,0);
            if (Input.mousePosition.x < 50)
            {
                gameObject.transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
            }
        }
        if(Input.mousePosition.x > Screen.width - 100 || Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(scrollSpeed * Time.deltaTime, 0, 0);
            if (Input.mousePosition.x > Screen.width - 50)
            {
                gameObject.transform.Translate(scrollSpeed * Time.deltaTime, 0, 0);
            }
        }
        //scrolling on y axis
        if (Input.mousePosition.y < 100 || Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(0, 0, -scrollSpeed * Time.deltaTime);
            if (Input.mousePosition.y < 50)
            {
                gameObject.transform.Translate(0, 0, -scrollSpeed * Time.deltaTime);
            }
        }
        if (Input.mousePosition.y > Screen.height - 100 || Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(0, 0, scrollSpeed * Time.deltaTime);
            if (Input.mousePosition.y > Screen.height - 50)
            {
                gameObject.transform.Translate(0, 0, scrollSpeed * Time.deltaTime);
            }
        }
        */


        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0,50*Time.deltaTime,0);
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -50 * Time.deltaTime, 0);

        }

        /*if (Input.GetKey(KeyCode.Mouse1))
        {
            if (m_vec2CursorPos.x < Input.mousePosition.x)
            {
                transform.Rotate(0, 200 * Time.deltaTime, 0);
            }
            else if (m_vec2CursorPos.x > Input.mousePosition.x)
            {
                transform.Rotate(0, -200 * Time.deltaTime, 0);
            }
            m_vec2CursorPos = Input.mousePosition;
        }*/


        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            transform.Translate(0, -100 * Time.deltaTime, 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            transform.Translate(0, 100 * Time.deltaTime, 0);
        }

        if(transform.position.y < 10)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
        if (transform.position.y > 60)
        {
            transform.position = new Vector3(transform.position.x, 60, transform.position.z);
        }
        m_vec2CursorPos = Input.mousePosition;

    }
}
