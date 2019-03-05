using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/***
 * 
 * 
 * Could spawn new items by handling the amount currently active...
 * 
 * 
 * 
 ***/

public class ResourceSpawner : MonoBehaviour
{
    [Tooltip("The number of separate resources to be spawned")]
    public int m_iResourceCount;
    [Tooltip("The resource - Enter the following values linearly and in accordance with m_goResources")]
    public GameObject[] m_goResources;
    [Tooltip("The total amount of spawns per resource count NOTE: Spawn Quantity w/UsingDensity == true = Spawn Quantity * Density Quantity")]
    public int[] m_iSpawnQuantity;
    [Tooltip("How far apart the resource(s) should spawn")]
    public int[] m_iSpawnDensity;
    [Tooltip("The quantity of objects to spawn within the current densiy level")]
    public int[] m_iDensityQuantity;
    [Tooltip("Whether or not you're using the de")]
    public bool m_bUsingDensity; //Could use an array for this and set by object.. cause maybe some objs might just be random and not density defined
    private GameObject m_goGround;


    private List<GameObject> m_goResourcesSpawned; // could split up into two lists for better management 
    private bool m_bResourcesSpwaned;
    private float[] m_fGroundRegion;
    private Vector3 m_vec3CenterRegion;
    private GameObject[] m_goRegionalObjects;
    private int m_iRegionalObjectCount;
    private List<GameObject> m_lRegionalObjs;

    // Start is called before the first frame update
    void Start()
    {
        m_bResourcesSpwaned = false;

        m_goGround = GameObject.FindGameObjectWithTag("Ground");
        m_fGroundRegion = new float[2];
        m_fGroundRegion[0] = m_goGround.GetComponent<Renderer>().bounds.size.x/2;
        m_fGroundRegion[1] = m_goGround.GetComponent<Renderer>().bounds.size.z/2;
        m_vec3CenterRegion = m_goGround.GetComponent<Renderer>().bounds.center;
        
        
        GameObject[] _treePotential = GameObject.FindGameObjectsWithTag("Wood");
        GameObject[] _miscPotential = GameObject.FindGameObjectsWithTag("Misc");
        m_iRegionalObjectCount = GameObject.FindGameObjectsWithTag("Wood").Length + GameObject.FindGameObjectsWithTag("Misc").Length;
        m_goRegionalObjects = new GameObject[m_iRegionalObjectCount];
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Wood").Length; i++)
        {
            m_goRegionalObjects[i] = _treePotential[i];
        }
        for (int j = GameObject.FindGameObjectsWithTag("Wood").Length; j < GameObject.FindGameObjectsWithTag("Misc").Length; j++)
        {
            m_goRegionalObjects[j] = _treePotential[j - GameObject.FindGameObjectsWithTag("Wood").Length];
        }
        m_goResourcesSpawned = new List<GameObject>();
    }


    private Vector3 CreateNewPosition(bool _bUsingDensity ,Vector3 _bNearByPos) // have to double check bool because of firstspawn..
    {
        float _iRandXRange;

        float _iRandZRange;

        Vector3 _newPos;
        
        switch(_bUsingDensity)
        {
            case false:
                {
                    for (int i = 0; i < 30; i++)
                    {
                        _iRandXRange = Random.Range(m_fGroundRegion[0] * -1.0f, m_fGroundRegion[0]);
                        _iRandZRange = Random.Range(m_fGroundRegion[1] * -1.0f, m_fGroundRegion[1]);
                        _newPos = new Vector3(_iRandXRange, 0.31f, _iRandZRange);
                        if (CheckObjectDistance(_newPos, GameObject.FindGameObjectWithTag("HomeBuilding").transform.position) > 70)
                        {
                            return _newPos;
                        }
                    }
                    break;
                }
            case true:
                {
                    for (int i = 0; i < 30; i++)
                    {
                        _iRandXRange = Random.Range(_bNearByPos.x - 15.0f, _bNearByPos.x + 15.0f); // 15 is default, the range is 15
                        _iRandZRange = Random.Range(_bNearByPos.z - 15.0f, _bNearByPos.z + 15.0f);
                        _newPos = new Vector3(_iRandXRange, 0.31f, _iRandZRange);
                        if (CheckObjectDistance(_newPos, _bNearByPos) > 5)
                        {
                            return _newPos;
                        }
                    }
                    break;
                }
        }

        // this disregards density - could do if else lol
        _iRandXRange = Random.Range(m_fGroundRegion[0] * -1.0f, m_fGroundRegion[0]);
        _iRandZRange = Random.Range(m_fGroundRegion[1] * -1.0f, m_fGroundRegion[1]);
        _newPos = new Vector3(_iRandXRange, 0.31f, _iRandZRange); // Give Up


        return _newPos;
     
    }

