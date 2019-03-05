using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class cameraController : MonoBehaviour {
    public float scrollSpeed;
    public Vector3 m_vec2CursorPos;
    public Camera m_pCamera;
    private bool m_pAlterFov;
    private Rect m_rScreenRect;
    public float m_zoomSpeed = 2.5f;

    public bool testPanning = false;
    float moveSpeed = 1.0f;
    float rotX = 0.0f;
    float rotY = 0.0f;
    float mouseX = 0.0f;
    float mouseY = 0.0f;
    Quaternion localRotation;
    public Transform castleobj;

    // Use this for initialization
    void Start () {
        m_pAlterFov = false;
        if (m_zoomSpeed == 0)
        {
            m_zoomSpeed = 2.5f;
        }

        m_rScreenRect = new Rect(0,0, Screen.width, Screen.height);
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
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            mouseX = horizontal;
            mouseY = -vertical;

            //Vector3 m_euler = transform.rotation.ToEuler();

            //rotY = m_euler.x;
            //rotX = m_euler.y;



            rotY += mouseX * moveSpeed;
            rotX += mouseY * moveSpeed;

            localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            print("transform.rotation before = " + transform.rotation);
            transform.rotation = localRotation;

            print("mouse horizontal = " + horizontal);
            print("mouse vertical = " + vertical);

            print("rotY = " + rotY);
            print("rotx = " + rotX);
            print("Quaternion = " + localRotation);
            print("transform.rotation after = " + transform.rotation);

            // Vector3 _temp = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
            //print("X: " +Input.mousePosition.x + ", Y: " + Input.mousePosition.y);

            //if (m_vec2CursorPos.x - 20 < Input.mousePosition.x && m_vec2CursorPos.x + 20 > Input.mousePosition.x)
            //{
            //    if (Input.mousePosition.x > Screen.width / 2)
            //    {
            //        _temp.y += 0.5f;
            //    }
            //    else
            //    {
            //        _temp.y -= 0.5f;
            //    }
            //}

            //if (m_vec2CursorPos.y - 20 < Input.mousePosition.y && m_vec2CursorPos.y + 20 > Input.mousePosition.y)
            //{
            //    if (Input.mousePosition.y > Screen.height / 2)
            //    {
            //        _temp.x += 3;
            //    }
            //    else
            //    {
            //        _temp.x -= 3;
            //    }
            //}
            //gameObject.transform.Rotate(_temp);
            //m_vec2CursorPos = Input.mousePosition;

            //gameObject.transform.Rotate

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

        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) // forwarde
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
            transform.Rotate(0, 50*Time.deltaTime, 0);
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -50 * Time.deltaTime, 0);

        }


        float screenMargin = 30f;
        if(testPanning)
        {
            // sorry saw you could turn off by bool but thought i'd add this anyways
            if (!m_rScreenRect.Contains(Input.mousePosition))
                return;

            if (!EditorWindow.mouseOverWindow)
                return;

            float panSpeed = screenMargin * Time.deltaTime;

                if (Input.mousePosition.x < screenMargin)
                {
                    gameObject.transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
                }
                else if (Input.mousePosition.x > Screen.width - screenMargin)
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

        //bool bSetCameraLookat = false;
        //RaycastHit hit = new RaycastHit();

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    if (!bSetCameraLookat)
        //    {
        //        bSetCameraLookat = true;


        //        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        //        {
        //            var distanceToGround = hit.distance;
        //        }
        //    }
        //}
        RaycastHit hit = new RaycastHit();
        if (Input.GetKey(KeyCode.Z))
        {
            //if (bSetCameraLookat)
            //Ray r = new Ray(transform.position, transform.LookAt - transform.position);
            //RaycastHit hit;
            //if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
            //Vector3 point = m_pCamera.ScreenToWorldPoint(new Vector3(m_pCamera.pixelWidth / 2, m_pCamera.pixelHeight / 2, m_pCamera.nearClipPlane));

            Ray ray = m_pCamera.ScreenPointToRay(new Vector3(m_pCamera.pixelWidth/2, m_pCamera.pixelHeight / 2, 0));
            
            if (Physics.Raycast(ray, out hit))
            {
                //Physics.Raycast(transform.position, -Vector3.up, out hit);
                //Vector3 pivotPoint = new Vector3(5.0f, 0.0f, 5.0f); //modify that value
                transform.RotateAround(hit.point, Vector3.up, 20 * Time.deltaTime);
                //transform.RotateAround(pivotPoint, transform.up, 20 * Time.deltaTime);

                print("transform.rotation on Z press = " + transform.rotation);

                Vector3 m_euler = transform.rotation.ToEuler();
                //transform.localEulerAngles;
               // transform.eulerAngles();

                //rotY = m_euler.x;
                //rotX = m_euler.y;

            }

        }
        if (Input.GetKey(KeyCode.X))
        {
            Ray ray = m_pCamera.ScreenPointToRay(new Vector3(m_pCamera.pixelWidth / 2, m_pCamera.pixelHeight / 2, 0));
            if (Physics.Raycast(ray, out hit))
            {
                transform.RotateAround(hit.point, Vector3.up, -20 * Time.deltaTime);              
            }
        }
        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    transform.LookAt(hit.point, m_pCamera.transform.up);
        //}


        if (transform.position.y < 10)
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
