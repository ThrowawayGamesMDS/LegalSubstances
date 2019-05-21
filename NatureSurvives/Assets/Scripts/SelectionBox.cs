using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utils
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }
    
    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
    
    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

}

/***
 * 
 * TO DO: Check for whether the npc has already been control selected.. 
 *          you can currently keep clicking and adding the same npc to the ctrl selected list
 * 
 ***/


public class SelectionBox : MonoBehaviour {

    public static SelectionBox m_bSBHandle;
    public bool isSelecting = false;
    public bool m_bCtrlSelectUnits = false;
    public List <GameObject> m_lCtrlUnits;
    Vector3 mousePosition1;
    public Camera camera;
    public GameObject[] m_goSelectOBJ; // 0 = DEFAULT, 1 = NPC, 2 = RESOURCE
    public GameObject selectionCirclePrefab;

    //private int m_iUserID = -1;
    private bool m_bUserLClicked;
    // whats up with the fucking coroutine system these days?????
    private float m_fUserClickedTime;

    /***
     * Unit controller variables
     *      - FUNCTION(1 & 2) Controller 
     ***/

    private int m_iActiveCombatWongles;
    public List<GameObject> m_goMeleeUnits;
    public List<GameObject> m_goRangedUnits;
    public bool[] m_bUnitsSelected;
    private GameObject m_goUnitDoubleClicked;
    public checkposofclick Checkposofclick;

    // For displayUI shit
    private GameObject m_goSelected;

    public bool m_bPlayerSelected;

    [Header("Idle Wongle Iter")]
    public int m_iIdleIterCount;
    public GameObject[] m_goActiveWongles;
    public List<GameObject> m_goActiveWorkers;
    public int m_iWongleWorkerCount;

    public int m_iUserID;

    private void Awake()
    {
#if !UNITY_EDITOR
            m_iUserID = 0;
#endif

        if (m_bSBHandle == null)
        {
            m_bSBHandle = this;
            //m_goMeleeUnits = new List<GameObject>();
            //m_goRangedUnits = new List<GameObject>();
            m_bUnitsSelected = new bool[2];
            m_bPlayerSelected = false;

            // bool system cheaper on performance then rechecking through an auto loop...
            m_bUnitsSelected[0] = false; // Melee units
            m_bUnitsSelected[1] = false; // Ranged Units
            m_iWongleWorkerCount = 0;
            m_iIdleIterCount = 0;
            m_goActiveWorkers = new List<GameObject>();
            RefreshWongleWorkerList();
            /* foreach (var wongle in _goUnits)
             {
                 if (wongle.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                     m_goActiveWorkers.Add(wongle);
             }*/

            //int _iTotalWongles = 3;

            GameObject[] _goUnits;
            _goUnits = GameObject.FindGameObjectsWithTag("Wongle");
            int _iTotalWongles = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().iCurrentWongleCount;

            // GET THE HOMESPAWNER TOTAL WONGLE SPAWN COUNT
            for (int i = 0; i < _iTotalWongles; i++)
            {
                if (_goUnits[i].GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Melee)
                {
                    m_goMeleeUnits.Add(_goUnits[i]);
                    m_iActiveCombatWongles++;
                }
                else if (_goUnits[i].GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Ranged)
                {
                    m_goRangedUnits.Add(_goUnits[i]);
                    m_iActiveCombatWongles++;
                }
            }
        }
    }

