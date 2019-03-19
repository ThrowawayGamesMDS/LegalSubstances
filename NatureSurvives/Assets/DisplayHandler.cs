using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * 
 * Created by Benjamin Pointer
 *  Uses SelectionBox.cs
 * 
 ***/

public class DisplayHandler : MonoBehaviour
{
    public GameObject[] m_goUiOBJ;
    public int m_iTotalNumber;
    public SelectableObject select;
    public bool m_bDisplayingUnit;
    public bool m_bDisplayingBuildings;

    private void Awake()
    {
        for (int i = 0; i < m_iTotalNumber; i++)
        {
            if (m_goUiOBJ[i].GetComponent<CanvasGroup>() != null)
            {
                m_goUiOBJ[i].GetComponent<CanvasGroup>().alpha = 0.0f;
            }
        }
        m_bDisplayingUnit = false;
        m_bDisplayingBuildings = false;
    }

    private RaycastHit GenerateRayCast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            print(hit.transform.name);
            if (hit.transform.gameObject.tag == "HomeBuilding" && !m_bDisplayingUnit)
            {
                print("set build options");
                UpdateState(false);
            }
            else if(hit.transform.gameObject.tag != "HomeBuilding" && m_bDisplayingUnit)
            {
                print("reset build options");
                UpdateState(false);
            }
        }
        return hit;
    }

    public void UpdateState(bool _bWorker)
    {


        switch (_bWorker)
        {
            case true:
                {
                    switch(m_bDisplayingBuildings)
                    {
                        case true:
                            {
                                m_bDisplayingBuildings = false;
                                m_goUiOBJ[1].GetComponent<CanvasGroup>().alpha = 0.0f; // m_goUiOBJ[1] = BuildOptions 
                                break;
                            }
                        case false:
                            {
                                m_bDisplayingBuildings = true;
                                m_goUiOBJ[1].GetComponent<CanvasGroup>().alpha = 1.0f; // m_goUiOBJ[1] = BuildOptions 
                                break;
                            }
                    }
                    break;
                }
            case false:
                {
                    switch (m_bDisplayingUnit)
                    {
                        case true:
                            {
                                m_bDisplayingUnit = false;
                                m_goUiOBJ[0].GetComponent<CanvasGroup>().alpha = 0.0f; // m_goUiOBJ[1] = BuildOptions 
                                print("reset unit display");
                                break;
                            }
                        case false:
                            {
                                m_bDisplayingUnit = true;
                                m_goUiOBJ[0].GetComponent<CanvasGroup>().alpha = 1.0f; // m_goUiOBJ[1] = BuildOptions 
                                print("set unit display");
                                break;
                            }
                    }
                    break;
                }
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //  int layermask = LayerMask.NameToLayer("");
            GenerateRayCast(ray);
           
        }
    }
}
