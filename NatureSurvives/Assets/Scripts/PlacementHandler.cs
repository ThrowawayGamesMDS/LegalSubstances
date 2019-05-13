using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHandler : MonoBehaviour
{
    public static PlacementHandler m_sPHControl;
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
    public GameObject[] m_goBarricadeWallObj;
    public List<GameObject> m_goObjsPlaced;
    public List<GameObject> m_goBarricadePlacements;
    public int m_iObjsPlaced;
    public int m_iMaxObjsPlaceable;
    public int m_iCurrentlyPlacing;
    private Vector2 m_vec2MouseCoords;
    private bool m_bBadPlacement;
    private bool m_bRefreshBuild;

    public enum m_ePlayerSelected
    {
        DEFAULT,HOME, GREEN_FARM, WHITE_FARM, CONTRACT_KILLER
    }
    public m_ePlayerSelected m_eDisplayUi;

    public enum m_ePlayerBuilding // 
    {
       DEFAULT = 5, P_MID_WALL = 1 ,P_WALL_START = 0, P_WALL_FINISH = 2, P_WALL_DOOR = 3
    }
    public m_ePlayerBuilding m_ePlayerIsBuilding;

    public enum PlayerStates
    {
        PLACING, DEFAULT
    }

    public PlayerStates m_ePlayerState;

    void Start()
    {
        if (m_sPHControl == null)
        {
            m_sPHControl = this;
            m_vec2MouseCoords = Input.mousePosition;
            m_iCurrentlyPlacing = 0;
            m_goCurrentlyPlacing = m_goPossibleObjects[0];
            m_ePlayerState = PlayerStates.DEFAULT;
            m_iMaxObjsPlaceable = 50;
            m_goPlacementDefault = m_goObjPlacementOk[m_iCurrentlyPlacing];
            m_bBadPlacement = false;
            m_eDisplayUi = m_ePlayerSelected.DEFAULT;
            m_ePlayerIsBuilding = m_ePlayerBuilding.DEFAULT;
            m_bRefreshBuild = false;
        }
    }


   /* void Pickup(int amount)
    {
        PublicStats.g_fResourceCount += amount;
        print(PublicStats.g_fResourceCount);
    }*/

    private void RefreshBuilder()
    {
        m_bRefreshBuild = false;
    }

    private void SelectiveDestroy(GameObject _destroy)
    {
        Destroy(_destroy);
    }


    RaycastHit GenerateRayCast(float _fDistanceOfRay, bool _bUseLayermask)
    {
        RaycastHit _rh;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * _fDistanceOfRay, new Color(1f, 0.922f, 0.016f, 1f));
        int _iLayerMask;
        if (_bUseLayermask)
        {
            _iLayerMask = LayerMask.GetMask("GridSquare","Ground");
            if (Physics.Raycast(ray.origin, ray.direction * _fDistanceOfRay, out _rh, 250.0f, _iLayerMask))
            {
                return _rh;
            }
            else
            {
                return _rh;
            }
        }
        else
        {
            if (Physics.Raycast(ray.origin, ray.direction * _fDistanceOfRay, out _rh, 250.0f))
            {
               return _rh;
            }
            else
            {
                return _rh;
            }
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

    IEnumerator DestroyParticle(GameObject _destroy, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(_destroy);
    }


    //private void PlaceHandle(bool _bBarricade, int _iObj, Vector3 _vec3Pos) // iObj == object to spawn within array
    private void PlaceHandle(bool _bBarricade, Vector3 _vec3Pos)
    {
        m_goSuccessfulBuild = Instantiate(m_goParticleEffects[0], _vec3Pos, m_goParticleEffects[0].transform.rotation) as GameObject;
        switch (_bBarricade)
        {
            case true:
                {
                    m_goBarricadePlacements.Add(Instantiate(m_goBarricadeWallObj[(int)m_ePlayerIsBuilding], _vec3Pos, Quaternion.identity));
                    if (m_goBarricadePlacements.Count != 0)
                    {
                        m_goBarricadePlacements[m_goBarricadePlacements.Count - 1].transform.rotation = m_goPlacementDefault.transform.rotation;
                    }
                    else
                    {
                        m_goBarricadePlacements[m_goBarricadePlacements.Count].transform.rotation = m_goPlacementDefault.transform.rotation;
                    }
                    Destroy(m_goPlacementDefault);

                    break;
                }
            case false:
                {
                    // Adjust player resources
                    HouseController.WhiteAmount -= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().FoodCost;
                    HouseController.WoodAmount -= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().WoodCost;
                    HouseController.CrystalAmount -= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().CrystalCost;
                    //Placement Occurs Here
                    m_goObjsPlaced.Add(Instantiate(m_goPossibleObjects[m_iCurrentlyPlacing], _vec3Pos, Quaternion.identity));

                    // Bundy Null fix
                    if (m_goObjsPlaced.Count != 0)
                    {
                        m_goObjsPlaced[m_goObjsPlaced.Count - 1].transform.rotation = m_goPlacementDefault.transform.rotation;
                    }
                    else
                    {
                        m_goObjsPlaced[m_goObjsPlaced.Count - 1].transform.rotation = m_goPlacementDefault.transform.rotation;
                    }

                    Destroy(m_goPlacementDefault);

                    if (m_goPossibleObjects[m_iCurrentlyPlacing].tag != "Construction")
                    {
                        NotificationManager.Instance.SetNewNotification("Building is built.");
                        GameObject m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
                        if (m_fogOfWar)
                        {
                            FogGenerator fogComponent = m_fogOfWar.transform.GetComponent<FogGenerator>();
                            float m_buildingRadius = m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().FogRadius;
                            fogComponent.Unfog(_vec3Pos, m_buildingRadius, m_goPossibleObjects[m_iCurrentlyPlacing].GetInstanceID());
                        }
                    }
                    
                    break;
                }
        }
        StartCoroutine(DestroyParticle(m_goSuccessfulBuild, 1.5f));
    }

    private void PlaceAnObject()
    {
        RaycastHit _rhCheck = GenerateRayCast(Camera.main.transform.position.y * 2, true);
        Vector3 pos = _rhCheck.point;
        if(!m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().isFlag)
        {
            pos = new Vector3(Mathf.Round(pos.x / 10) * 10, pos.y, Mathf.Round(pos.z / 10) * 10);
        }
        if (!PlacementUnacceptable(pos))
        {
            if (m_ePlayerIsBuilding != m_ePlayerBuilding.DEFAULT)
            {
                /***
                 * 
                 * 
                 * Need to remember the cost - adjust the possible amount to be placed and update the last with the wall_end obj.
                 * Do door placement post wall placement and replace adjacent pieces with the door
                 * 
                 * 
                 * TO DO: figure out a cost for each placement, create AI to adjust overall placement incl door to player's current cash val
                 *          if the total amount exceeds their resource amt
                 * 
                 ***/

                switch(m_ePlayerIsBuilding)
                {
                    case m_ePlayerBuilding.P_WALL_START:
                        {
                           
                            PlaceHandle(true, pos);
                            m_ePlayerIsBuilding = m_ePlayerBuilding.P_MID_WALL;
                            break;
                        }
                    case m_ePlayerBuilding.P_MID_WALL:
                        {
                            PlaceHandle(true, pos);
                            m_ePlayerIsBuilding = m_ePlayerBuilding.P_MID_WALL;
                            break;
                        }
                    case m_ePlayerBuilding.P_WALL_DOOR:

                        {
                            PlaceHandle(true, pos);
                            m_ePlayerIsBuilding = m_ePlayerBuilding.DEFAULT;
                            break;
                        }
                    case m_ePlayerBuilding.P_WALL_FINISH:
                        {
                            PlaceHandle(true, pos);
                            m_ePlayerIsBuilding = m_ePlayerBuilding.P_WALL_DOOR;
                            break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                // BASE PLACEMENT
                if (HouseController.WhiteAmount >= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().FoodCost)
                {
                    if (HouseController.WoodAmount >= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().WoodCost)
                    {
                        if (HouseController.CrystalAmount >= m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().CrystalCost)
                        {
                            if (_rhCheck.transform.tag != "CorruptedGrid")
                            {
                                PlaceHandle(false, pos);
                                m_ePlayerState = PlayerStates.DEFAULT;
                            }
                            else
                            {
                                // gameObject.GetComponent<AudioHandler>().PlaySound(AudioHandler.m_soundTypes.FAILURE);
                                NotificationManager.Instance.SetNewNotification("corrupted grid");
                            }
                        }
                        else
                        {
                            NotificationManager.Instance.SetNewNotification("Can't place, not enough crystal");
                        }
                    }
                    else
                    {
                        NotificationManager.Instance.SetNewNotification("Can't place, not enough wood");
                    }
                }
                else
                {
                    NotificationManager.Instance.SetNewNotification("Can't place, not enough food");
                }
            }

            
        }
    }

    private void DetermineIfSuccesfulClick() // Users click hit an obj
    {
        RaycastHit _rhCheck = GenerateRayCast(Camera.main.transform.position.y * 2, false);
        //Destroy(_rhCheck.transform.gameObject);

        switch(_rhCheck.transform.tag)
        {
            case "Home":
                {
                    m_eDisplayUi = m_ePlayerSelected.HOME;
                    break;
                }
            case "green_farm":
                {
                    m_eDisplayUi = m_ePlayerSelected.GREEN_FARM;
                    break;
                }
            case "white_farm":
                {
                    m_eDisplayUi = m_ePlayerSelected.WHITE_FARM;
                    break;
                }

            default:
                break;
        }

    }

    private void CheckIfPlacementIsOkay()
    {
        RaycastHit _rhCheck;
        _rhCheck = GenerateRayCast(Camera.main.transform.position.y * 2, true);
        Vector3 pos = _rhCheck.point;
        pos.y = 0;
        if (!m_goPossibleObjects[m_iCurrentlyPlacing].GetComponent<costToPlace>().isFlag)
        {
            pos = new Vector3(Mathf.Round(pos.x / 10) * 10, pos.y, Mathf.Round(pos.z / 10) * 10);
        }
        if (m_ePlayerState == PlayerStates.DEFAULT)
        {
            if (_rhCheck.transform.tag != "CorruptedGrid")
            {
                m_goPlacementDefault = Instantiate(m_goObjPlacementOk[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject;
            }
            else
            {
                m_goPlacementDefault = Instantiate(m_goObjPlacementBad[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject;
            }
        }
        else
        {
            if (_rhCheck.transform.tag == "CorruptedGrid")
            {
                if (m_goPlacementDefault != null)
                {
                    Destroy(m_goPlacementDefault);
                }
                m_goPlacementDefault = Instantiate(m_goObjPlacementBad[m_iCurrentlyPlacing], pos, Quaternion.identity) as GameObject;
                m_bBadPlacement = true;

                return;
            }
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

    private bool CanPurchase(int _iPurchasing)
    {
        bool _bAccepted = false;

        if (HouseController.WhiteAmount >= m_goPossibleObjects[_iPurchasing].GetComponent<costToPlace>().FoodCost)
        {
            if (HouseController.WoodAmount >= m_goPossibleObjects[_iPurchasing].GetComponent<costToPlace>().WoodCost)
            {
                if (HouseController.CrystalAmount >= m_goPossibleObjects[_iPurchasing].GetComponent<costToPlace>().CrystalCost)
                {
                    _bAccepted = true;
                }
            }
        }
        
        return _bAccepted;
    }
    
    public void BuildButton(int i)
    {
        if (!pause.m_sPauseHandle.m_bPlayerPaused)
        {
            if (CanPurchase(i))
            {
                m_iCurrentlyPlacing = i;
                //gameObject.GetComponent<AudioHandler>().PlaySound("PurchaseOk");

                m_bRefreshBuild = true;
                Invoke("RefreshBuilder", 0.1f);

                CheckIfPlacementIsOkay();
                m_vec2MouseCoords = Input.mousePosition;
                m_ePlayerState = PlayerStates.PLACING;
            }
            else
            {
                // gameObject.GetComponent<AudioHandler>().PlaySound("PurchaseBad");
                NotificationManager.Instance.SetNewNotification("You cannot purchase. Check your resources");
            }
        }
       else
        {
            NotificationManager.Instance.SetNewNotification("You cannot place buildings whilst paused!");
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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_ePlayerState != PlayerStates.DEFAULT)
            {
                Destroy(m_goPlacementDefault);
                m_ePlayerState = PlayerStates.DEFAULT;
            }
            
        }
        

        if (m_ePlayerState == PlayerStates.PLACING)
        {
            if (m_vec2MouseCoords.x != Input.mousePosition.x && m_vec2MouseCoords.y != Input.mousePosition.y)
            {
                CheckIfPlacementIsOkay();
                m_vec2MouseCoords = Input.mousePosition;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            switch (m_ePlayerState)
            {
                case PlayerStates.DEFAULT:
                    {
                        // click to select obj
                        //raycast out -> get obj type (maybe make a class to return type...) -> update UI to display possible options for selected item... :D
                        //DetermineIfSuccesfulClick();
                        break;
                    }
                case PlayerStates.PLACING:
                    {
                        if (!m_bRefreshBuild)
                        {
                            PlaceAnObject();
                        }
                        break;
                    }
            }
        }
    }
}