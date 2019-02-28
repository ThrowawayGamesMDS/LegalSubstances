using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    public float scrollSpeed;
    public Vector3 m_vec2CursorPos;
    public Camera m_pCamera;
    private bool m_pAlterFov;
    public float m_zoomSpeed = 2.5f;
    public bool testPanning = true;
    // Use this for initialization
    void Start () {
        m_pAlterFov = false;
        if (m_zoomSpeed == 0)
        {
            m_zoomSpeed = 2.5f;
        }
        testPanning = true;
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

        if (Input.GetMouseButton(2))
        {
            Vector3 _temp = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
            print("X: " +Input.mousePosition.x + ", Y: " + Input.mousePosition.y);
            
            if (m_vec2CursorPos.x - 20 < Input.mousePosition.x && m_vec2CursorPos.x + 20 > Input.mousePosition.x)
            {
                if (Input.mousePosition.x > Screen.width / 2)
                {
                    _temp.y += 1;
                }
                else
                {
                    _temp.y -= 1;
                }
            }

            /*if (m_vec2CursorPos.y-20 < Input.mousePosition.y && m_vec2CursorPos.y + 20 > Input.mousePosition.y)
            {
                if (Input.mousePosition.y > Screen.height / 2)
                {
                    _temp.x += 3;
                }
                else
                {
                    _temp.x -= 3;
                }
            }*/
            gameObject.transform.Rotate(_temp);
            m_vec2CursorPos = Input.mousePosition;

        }

        /* if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
         {
             print ("FUCKING ALTER THE FOV");
             if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) // forward
             {
                 if (m_pCamera.fieldOfView < 115)
                 {
                     // m_pCamera.fieldOfView += 0.25f;
                     Camera.main.fieldOfView = 115;
                   //  m_pCamera.fieldOfView = 115;
                     //gameObject.GetComponent<Camera>().fieldOfView += 0.25f;
                 }
             }
             else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) // backwards
             {
                 if (m_pCamera.fieldOfView > 60)
                 {
                     //m_pCamera.fieldOfView = 60;
                     Camera.main.fieldOfView = 60;
                     //m_pCamera.fieldOfView -= 0.25f;
                 }
             }
         }
         else
         {
             if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
             {
                 transform.Translate(0, -100 * Time.deltaTime, 0);
             }
             else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
             {
                 transform.Translate(0, 100 * Time.deltaTime, 0);
             }
         }*/
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            m_pAlterFov = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            m_pAlterFov = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) // forward
        {
            switch (m_pAlterFov)
            {
                case true:
                    {
                        if (m_pCamera.fieldOfView > 60)
                        {
                            m_pCamera.fieldOfView -= m_zoomSpeed;
                        }
                        break;
                    }
                case false:
                    {
                        transform.Translate(0, -100 * Time.deltaTime, 0);
                        break;
                    }
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) // backwards
        {
            switch (m_pAlterFov)
            {
                case true:
                    {
                        if (m_pCamera.fieldOfView < 115)
                        {
                            m_pCamera.fieldOfView += m_zoomSpeed;
                        }
                        break;
                    }
                case false:
                    {
                        transform.Translate(0, 100 * Time.deltaTime, 0);
                        break;
                    }
            }
        }


        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0,50*Time.deltaTime,0);
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -50 * Time.deltaTime, 0);

        }


        float screenMargin = 30f;
        if(testPanning)
        {
            float x = 0, y = 0, z = 0;
            float panSpeed = screenMargin * Time.deltaTime;

            if(Input.mousePosition.x < screenMargin)
            {
                gameObject.transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
            }
            else if(Input.mousePosition.x > Screen.width - screenMargin)
            {
                gameObject.transform.Translate(scrollSpeed * Time.deltaTime, 0, 0);
            }

            if (Input.mousePosition.y < screenMargin)
            {
                gameObject.transform.Translate(0, 0, -scrollSpeed * Time.deltaTime);
            }
            else if (Input.mousePosition.y > Screen.height - screenMargin)
            {
                gameObject.transform.Translate(0, 0, scrollSpeed * Time.deltaTime);
            }
            //Vector3 move = new Vector3(x, y, z) + transform.position;
            //transform.position = move;
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
