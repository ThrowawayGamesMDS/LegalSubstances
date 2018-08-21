using System.Collections;
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
    public GameObject[] m_goObjsPlaced;

    public List<GameObject> m_lPlacedObjects;
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
        m_iObjsPlaced = 0;
        m_iCurrentlyPlacing = 0;
        m_goCurrentlyPlacing = m_goPossibleObjects[0];
        m_ePlayerState = PlayerStates.DEFAULT;
        m_iMaxObjsPlaceable = 50;
        m_goObjsPlaced = new GameObject[m_iMaxObjsPlaceable];
        m_goPlacementDefault = m_goObjPlacementOk[m_iCurrentlyPlacing];
        m_bBadPlacement = false;
    }
    

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
        if (m_iObjsPlaced != 0)
        {
            for (int i = 0; i < m_iObjsPlaced; i++)
            {
                if (Vector3.Distance(_vec3DesiredPos, m_lPlacedObjects[i].transform.position) <= 9.8f)
                {
                    // a turret already exists in the desired position
                    _bObjExists = true;
                }
                if (m_lPlacedObjects[i].transform.position.x - 4 < _vec3DesiredPos.x && _vec3DesiredPos.x < m_lPlacedObjects[i].transform.position.x + 4)
                {
                    _bObjExists = true;
                }
                if (m_lPlacedObjects[i].transform.position.z - 4 < _vec3DesiredPos.z && _vec3DesiredPos.z < m_lPlacedObjects[i].transform.position.z + 4)
                {
                    _bObjExists = true;
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
        if (!PlacementUnacceptable(pos))
        {
            m_lPlacedObjects.Add(Instantiate(m_goPossibleObjects[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject);
            m_lPlacedObjects[m_iObjsPlaced].transform.rotation = m_goPlacementDefault.transform.rotation;
            m_iObjsPlaced += 1;
        }
    }

   private void CheckIfPlacementIsOkay()
    {
        RaycastHit _rhCheck;
        _rhCheck = GenerateRayCast(Camera.main.transform.position.y * 2);
        Vector3 pos = _rhCheck.point;
        pos.y = 0;
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
                }

          
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {

            switch (m_ePlayerState)
            {
                case PlayerStates.PLACING:
                    Destroy(m_goPlacementDefault);
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