    /// <summary>
    /// pretty shit atm just checks the lists initialized within the start mehtod - trees and bushes... - have a separate check for house in createnewpos
    /// </summary>
    /// <returns></returns>
    private bool CompleteRegionalObjectCheck(GameObject _goObj, bool _bUsingDensity, Vector3 _vec3FirstPos)
    {
        bool _bSpawned;
        // Check the newly spawned NPC's position for overlapping
        switch(_bUsingDensity)
        {
            case false:
                {
                    for (int k = 0; k < m_iRegionalObjectCount; k++)
                    {
                        if (1 < CheckObjectDistance(_goObj.transform.position, m_goRegionalObjects[k].transform.position))
                        {
                            _bSpawned = GenerateNewPosition(100, _goObj, false, new Vector3(0,0,0)); // 100 attempts
                        }
                    }
                    break;
                }
            case true:
                {
                    for (int k = 0; k < m_iRegionalObjectCount; k++)
                    {
                        if (1 < CheckObjectDistance(_goObj.transform.position, m_goRegionalObjects[k].transform.position))
                        {
                            _bSpawned = GenerateNewPosition(100, _goObj, true, _vec3FirstPos); // 100 attempts
                        }
                    }
                    break;
                }
        }
        return true;
    }



    private bool SpawnResources()
    {
        bool _bSpawned;
        Vector3 spawnPos;
        switch (m_bUsingDensity)
        {
            case false:
                {
                    for (int i = 0; i < m_iResourceCount; i++)
                    {
                        for (int j = 0; j < m_iSpawnQuantity[i]; j++)
                        {
                            _bSpawned = false;
                            spawnPos = CreateNewPosition(false,new Vector3(0,0,0));
                            m_goResourcesSpawned.Add(Instantiate(m_goResources[i], spawnPos, Quaternion.identity) as GameObject);
                            _bSpawned = CompleteRegionalObjectCheck(m_goResourcesSpawned[j].gameObject, false, new Vector3(0,0,0));
                            if (!_bSpawned)
                            {
                                print("Object had trouble spawning");
                            }
                            // Check the newly spawned NPC's position for overlapping
                        }
                    }
                    break;
                }
            case true:
                {
                    bool _firstSpawn = true;
                    Vector3 _vec3FirstSpawnPos = new Vector3();
                    int _iObjID = -1;
                    for (int i = 0; i < m_iResourceCount; i++)
                    {
                        for (int j = 0; j < m_iSpawnDensity[i] * 2; j++) // *2 because of the wasted loop caused by if/else
                        {
                            _iObjID++;
                                if (_firstSpawn)
                                {
                                    _bSpawned = false;
                                    spawnPos = CreateNewPosition(false, new Vector3(0, 0, 0));
                                    m_goResourcesSpawned.Add(Instantiate(m_goResources[i], spawnPos, Quaternion.identity) as GameObject); // COULD JUST CHECK THEIR POS BEFORE SPAWN BUT CEEBS
                                    _bSpawned = CompleteRegionalObjectCheck(m_goResourcesSpawned[_iObjID].gameObject, false, new Vector3(0,0,0));
                                    _vec3FirstSpawnPos = new Vector3(m_goResourcesSpawned[_iObjID].transform.position.x, m_goResourcesSpawned[_iObjID].transform.position.y, m_goResourcesSpawned[_iObjID].transform.position.z);
                                    _firstSpawn = false;
                                }
                                else
                                {
                                     for (int l = 0; l < m_iDensityQuantity[i] - 1; l++) // -1 for original spawn
                                    {
                                        _bSpawned = false;
                                        spawnPos = CreateNewPosition(true, _vec3FirstSpawnPos);
                                        m_goResourcesSpawned.Add(Instantiate(m_goResources[i], spawnPos, Quaternion.identity) as GameObject); // COULD JUST CHECK THEIR POS BEFORE SPAWN BUT CEEBS
                                        _bSpawned = CompleteRegionalObjectCheck(m_goResourcesSpawned[_iObjID].gameObject, true, _vec3FirstSpawnPos);
                                    }
                                    _firstSpawn = true;
                                 }
                                
                        }
                    }
                    break;
                }
        }
        return true;
    }

    private float CheckObjectDistance(Vector3 _objectA, Vector3 _objectB)
    {
        return Vector3.Distance(_objectA, _objectB);
    }

    private bool CheckForNearbyObjects(Vector3 _vec3NewPos)
    {
        for (int i = 0; i < m_iRegionalObjectCount; i++)
        {
            if (1 < CheckObjectDistance(_vec3NewPos, m_goRegionalObjects[i].transform.position))
            {
                return true;
            }
        }
        return false;
    }

    private bool GenerateNewPosition(int _iAttempts, GameObject _goObjPosToAlter, bool _bCreatingNearByPos, Vector3 _vec3FirstPos)
    {
            for (int i = 0; i < _iAttempts; i++)
            {
                bool _bObjClear = false;

                Vector3 spawnPos;
                if (!_bCreatingNearByPos)
                {
                    spawnPos = CreateNewPosition(false, new Vector3(0, 0, 0));
                }
                else
                {
                // need to set this up more efficiently
                    spawnPos = CreateNewPosition(true, _vec3FirstPos);
                }

                 _bObjClear = CheckForNearbyObjects(spawnPos);

                if (_bObjClear)
                {
                    //print("Object clear :: SPAWN ACCEPTED");
                    _goObjPosToAlter.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
                    return true;
                }
            }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (!m_bResourcesSpwaned)
        {
            // havent included scuffedtrees or the corrupt zone for potential hazards to spawns..
            // could do this at build to save doing it say each time we respawn resources.. idk
            print("|| Spawning Resources ||");
            m_bResourcesSpwaned = SpawnResources();
        }

        // could do some checking here to see when a resources needs refreshing..
    }
}
