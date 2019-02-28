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
    [Tooltip("The rarity of the resource")]
    public int[] m_iSpawnQuantity;
    [Tooltip("How far apart the resource(s) should spawn")]
    public int[] m_iSpawnDensity;
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
        m_fGroundRegion[0] = m_goGround.GetComponent<Renderer>().bounds.size.x / 2;
        m_fGroundRegion[1] = m_goGround.GetComponent<Renderer>().bounds.size.z / 2;
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

    private Vector3 CreateNewPosition()
    {
        //Vector3 _newPos = new Vector3(m_vec3CenterRegion.x + Random.Range(-m_fGroundRegion[0] * 1.0f, m_fGroundRegion[0] * 1.0f), 0.31f, m_vec3CenterRegion.z + Random.Range(-m_fGroundRegion[1] * 1.0f, m_fGroundRegion[1] * 1.0f));

        float _iRandXRange1 = Random.Range((-m_fGroundRegion[0] * 1.0f), m_fGroundRegion[0] * 1.0f);

        float _iRandZRange1 = Random.Range((-m_fGroundRegion[1] * 0.3f), -m_fGroundRegion[1] * 0.9f);
        float _iRandZRange2 = Random.Range((m_fGroundRegion[1] * 0.3f), m_fGroundRegion[1] * 0.9f);

        Vector3 _newPos;

        int _iRand = Random.Range(0, 2);
        print("Random number: " + _iRand);

        if (Random.Range(0,1) == 1)
        {
            _newPos = new Vector3(m_vec3CenterRegion.x + _iRandXRange1,
            0.31f,
            m_vec3CenterRegion.z + _iRandZRange2);
        }
        else
        {
            _newPos =  new Vector3(m_vec3CenterRegion.x + _iRandXRange1,
           0.31f,
           m_vec3CenterRegion.z + _iRandZRange1);
        }
        


        return _newPos;
    }



    private void SpawnResources()
    {
        bool _bSpawned = false;
        for (int i = 0; i < m_iResourceCount; i++)
        {
            for (int j = 0; j < m_iSpawnQuantity[i]; j++)
            {
                Vector3 spawnPos = CreateNewPosition();
                m_goResourcesSpawned.Add(Instantiate(m_goResources[i], spawnPos, Quaternion.identity) as GameObject);
                _bSpawned = true;
                // Check the newly spawned NPC's position for overlapping
                for (int k = 0; k < m_iRegionalObjectCount; k++)
                {
                    if (1 < CheckObjectDistance(m_goResourcesSpawned[i].transform.position, m_goRegionalObjects[i].transform.position))
                    {
                        _bSpawned = GenerateNewPosition(100, m_goResourcesSpawned[i]);
                        if (!_bSpawned)
                        {
                            print("A new position couldn't be found for the object being spawned");
                        }

                    }
                }
            }
        }
        m_bResourcesSpwaned = true;
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

    private bool GenerateNewPosition(int _iAttempts, GameObject _goObjPosToAlter)
    {
            for (int i = 0; i < _iAttempts; i++)
            {
                bool _bObjClear = false;



                Vector3 spawnPos = CreateNewPosition();

                 _bObjClear = CheckForNearbyObjects(spawnPos);

                if (_bObjClear)
                {
                    print("Object clear :: SPAWN ACCEPTED");
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
            SpawnResources();
        }

        // could do some checking here to see when a resources needs refreshing..
    }
}