    /*  private void Start()
      {
          if (m_bSBHandle == null)
          {
              m_bSBHandle = this;
              //m_goMeleeUnits = new List<GameObject>();
              //m_goRangedUnits = new List<GameObject>();
              m_bUnitsSelected = new bool[2];
              m_bPlayerSelected = false;

              // bool system cheaper on performance then rechecking through an auto loop...
              m_bUnitsSelected[0] = false; // Melee units
              m_bUnitsSelected[1] = false; // Ranged Units
              m_iWongleWorkerCount = 0;
              m_iIdleIterCount = 0;
              m_goActiveWorkers = new List<GameObject>();
              RefreshWongleWorkerList();


              GameObject[] _goUnits;
              _goUnits = GameObject.FindGameObjectsWithTag("Wongle");
              int _iTotalWongles = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().iCurrentWongleCount;

              // GET THE HOMESPAWNER TOTAL WONGLE SPAWN COUNT
              for (int i = 0; i < _iTotalWongles; i++)
              {
                  if (_goUnits[i].GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Melee)
                  {
                      m_goMeleeUnits.Add(_goUnits[i]);
                      m_iActiveCombatWongles++;
                  }
                  else if (_goUnits[i].GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Ranged)
                  {
                      m_goRangedUnits.Add(_goUnits[i]);
                      m_iActiveCombatWongles++;
                  }
              }
          }
      }*/

    private void RefreshWongleWorkerList()
    {
        GameObject[] _goUnits;
        _goUnits = GameObject.FindGameObjectsWithTag("Wongle");
        // clear current list
        m_goActiveWorkers.Clear();

        // update list
        foreach (var wongle in _goUnits)
        {
            //inactive wongle
            if (wongle.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker
                /*&& wongle.GetComponent<WongleController>().agent.velocity.magnitude == 0*/
                && wongle.GetComponent<WongleController>().anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                && wongle.GetComponent<WongleController>().Target == null)
            {
                m_goActiveWorkers.Add(wongle);
                m_iWongleWorkerCount++;
            }
        }
    }

