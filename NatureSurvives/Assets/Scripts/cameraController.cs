using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class cameraController : MonoBehaviour
{

    public static cameraController m_sCameraControl;

    public float scrollSpeed;
    public Vector3 m_vec2CursorPos;
    public Camera m_pCamera;
    private bool m_pAlterFov;
    private Rect m_rScreenRect;
    public float m_zoomSpeed = 2.5f;

    float moveSpeed = 1.0f;
    float rotX = 0.0f;
    float rotY = 0.0f;
    float mouseX = 0.0f;
    float mouseY = 0.0f;
    Quaternion localRotation;
    public Transform castleobj;

    public bool m_bCamSelObjRotation; // Camera has an object to rotate around
    public GameObject m_goRotateAround;
    public bool m_bCamReset;

    public bool m_bPlayerCheatReset;
    public Vector3 m_vec3PlayerPos;

    private float lastFrameTime;

    [Header("Lerp for Idle camera shift")]
    private Vector3 m_vec3LerpPos;
    public float m_fTransitionTime;
    private float m_fRotationTransitionTime;
    private Vector3 m_vec3OrigPos;
    private Vector3 m_vec3OrigRot;
    private Vector3 m_vec3LerpRot;
    [SerializeField]
    private bool m_bLerpTarget;
    private bool m_bCameraRotLerp;
    [Range(0.0f, 1.0f)]
    public float m_fTransitionSpeed;

    // Use this for initialization
    void Start()
    {
        if (m_sCameraControl == null)
        {
            m_sCameraControl = this;
            m_pAlterFov = false;
            if (m_zoomSpeed == 0)
            {
                m_zoomSpeed = 2.5f;
            }

            m_rScreenRect = new Rect(0, 0, Screen.width, Screen.height);

            m_bCamSelObjRotation = false;
            m_bCamReset = false;
            m_goRotateAround = null;
            m_vec3LerpPos = new Vector3();
            m_vec3OrigPos = new Vector3();
            m_vec3LerpRot = new Vector3();
            m_vec3OrigRot = new Vector3();
            m_vec3OrigRot = gameObject.transform.rotation.eulerAngles;
            m_bLerpTarget = false;
            m_bPlayerCheatReset = false;
            m_bCameraRotLerp = false;
            m_fTransitionTime = 0.0f;
            m_fTransitionSpeed = 0.2f;
            m_fRotationTransitionTime = 0.0f;
            lastFrameTime = Time.realtimeSinceStartup;
        }
    }


    private bool RotateCameraAroundSelectedObject()
    {
        int _test = 0;
        if (!gameObject.GetComponent<SelectionBox>().m_bCtrlSelectUnits)
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (selectableObject.selectionCircle != null)
                {
                    _test++;
                    m_goRotateAround = selectableObject.gameObject;
                }
                if (_test > 1)
                {
                    m_goRotateAround = null;
                    return false;
                }
            }
        return true;
    }

    public void UpdateCameraTargetForLerping(Vector3 _vec3TargetPos)
    {
        m_vec3LerpPos = _vec3TargetPos;

        // Bundy handle to reset camera zoom on idle select
        if (m_vec3LerpPos.y != 50)
        {
            m_vec3LerpPos.y = 50;
        }

        m_vec3OrigPos = transform.position;
        m_fTransitionTime = 0.0f;
        m_bLerpTarget = true;
       /* if (gameObject.transform.rotation.eulerAngles != m_vec3OrigRot)
        {
            m_bCameraRotLerp = true;
            m_vec3LerpRot = gameObject.transform.rotation.eulerAngles;
        }*/
    }

    void Orbit()
    {
        if (m_goRotateAround != null)
        {
            // Keep us at orbitDistance from target
            transform.position = m_goRotateAround.transform.position + (transform.position - m_goRotateAround.transform.position).normalized * 150.0f;
            transform.RotateAround(m_goRotateAround.transform.position, Vector3.up, 180.0f * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (CheatHandler.m_sCheatHandler.m_bPlayerIsEnteringCheat && !m_bPlayerCheatReset)
        {
           m_bPlayerCheatReset = true;
           m_vec3PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
           return;
        }

        if (CheatHandler.m_sCheatHandler.m_bPlayerIsEnteringCheat)
            return;

        if (!CheatHandler.m_sCheatHandler.m_bPlayerIsEnteringCheat && m_bPlayerCheatReset)
        {
            m_bPlayerCheatReset = false;
            GameObject.FindGameObjectWithTag("Player").transform.position = m_vec3PlayerPos;
        }
         
        //I'm using my own delta time calculation, so that the camera can't be paused when Time.timeScale = 0
        float myDeltaTime = Time.realtimeSinceStartup - lastFrameTime;
        lastFrameTime = Time.realtimeSinceStartup;

        /*  if (!m_bCamSelObjRotation && Camera.main.transform.rotation.y != gameObject.transform.rotation.y && m_bCamReset)
          {
              Vector3 _playerRot = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
              Vector3 _playerPos = gameObject.transform.position;

              //Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(gameObject.transform.forward,gameObject.transform.up), 15);
              Camera.main.transform.forward = gameObject.transform.forward;
              Camera.main.transform.rotation = Quaternion.Euler(45.0f, _playerRot.y, 0);
              Camera.main.transform.position = _playerPos;
              m_bCamReset = false;
          }*/

        //scrolling on x axis

        // if (!m_bLerpTarget)
        // {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (m_bLerpTarget)
                m_bLerpTarget = false;
            //gameObject.transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
            gameObject.transform.Translate(-scrollSpeed * myDeltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (m_bLerpTarget)
                m_bLerpTarget = false;
            gameObject.transform.Translate(Vector3.right * scrollSpeed * myDeltaTime);
            //gameObject.transform.Translate( Vector3.right * scrollSpeed * Time.deltaTime);
            //gameObject.transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime, 0, 0);
        }
        //scrolling on y axis
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (m_bLerpTarget)
                m_bLerpTarget = false;
            //gameObject.transform.Translate(0, 0, -scrollSpeed * Time.deltaTime);
            gameObject.transform.Translate(0, 0, -scrollSpeed * myDeltaTime);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (m_bLerpTarget)
                m_bLerpTarget = false;
            //gameObject.transform.Translate(0, 0, scrollSpeed * Time.deltaTime);
            gameObject.transform.Translate(0, 0, scrollSpeed * myDeltaTime);
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

            rotY += mouseX * moveSpeed;
            rotX += mouseY * moveSpeed;

            localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;

        }


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
                        //transform.Translate(0, -100 * Time.deltaTime, 0);
                        transform.Translate(0, -100 * myDeltaTime, 0);
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
                        //transform.Translate(0, 100 * Time.deltaTime, 0);
                        transform.Translate(0, 100 * myDeltaTime, 0);
                        break;
                    }
            }
        }



        float screenMargin = 20f;
        //if(testPanning)
        //{
        // sorry saw you could turn off by bool but thought i'd add this anyways
        if (!m_rScreenRect.Contains(Input.mousePosition))
            return;

        //if (!EditorWindow.mouseOverWindow)
        // return;

        float panSpeed = scrollSpeed * myDeltaTime;

        if (Input.mousePosition.x < screenMargin)
        {
            gameObject.transform.Translate(-panSpeed, 0, 0);
        }
        else if (Input.mousePosition.x > Screen.width - screenMargin)
        {
            gameObject.transform.Translate(panSpeed, 0, 0);
        }

        if (Input.mousePosition.y < screenMargin)
        {
            gameObject.transform.Translate(0, 0, -panSpeed);
        }
        else if (Input.mousePosition.y > Screen.height - screenMargin)
        {
            gameObject.transform.Translate(0, 0, panSpeed);
        }
        //Vector3 move = new Vector3(x, y, z) + transform.position;
        //transform.position = move;           
        //}

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
        if (Input.GetKey(KeyCode.Q))
        {
            //if (bSetCameraLookat)
            //Ray r = new Ray(transform.position, transform.LookAt - transform.position);
            //RaycastHit hit;
            //if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
            //Vector3 point = m_pCamera.ScreenToWorldPoint(new Vector3(m_pCamera.pixelWidth / 2, m_pCamera.pixelHeight / 2, m_pCamera.nearClipPlane));

            Ray ray = m_pCamera.ScreenPointToRay(new Vector3(m_pCamera.pixelWidth / 2, m_pCamera.pixelHeight / 2, 0));

            if (Physics.Raycast(ray, out hit))
            {
                //Physics.Raycast(transform.position, -Vector3.up, out hit);
                //Vector3 pivotPoint = new Vector3(5.0f, 0.0f, 5.0f); //modify that value
                //transform.RotateAround(hit.point, Vector3.up, 20 * Time.deltaTime);
                transform.RotateAround(hit.point, Vector3.up, 20 * myDeltaTime);
                //transform.RotateAround(pivotPoint, transform.up, 20 * Time.deltaTime);

                rotX = transform.rotation.eulerAngles.x;
                rotY = transform.rotation.eulerAngles.y;
            }

        }
        if (Input.GetKey(KeyCode.E))
        {
            Ray ray = m_pCamera.ScreenPointToRay(new Vector3(m_pCamera.pixelWidth / 2, m_pCamera.pixelHeight / 2, 0));
            if (Physics.Raycast(ray, out hit))
            {
                //transform.RotateAround(hit.point, Vector3.up, -20 * Time.deltaTime);
                transform.RotateAround(hit.point, Vector3.up, -20 * myDeltaTime);

                rotX = transform.rotation.eulerAngles.x;
                rotY = transform.rotation.eulerAngles.y;
            }
        }


        if (transform.position.y < 10)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
        if (transform.position.y > 60)
        {
            transform.position = new Vector3(transform.position.x, 60, transform.position.z);
        }
        m_vec2CursorPos = Input.mousePosition;

        if (m_bLerpTarget || m_bCameraRotLerp)
        {
            if (m_bLerpTarget)
            {
                m_fTransitionTime += m_fTransitionSpeed;
                if (gameObject.transform.position != m_vec3LerpPos)
                {
                    gameObject.transform.position = Vector3.Lerp(m_vec3OrigPos, m_vec3LerpPos, m_fTransitionTime);

                }
                else
                    m_bLerpTarget = false;
                
            }

           /* if (m_bCameraRotLerp)
            {
                m_fRotationTransitionTime += Time.deltaTime;
                if (gameObject.transform.rotation.eulerAngles != m_vec3OrigRot)
                {

                    //gameObject.transform.eulerAngles = m_vec3OrigRot;
                    gameObject.transform.eulerAngles = Vector3.Lerp(m_vec3OrigPos, m_vec3OrigRot, m_fRotationTransitionTime);
                }
                else
                    m_bCameraRotLerp = false;
            }*/

        }
    }

    /*private void LateUpdate()
    {
        if (m_bLerpTarget) // lerp functionality
        {
            if (gameObject.transform.position != m_vec3LerpPos)
            {
                gameObject.transform.position = Vector3.Lerp(m_vec3OrigPos, m_vec3LerpPos, m_fTransitionTime);
            }
            else
                m_bLerpTarget = false;
        }
    }*/
}
