using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    bool isSelecting = false;
    bool m_bCtrlSelectUnits = false;
    public List <GameObject> m_lCtrlUnits;
    Vector3 mousePosition1;
    public Camera camera;
    public GameObject[] m_goSelectOBJ; // 0 = DEFAULT, 1 = NPC, 2 = RESOURCE
    public GameObject selectionCirclePrefab;

    private int m_iUserID = -1;
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
    private bool[] m_bUnitsSelected;

    private void Awake()
    {
        #if !UNITY_EDITOR
            m_iUserID = 0;
        #endif
    }

    private void Start()
    {
        m_goMeleeUnits = new List<GameObject>();
        m_goRangedUnits = new List<GameObject>();
        m_bUnitsSelected = new bool[2];

        // bool system cheaper on performance then rechecking through an auto loop...
        m_bUnitsSelected[0] = false; // Melee units
        m_bUnitsSelected[1] = false; // Ranged Units


        GameObject[] _goUnits;
        _goUnits = GameObject.FindGameObjectsWithTag("Wongle");
        //int _iTotalWongles = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().iCurrentWongleCount;
        int _iTotalWongles = 5;
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
    private void CheckUnitForType(Ray ray, int layermask)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layermask))
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
                                SelectRegular(ray, layermask, false);
                                break;
                            }
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
                            print("deselect");
                            _goUpdateList.Add(_npc);
                            m_lCtrlUnits.RemoveAt(_iBundyCount);
                            _iBundyCount++;
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
                }

                unit.isSelected = true;
            }

        }
        return hit;
    }


    private void RemoveSelectionCircleFromObjects()
    {
        foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
        {
            selectableObject.isSelected = false;
            if (selectableObject.selectionCircle != null)
            {
                Destroy(selectableObject.selectionCircle.gameObject);
                selectableObject.selectionCircle = null;
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
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }

    }

    void Update()
    {
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
            if (Physics.Raycast(ray, out hit, 1000))
            {
                _sOriginalHitResult = hit.transform.gameObject.tag;
            }
                Physics.Raycast(ray, out hit, 1000, layermask);
                switch(_sOriginalHitResult)
                {
                    case "Wood":
                        {
                            Instantiate(m_goSelectOBJ[2], hit.point, camera.transform.rotation);
                            break;
                        }
                    case "Enemy":
                        {
                            Instantiate(m_goSelectOBJ[1], hit.point, camera.transform.rotation);
                            break;
                        }
                    default:
                        {
                            Instantiate(m_goSelectOBJ[0], hit.point, camera.transform.rotation);
                            break;
                        }
                }
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (!m_bUserLClicked)
            {
                switch (m_bCtrlSelectUnits)
                {
                    case false:
                        {
                            if (EventSystem.current.IsPointerOverGameObject(m_iUserID) == false)
                            {
                                if (m_lCtrlUnits.Count > 0)
                                {
                                    foreach (var unit in m_lCtrlUnits)
                                    {
                                        if (unit.GetComponent<SelectableUnitComponent>().selectionCircle != null)
                                        {
                                            Destroy(unit.GetComponent<SelectableUnitComponent>().selectionCircle.gameObject);
                                            unit.GetComponent<SelectableUnitComponent>().selectionCircle = null;
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
                                        Destroy(selectableObject.selectionCircle.gameObject);
                                        selectableObject.selectionCircle = null;
                                    }
                                }
                            }
                            break;
                        }
                    case true:
                        {
                            if (EventSystem.current.IsPointerOverGameObject(m_iUserID) == false)
                            {
                                mousePosition1 = Input.mousePosition;

                                foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
                                {
                                    if (selectableObject.selectionCircle != null && !m_bCtrlSelectUnits)
                                    {
                                        Destroy(selectableObject.selectionCircle.gameObject);
                                        selectableObject.selectionCircle = null;
                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            int layermask = LayerMask.GetMask("Wongle");

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);



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
                                    return;
                                }
                            case false:
                                {
                                    bool _bWorkerCheck = SelectRegular(ray, layermask, true);
                                    if (!_bWorkerCheck)
                                    {
                                        m_bUserLClicked = true;
                                        m_fUserClickedTime = Time.time + 1.0f;
                                    }
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
                                }
                            }
                        }
                        else // deselection bug fix?
                        {
                            if (EventSystem.current.IsPointerOverGameObject(m_iUserID) == false)
                            {
                                if (hit.transform.GetComponent<SelectableUnitComponent>().selectionCircle != null)
                                {
                                    Destroy(hit.transform.GetComponent<SelectableUnitComponent>().selectionCircle);
                                    hit.transform.GetComponent<SelectableUnitComponent>().selectionCircle = null;
                                }
                            }
                            print("maybe do deslection here");
                        }

                     
                        print("Controlled units size: " + m_lCtrlUnits.Count);
                            break;
                    }
            }

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
                }
            }
        }


        // Highlight all objects within the selection box
        if (isSelecting)
        {
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    if (selectableObject.selectionCircle == null)
                    {
                        selectableObject.selectionCircle = Instantiate(selectionCirclePrefab);
                        selectableObject.selectionCircle.transform.SetParent(selectableObject.transform, false);
                        selectableObject.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
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
                    }
                }
            }
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
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