    private void CheckUnitForType(Ray ray, int layermask)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layermask))
        {
            if (m_goUnitDoubleClicked == hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<SelectableUnitComponent>() != null)
                {
                    SelectableUnitComponent unit = hit.transform.gameObject.GetComponent<SelectableUnitComponent>();



                    switch (unit.Type.ToString())
                    {
                        case "Melee":
                            {
                                SelectEntireUnit(false, true);
                                break;
                            }
                        case "Ranged":
                            {
                                SelectEntireUnit(false, false);
                                break;
                            }
                        default:
                            {
                                RemoveSelectionCircleFromObjects();
                                ResetWongleIdleIterFunctionality();
                                SelectRegular(ray, layermask, false);
                                break;
                            }
                    }
                    m_goUnitDoubleClicked = new GameObject();
                }
            }
            
        }
        return;




    }

    private RaycastHit GenerateRayCast(Ray ray, int layermask, bool _bCtrlSelect)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layermask))
        {
            List<GameObject> _goUpdateList = new List<GameObject>();
            if (hit.transform.gameObject.GetComponent<SelectableUnitComponent>() != null)
            {
                SelectableUnitComponent unit = hit.transform.gameObject.GetComponent<SelectableUnitComponent>();
                if (_bCtrlSelect)
                {
                    int _iBundyCount = 0;
                    foreach (var _npc in m_lCtrlUnits)
                    {
                        if (hit.transform.gameObject == _npc)
                        {
                            Destroy(unit.GetComponent<SelectableUnitComponent>().selectionCircle.gameObject);
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle = null;
                            unit.isSelected = false;
                            _goUpdateList.Add(_npc);
                            m_lCtrlUnits.RemoveAt(_iBundyCount);
                            _iBundyCount++;

                            GameObject healthBarCanvasGameObject;
                            healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                            healthBarCanvasGameObject.SetActive(false);


                            //if (DisplayHandler.m_sDHControl.m_bDisplayingText)
                            DisplayHandler.m_sDHControl.ResetState(true);

                            return hit;
                        }
                    }
                        m_lCtrlUnits.Add(hit.transform.gameObject);
                }
                else if (unit.selectionCircle == null)
                {
                    unit.selectionCircle = Instantiate(selectionCirclePrefab);
                    unit.selectionCircle.transform.SetParent(unit.transform, false);
                    unit.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);

                    GameObject healthBarCanvasGameObject;
                    healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                    healthBarCanvasGameObject.SetActive(true);
                }

                unit.isSelected = true;
            }

        }
        return hit;
    }


    private void RemoveSelectionCircleFromObjects()
    {
        bool _bResetState = false;
        foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
        {
            selectableObject.isSelected = false;
            if (selectableObject.selectionCircle != null)
            {
                Destroy(selectableObject.selectionCircle.gameObject);
                selectableObject.selectionCircle = null;

                GameObject healthBarCanvasGameObject;
                healthBarCanvasGameObject = selectableObject.transform.Find("Health Bar").gameObject;
                healthBarCanvasGameObject.SetActive(false);
                if (!_bResetState && selectableObject.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                {
                    //if (DisplayHandler.m_sDHControl.m_bDisplayingText)
                        DisplayHandler.m_sDHControl.ResetState(true);
                    _bResetState = true;
                }
            }
        }
    }

    /// <summary>
    /// Nooby as hell, SEU(true,false) = select every spawned combat unit.. SEU(FALSE,TRUE) = select all melee units.. SEU(FALSE,FALES) = select all ranged units..
    /// </summary>
    private void SelectEntireUnit(bool _bAllUnits, bool _bMeleeUnits)
    {

        // deselect other units?
        RemoveSelectionCircleFromObjects();
        ResetWongleIdleIterFunctionality();
        switch (_bAllUnits)
        {
            case true:
                {
                    foreach (var unit in m_goMeleeUnits)
                    {
                        if (unit.GetComponent<SelectableUnitComponent>().selectionCircle == null)
                        {
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
                            unit.GetComponent<SelectableUnitComponent>().isSelected = true;

                            GameObject healthBarCanvasGameObject;
                            healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                            healthBarCanvasGameObject.SetActive(true);
                        }
                    }
                    foreach (var unit in m_goRangedUnits)
                    {
                        if (unit.GetComponent<SelectableUnitComponent>().selectionCircle == null)
                        {
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
                            unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
                            unit.GetComponent<SelectableUnitComponent>().isSelected = true;

                            GameObject healthBarCanvasGameObject;
                            healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                            healthBarCanvasGameObject.SetActive(true);
                        }
                    }
                    break;
                }
            case false:
                {
                   switch(_bMeleeUnits)
                    {
                        case true:
                            {
                                foreach (var unit in m_goMeleeUnits)
                                {
                                    if (unit.GetComponent<SelectableUnitComponent>().selectionCircle == null)
                                    {
                                        unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
                                        unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
                                        unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
                                        unit.GetComponent<SelectableUnitComponent>().isSelected = true;

                                        GameObject healthBarCanvasGameObject;
                                        healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                                        healthBarCanvasGameObject.SetActive(true);
                                    }
                                }
                                break;
                            }
                        case false:
                            {
                                foreach (var unit in m_goRangedUnits)
                                {
                                    if (unit.GetComponent<SelectableUnitComponent>().selectionCircle == null)
                                    {
                                        unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
                                        unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
                                        unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
                                        unit.GetComponent<SelectableUnitComponent>().isSelected = true;

                                        GameObject healthBarCanvasGameObject;
                                        healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                                        healthBarCanvasGameObject.SetActive(true);
                                    }
                                }
                                break;
                            }
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// Nooby as again, if _bCheckIfWorker it will check the unit's type else it will always return true
    /// </summary>
    private bool SelectRegular(Ray ray, int layermask, bool _bCheckIfWorker)
    {
        var selectedObjects = new List<SelectableUnitComponent>();
        foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
        {
            if (IsWithinSelectionBounds(selectableObject.gameObject))
            {
                selectedObjects.Add(selectableObject);
            }
        }

        isSelecting = false;

        RaycastHit hit = GenerateRayCast(ray, layermask, false);

        if (_bCheckIfWorker && hit.transform != null)
        {
            if (hit.transform.gameObject.GetComponent<SelectableUnitComponent>().Type.ToString() == "Worker")
            {
                m_goSelected = hit.transform.gameObject;
                return true;
            }
            else
            {
                if (m_goSelected != null)
                {
                    m_goSelected = null;
                }
                m_goUnitDoubleClicked = hit.transform.gameObject;
                m_goSelected = null;
                return false;
            }
        }
        else
        {
            m_goSelected = null;
            return true;
        }

    }

    private void UpdateCameraVariables(Ray ray, int layermask)// for rotation around an obj
    {
        RaycastHit hit = GenerateRayCast(ray, layermask, false);

        if (hit.transform != null)
        {
            if (hit.transform.gameObject.GetComponent<SelectableUnitComponent>() != null)
            {
                gameObject.GetComponent<cameraController>().m_goRotateAround = hit.transform.gameObject;
                gameObject.GetComponent<cameraController>().m_bCamSelObjRotation = true;
            }
        }
    }

    private int ResetWongleIdleIterFunctionality()
    {
        /***
         * 
         * 
         * Resetting of the bool is also handled in WongleController -> in the if statement 'agent.velocity.magnitude > 0'
         * 
         * 
         ***/
        foreach (var unit in m_goActiveWorkers)
        {
            unit.GetComponent<WongleController>().m_bIdleSelected = false;
        }

        return 0;
    }

    /// <summary>
    /// Semi recursive for CTRL/Selection state. Takes gameobject being selected as parameter and yeah...
    /// </summary>
    private void AssignUIObjectsToSelected(GameObject unit)
    {
        unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
        unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
        unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);

        GameObject healthBarCanvasGameObject;
        healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
        healthBarCanvasGameObject.SetActive(true);

        if (unit.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker
            && gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == false)
        {

            gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
            DisplayHandler.m_sDHControl.UpdateAndDisplayWongleWorkerText(unit.gameObject); // remove this if we create a total/average function as per below

            if ( isSelecting && !m_bPlayerSelected)
            {
                m_bPlayerSelected = true;
                DisplayHandler.m_sDHControl.UpdateAndDisplayWongleWorkerText(unit); // remove this if we create a total/average function as per below
            }
        }

        unit.GetComponent<SelectableUnitComponent>().isSelected = true;
    }

    private bool CheckForSelectedCircle()
    {
        foreach (var _unit in FindObjectsOfType<SelectableUnitComponent>())
        {
            if (_unit.selectionCircle != null)
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {
      //  print("Mouse is over UI: " + EventSystem.current.IsPointerOverGameObject(-1));
        // Wee c++ style timer handle for double click select - coroutine was fucking out mega
        if (m_fUserClickedTime < Time.time && m_bUserLClicked == true)
        {
            m_bUserLClicked = false;
        }

        // If we press the left mouse button, begin selection and remember the location of the mouse
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            m_bCtrlSelectUnits = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            m_bCtrlSelectUnits = false;
        }

        if (Input.GetKeyUp(KeyCode.I)) // iter through idle workers
        {
            /***
             * 
             * 
             * Will only select the first one always, need to add a bool to the wonglecontroller to say that this one has been selected, or we could alternatively check the 
             * selectionCircle to see whether or not it is null.. 
             * 
             * Added that ^ but theres a weird bug on third select from start which will double select. need to fix later
             * 
             * 
             ***/
            bool _bIsDisplaying = false;
            
            RefreshWongleWorkerList();

            // potential bug handle
            // we should unselect all, and reset bools..
            RemoveSelectionCircleFromObjects();

            foreach (var unit in m_goActiveWorkers)
            {
                if (unit.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                {
                    if (!unit.GetComponent<WongleController>().m_bIdleSelected 
                        && !_bIsDisplaying 
                        && unit.GetComponent<SelectableUnitComponent>().selectionCircle == null) //becuz there isn't a state handle for wongles... so have to check by anim
                    {
                        Vector3 _newPos = new Vector3(unit.transform.position.x, Camera.main.transform.position.y, unit.transform.position.z - (Camera.main.transform.forward.magnitude * 50));
                        cameraController.m_sCameraControl.UpdateCameraTargetForLerping(_newPos);
                        // gameObject.transform.position = _newPos;

                        AssignUIObjectsToSelected(unit);

                        _bIsDisplaying = true;
                        unit.GetComponent<WongleController>().m_bIdleSelected = true;
                        m_iIdleIterCount++;

                    }
                    if (m_iIdleIterCount >= m_goActiveWorkers.Count)
                    {
                        m_iIdleIterCount = ResetWongleIdleIterFunctionality();
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SelectEntireUnit(true,false);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SelectEntireUnit(false,true);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SelectEntireUnit(false, false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            int layermask = LayerMask.GetMask("Ground");
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            string _sOriginalHitResult = "";
            Debug.DrawRay(camera.transform.position, ray.direction, Color.red);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                _sOriginalHitResult = hit.transform.gameObject.tag;
            }
            Physics.Raycast(ray, out hit, 1000, layermask);
            switch(_sOriginalHitResult)
            {
                case "Wood":
                case "Crystal":
                    {
                        if (DisplayHandler.m_sDHControl.m_bDisplayingBuildings) // worker unit's are selected
                            Instantiate(m_goSelectOBJ[2], hit.point, camera.transform.rotation);
                        else
                            Instantiate(m_goSelectOBJ[0], hit.point, camera.transform.rotation); // else just display white
                        break;
                    }
                case "Enemy":
                    {
                        Instantiate(m_goSelectOBJ[1], hit.point, camera.transform.rotation);
                        break;
                    }
                default:
                    {
                        print(hit.point);
                        Instantiate(m_goSelectOBJ[0], hit.point, camera.transform.rotation);
                        break;
                    }
            }
        }


        //if (Input.GetMouseButtonDown(0) && PlacementHandler.m_sPHControl.m_ePlayerState != PlacementHandler.PlayerStates.PLACING)
        if (Input.GetMouseButtonDown(0) )
        {
            if (PlacementHandler.m_sPHControl.m_ePlayerState == PlacementHandler.PlayerStates.PLACING || EventSystem.current.IsPointerOverGameObject(-1) == true) // we need to watch this, -1 is the user id. could change dependent on build type
                return;

            //if(!Checkposofclick.isHovered) // this might be obsolete
            {
                bool _bResetState = false;
                if (!m_bUserLClicked)
                {
                    switch (m_bCtrlSelectUnits)
                    {
                        case false:
                            {
                                //if (EventSystem.current.IsPointerOverGameObject(m_iUserID) == false)
                                {
                                    if (m_lCtrlUnits.Count > 0)
                                    {
                                        foreach (var unit in m_lCtrlUnits)
                                        {
                                            if (unit.GetComponent<SelectableUnitComponent>().selectionCircle != null)
                                            {
                                                Destroy(unit.GetComponent<SelectableUnitComponent>().selectionCircle.gameObject);
                                                unit.GetComponent<SelectableUnitComponent>().selectionCircle = null;

                                                GameObject healthBarCanvasGameObject;
                                                healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                                                healthBarCanvasGameObject.SetActive(false);

                                                if (!_bResetState && unit.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                                                {
                                                    //if (DisplayHandler.m_sDHControl.m_bDisplayingText)
                                                    DisplayHandler.m_sDHControl.ResetState(true);
                                                    _bResetState = true;
                                                }
                                            }
                                        }
                                        m_lCtrlUnits.Clear();
                                    }
                                    isSelecting = true;
                                    mousePosition1 = Input.mousePosition;
                                    foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
                                    {
                                        if (selectableObject.selectionCircle != null)
                                        {
                                            print(selectableObject.name);
                                            print(selectableObject.transform.position);
                                            Destroy(selectableObject.selectionCircle.gameObject);
                                            selectableObject.selectionCircle = null;

                                            GameObject healthBarCanvasGameObject;
                                            healthBarCanvasGameObject = selectableObject.transform.Find("Health Bar").gameObject;
                                            healthBarCanvasGameObject.SetActive(false);
                                            /* if (!_bResetState && selectableObject.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                                             {
                                                 //if (DisplayHandler.m_sDHControl.m_bDisplayingText)
                                                 DisplayHandler.m_sDHControl.ResetState(true);
                                                 print("resetting state4");
                                                 _bResetState = true;
                                             }*/
                                        }
                                    }
                                }
                                break;
                            }
                        case true:
                            {
                                //if (EventSystem.current.IsPointerOverGameObject(m_iUserID) == false)
                                {
                                    mousePosition1 = Input.mousePosition;

                                    foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
                                    {
                                        if (selectableObject.selectionCircle != null && !m_bCtrlSelectUnits)
                                        {
                                            Destroy(selectableObject.selectionCircle.gameObject);
                                            selectableObject.selectionCircle = null;

                                            GameObject healthBarCanvasGameObject;
                                            healthBarCanvasGameObject = selectableObject.transform.Find("Health Bar").gameObject;
                                            healthBarCanvasGameObject.SetActive(false);
                                            if (!_bResetState && selectableObject.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                                            {
                                                //if (DisplayHandler.m_sDHControl.m_bDisplayingText)
                                                DisplayHandler.m_sDHControl.ResetState(true);
                                                _bResetState = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            
        }
        // If we let go of the left mouse button, end selection
        //if (Input.GetMouseButtonUp(0) && PlacementHandler.m_sPHControl.m_ePlayerState != PlacementHandler.PlayerStates.PLACING)
        if (Input.GetMouseButtonUp(0))
        {
            if (PlacementHandler.m_sPHControl.m_ePlayerState == PlacementHandler.PlayerStates.PLACING || EventSystem.current.IsPointerOverGameObject(-1) == true)
                return;



            int layermask = LayerMask.GetMask("Wongle");

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            bool _bWorkerCheck = false ;

            switch (m_bCtrlSelectUnits)
            {
                case false:
                    {

                        switch (m_bUserLClicked)
                        {
                            case true:
                                {
                                    // Get the type of unit selected...
                                    CheckUnitForType(ray, layermask);
                                  
                                    m_bUserLClicked = false;

                                    if (gameObject.GetComponent<cameraController>().m_bCamSelObjRotation)
                                    {
                                        gameObject.GetComponent<cameraController>().m_bCamSelObjRotation = false;
                                        gameObject.GetComponent<cameraController>().m_goRotateAround = null;
                                    }

                                    return;
                                }
                            case false:
                                {
                                    _bWorkerCheck = SelectRegular(ray, layermask, true);
                                    if (!_bWorkerCheck)
                                    {
                                        m_bUserLClicked = true;
                                        m_fUserClickedTime = Time.time + 0.3f;
                                    }
                                    else
                                    {
                                        if (!EventSystem.current.IsPointerOverGameObject())
                                        {
                                            if (m_goSelected != null)
                                            {
                                                if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == false)
                                                {
                                                    gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                                                }
                                                DisplayHandler.m_sDHControl.UpdateAndDisplayWongleWorkerText(m_goSelected);
                                            }
                                        }
                                    }
                                    UpdateCameraVariables(ray, layermask);
                                    break;
                                }
                        }


                        break;
                    }
                case true:
                    {
                        RaycastHit hit = GenerateRayCast(ray, layermask, true);
                        if (hit.transform.tag != null)
                        {
                            foreach (var unit in m_lCtrlUnits) // update all of the ctrl selected workers
                            {
                                if (unit.GetComponent<SelectableUnitComponent>().selectionCircle == null)
                                {
                                    unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
                                    unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
                                    unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);

                                    GameObject healthBarCanvasGameObject;
                                    healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                                    healthBarCanvasGameObject.SetActive(true);
                                }
                            }
                        }
                        else // deselection bug fix?
                        {
                            //if (EventSystem.current.IsPointerOverGameObject(m_iUserID) == false)
                            {
                                if (hit.transform.GetComponent<SelectableUnitComponent>().selectionCircle != null)
                                {
                                    Destroy(hit.transform.GetComponent<SelectableUnitComponent>().selectionCircle);
                                    hit.transform.GetComponent<SelectableUnitComponent>().selectionCircle = null;

                                    GameObject healthBarCanvasGameObject;
                                    healthBarCanvasGameObject = hit.transform.Find("Health Bar").gameObject;
                                    healthBarCanvasGameObject.SetActive(false);

                                    if ( hit.transform.gameObject.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                                    {
                                        //if (DisplayHandler.m_sDHControl.m_bDisplayingText)
                                        DisplayHandler.m_sDHControl.ResetState(true);
                                    }
                                }
                            }
                            print("maybe do deslection here");
                        }

                     
                        print("Controlled units size: " + m_lCtrlUnits.Count);
                            break;
                    }
            }

            /***
            * 
            * Handle for UI Display clicking shit
            * 
            ***/
            if (!EventSystem.current.IsPointerOverGameObject() && !m_bPlayerSelected)
            {
                if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == true && m_goSelected == null)
                {
                    gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                }
            }
            //   if (m_bPlayerSelected && !isSelecting)
            if (!CheckForSelectedCircle() && m_bPlayerSelected
                           && _bWorkerCheck) // worker check will equal true if worker
            {

                DisplayHandler.m_sDHControl.ResetState(true);
                m_bPlayerSelected = false;
            }
        }


        /***
         * 
         * 
         * Started to encounter a bug with the following bit of code. Something's delteing placementhandler or something - the home wasn't being destroyed...
         * 
         * 
         ***/
        if (PlacementHandler.m_sPHControl != null)
        {
            if (PlacementHandler.m_sPHControl.m_ePlayerState == PlacementHandler.PlayerStates.PLACING && isSelecting)
                isSelecting = false;
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<PlacementHandler>().m_ePlayerState == PlacementHandler.PlayerStates.PLACING && isSelecting)
                isSelecting = false;
        }


        if (m_bCtrlSelectUnits)
        {
            foreach (var unit in m_lCtrlUnits)
            {
                if (unit.GetComponent<SelectableUnitComponent>().selectionCircle == null)
                {
                    unit.GetComponent<SelectableUnitComponent>().selectionCircle = Instantiate(selectionCirclePrefab);
                    unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.SetParent(unit.transform, false);
                    unit.GetComponent<SelectableUnitComponent>().selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);

                    GameObject healthBarCanvasGameObject;
                    healthBarCanvasGameObject = unit.transform.Find("Health Bar").gameObject;
                    healthBarCanvasGameObject.SetActive(true);
                }
            }
        }


        // Highlight all objects within the selection box
        if (isSelecting)
         {
            //bool _bResetState = false;
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    if (selectableObject.selectionCircle == null)
                    {
                        selectableObject.selectionCircle = Instantiate(selectionCirclePrefab);
                        selectableObject.selectionCircle.transform.SetParent(selectableObject.transform, false);
                        selectableObject.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);

                        GameObject healthBarCanvasGameObject;
                        healthBarCanvasGameObject = selectableObject.transform.Find("Health Bar").gameObject;
                        healthBarCanvasGameObject.SetActive(true);

                        if (selectableObject.Type == SelectableUnitComponent.workerType.Worker)
                        {
                            if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == false && !m_bPlayerSelected)
                            {
                                gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                                m_bPlayerSelected = true;
                                DisplayHandler.m_sDHControl.UpdateAndDisplayWongleWorkerText(selectableObject.gameObject); // remove this if we create a total/average function as per below
                            }


                          
                        }
                        //AssignUIObjectsToSelected(selectableObject.gameObject);
                    }

                   selectableObject.isSelected = true;

                }
                else
                {
                    selectableObject.isSelected = false;
                    if (selectableObject.selectionCircle != null)
                    {
                        Destroy(selectableObject.selectionCircle.gameObject);
                        selectableObject.selectionCircle = null;

                        GameObject healthBarCanvasGameObject;
                        healthBarCanvasGameObject = selectableObject.transform.Find("Health Bar").gameObject;
                        healthBarCanvasGameObject.SetActive(false);
                        if (!CheckForSelectedCircle() && m_bPlayerSelected
                            && selectableObject.GetComponent<WongleController>().type == SelectableUnitComponent.workerType.Worker)
                        {
                          
                            DisplayHandler.m_sDHControl.ResetState(true);
                            m_bPlayerSelected = false;
                        }
                    }
                }
            }
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting && PlacementHandler.m_sPHControl.m_ePlayerState != PlacementHandler.PlayerStates.PLACING)
            return false;

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }


}
