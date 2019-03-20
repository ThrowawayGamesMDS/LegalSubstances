using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/***
 * 
 * Created by Benjamin Pointer
 *  Uses SelectionBox.cs
 * 
 ***/

public class DisplayHandler : MonoBehaviour
{
    public int m_iTotalNumber;
    public SelectableObject select;
    public bool m_bDisplayingUnit;
    public bool m_bDisplayingBuildings;

    private List<GameObject> m_goUIObj;

    private void Awake()
    {
        m_goUIObj = new List<GameObject>();
        m_goUIObj.Add(GameObject.FindGameObjectWithTag("UnitButtonPanel"));
        m_goUIObj.Add(GameObject.FindGameObjectWithTag("BuildButtonPanel"));
        for (int i = 0; i < m_goUIObj.Count; i++)
        {
            if (m_goUIObj[i] != null)
            {
                m_goUIObj[i].SetActive(false);
            }
        }
        m_bDisplayingUnit = false;
        m_bDisplayingBuildings = false;
    }

    private bool GenerateRayCast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            print(hit.transform.name);
            if (hit.transform.gameObject.tag == "HomeBuilding" && !m_bDisplayingUnit)
            {
                print("set build options");
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateState(false);
                    return true;
                }
                
            }
            else if (hit.transform.gameObject.tag != "HomeBuilding" && m_bDisplayingUnit)
            {
                print("reset build options");
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateState(false);
                    return true;
                }
            }
        }
        return false;
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
                                m_goUIObj[1].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                                break;
                            }
                        case false:
                            {
                                if (m_bDisplayingUnit)
                                {
                                    m_bDisplayingUnit = false;
                                    m_goUIObj[0].SetActive(false); 
                                }
                                m_bDisplayingBuildings = true;
                                m_goUIObj[1].SetActive(true); // m_goUiOBJ[1] = BuildOptions 
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
                                m_goUIObj[0].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                                print("reset unit display");
                                break;
                            }
                        case false:
                            {
                                if (m_bDisplayingBuildings)
                                {
                                    m_bDisplayingBuildings = false;
                                    m_goUIObj[1].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                                }
                                m_bDisplayingUnit = true;
                                m_goUIObj[0].SetActive(true); // m_goUiOBJ[1] = BuildOptions 
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
            /*if ( !GenerateRayCast(ray))
            {
                if (gameObject.GetComponent<SelectionBox>().m_goSelectOBJ != null && !m_bDisplayingBuildings)
                {
                    if (m_bDisplayingUnit)
                    {
                        m_bDisplayingUnit = false;
                        m_goUIObj[0].SetActive(false);
                    }
                    UpdateState(true);
                }
            }*/

        }
    }
}
