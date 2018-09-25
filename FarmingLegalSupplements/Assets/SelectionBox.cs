using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject selectionCirclePrefab;

    private RaycastHit GenerateRayCast(Ray ray, int layermask, bool _bCtrlSelect)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layermask))
        {

            if (hit.transform.gameObject.GetComponent<SelectableUnitComponent>() != null)
            {
                SelectableUnitComponent unit = hit.transform.gameObject.GetComponent<SelectableUnitComponent>();
                if (_bCtrlSelect)
                {
                    foreach (var _npc in m_lCtrlUnits)
                    {
                        if (hit.transform.gameObject == _npc)
                        {
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
                /*if (unit.selectionCircle == null && _bCtrlSelect)
                {
                    unit.selectionCircle = Instantiate(selectionCirclePrefab);
                    unit.selectionCircle.transform.SetParent(unit.transform, false);
                    unit.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
                }*/

                unit.isSelected = true;
            }

        }
        return hit;
    }

    void Update()
    {
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            m_bCtrlSelectUnits = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            m_bCtrlSelectUnits = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch(m_bCtrlSelectUnits)
            {
                case false:
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
                        break;
                    }
                case true:
                    {
                        mousePosition1 = Input.mousePosition;

                        foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
                        {
                            if (selectableObject.selectionCircle != null)
                            {
                                Destroy(selectableObject.selectionCircle.gameObject);
                                selectableObject.selectionCircle = null;
                            }
                        }
                        break;
                    }
            }
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            switch(m_bCtrlSelectUnits)
            {
                case false:
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


                        int layermask = LayerMask.GetMask("Wongle");
                        
                        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit = GenerateRayCast(ray, layermask, false);
                        
                        break;
                    }
                case true:
                    {
                        int layermask = LayerMask.GetMask("Wongle");

                        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit = GenerateRayCast(ray, layermask, true);
                        if (hit.transform.tag != null)
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
