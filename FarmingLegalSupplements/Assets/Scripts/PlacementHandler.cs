﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHandler : MonoBehaviour
{
    [Tooltip("Add all possible objects to this array - add them linearly to the following to arrays as well")]
    public GameObject[] m_goPossibleObjects;
    [Tooltip("Possible objs displaying a transparent green mesh")]
    public GameObject[] m_goObjPlacementOk;
    [Tooltip("Possible objs displaying a transparent red mesh")]
    public GameObject[] m_goObjPlacementBad;
    public GameObject m_goPlacementDefault;
    public GameObject m_goCurrentlyPlacing;
    public GameObject[] m_goParticleEffects; // 0 = building
    public GameObject m_goSuccessfulBuild;
    public List<GameObject> m_goObjsPlaced;
    public int m_iObjsPlaced;
    public int m_iMaxObjsPlaceable;
    public int m_iCurrentlyPlacing;
    private Vector2 m_vec2MouseCoords;
    private bool m_bBadPlacement;
    public enum PlayerStates
    {
        PLACING, DEFAULT
    }
    public PlayerStates m_ePlayerState;

    void Start()
    {
        m_vec2MouseCoords = Input.mousePosition;
        m_iCurrentlyPlacing = 0;
        m_goCurrentlyPlacing = m_goPossibleObjects[0];
        m_ePlayerState = PlayerStates.DEFAULT;
        m_iMaxObjsPlaceable = 50;
        m_goPlacementDefault = m_goObjPlacementOk[m_iCurrentlyPlacing];
        m_bBadPlacement = false;
    }


   /* void Pickup(int amount)
    {
        PublicStats.g_fResourceCount += amount;
        print(PublicStats.g_fResourceCount);
    }*/


    RaycastHit GenerateRayCast(float _fDistanceOfRay)
    {
        RaycastHit _rh;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * _fDistanceOfRay, new Color(1f, 0.922f, 0.016f, 1f));
        int _iLayerMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray.origin, ray.direction * _fDistanceOfRay, out _rh, 250.0f, _iLayerMask))
        {
            return _rh;
        }
        else
        {
            return _rh;
        }
    }
    
    private bool PlacementUnacceptable(Vector3 _vec3DesiredPos)
    {
        bool _bObjExists = false;
        if (m_goObjsPlaced.Count != 0)
        {
            for (int i = 0; i < m_goObjsPlaced.Count; i++)
            {
                if(_vec3DesiredPos.x > m_goObjsPlaced[i].transform.position.x - 9.8f && _vec3DesiredPos.x < m_goObjsPlaced[i].transform.position.x + 9.8f)
                {
                    if(_vec3DesiredPos.z > m_goObjsPlaced[i].transform.position.z - 9.8f && _vec3DesiredPos.z < m_goObjsPlaced[i].transform.position.z + 9.8f)
                    {
                        _bObjExists = true;
                    }
                }
            }
        }
        else
        {
            return false;
        }
      
        return _bObjExists;
    }
    
    private void PlaceAnObject()
    {
        RaycastHit _rhCheck = GenerateRayCast(Camera.main.transform.position.y * 2);
        Vector3 pos = _rhCheck.point;
        pos = new Vector3(Mathf.Round(pos.x/10)*10, pos.y, Mathf.Round(pos.z / 10) * 10);
        if (!PlacementUnacceptable(pos))
        {
            if(HouseController.CashAmount >= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().Cost)
            {
                HouseController.CashAmount -= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().Cost;
                //m_goSuccessfulBuild = Instantiate(m_goParticleEffects[0], m_vec3Pos, m_goParticleEffects[0].transform.rotation) as GameObject;
                m_goObjsPlaced.Add(Instantiate(m_goPossibleObjects[m_iCurrentlyPlacing], pos, Quaternion.identity));
                m_goObjsPlaced[m_goObjsPlaced.Count - 1].transform.rotation = m_goPlacementDefault.transform.rotation;
            }
        }
    }

   private void CheckIfPlacementIsOkay()
    {
        RaycastHit _rhCheck;
        _rhCheck = GenerateRayCast(Camera.main.transform.position.y * 2);
        Vector3 pos = _rhCheck.point;
        pos.y = 0;
        pos = new Vector3(Mathf.Round(pos.x / 10) * 10, pos.y, Mathf.Round(pos.z / 10) * 10);
        
        if (m_ePlayerState == PlayerStates.DEFAULT)
        {
            m_goPlacementDefault = Instantiate(m_goObjPlacementOk[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject;
        }

        else
        {
                if (!PlacementUnacceptable(pos))
                {
                    if (m_bBadPlacement)
                    {
                        Destroy(m_goPlacementDefault);
                        m_goPlacementDefault = Instantiate(m_goObjPlacementOk[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject;
                        m_bBadPlacement = false;
                    }
                    else
                    {
                        m_goPlacementDefault.transform.position = pos;
                    }
                    // accepted
                }
                else
                {
                    if (!m_bBadPlacement)
                    {
                        Destroy(m_goPlacementDefault);
                        m_goPlacementDefault = Instantiate(m_goObjPlacementBad[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject;
                        m_bBadPlacement = true;
                    }
                    else
                    {
                        m_goPlacementDefault.transform.position = pos;
                    }
                    // unaccepted
                }

          
        }

    }
    
    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < m_goObjsPlaced.Count; i++)
        {
            if(m_goObjsPlaced[i] == null)
            {
                m_goObjsPlaced.RemoveAt(i);
            }
        }


        if (Input.GetKeyUp(KeyCode.F))
        {

            switch (m_ePlayerState)
            {
                case PlayerStates.PLACING:
                    Destroy(m_goPlacementDefault);
                    //Destroy(m_goBuilderUI3D);
                    m_ePlayerState = PlayerStates.DEFAULT;
                    break;
                case PlayerStates.DEFAULT:
                    CheckIfPlacementIsOkay();
                    m_vec2MouseCoords = Input.mousePosition;
                    m_ePlayerState = PlayerStates.PLACING;
                    break;
            }

        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_iCurrentlyPlacing = 0;
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            m_iCurrentlyPlacing = 1;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Break();
        }

        if (m_ePlayerState == PlayerStates.PLACING)
        {
            if (m_vec2MouseCoords.x != Input.mousePosition.x && m_vec2MouseCoords.y != Input.mousePosition.y)
            {
                CheckIfPlacementIsOkay();
                m_vec2MouseCoords = Input.mousePosition;
            }
            //CheckIfPlacementIsOkay();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            print("player trying to left click");
            switch (m_ePlayerState)
            {
                case PlayerStates.DEFAULT:
                    {
                        break;
                    }
                case PlayerStates.PLACING:
                    {
                        PlaceAnObject();
                        break;
                    }
            }
        }
    }
}