using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FogGenerator : MonoBehaviour
{
    //black cube prefab
    public GameObject m_fogCube;
    // width and height of the whole terrain
    private float m_width = 500;
    private float m_height = 500;

    public int m_cubeSize = 10;
    public float m_wongleRadius = 20;
    public float m_buildingRadius = 40;
    private float m_wongleRadiusSqrared { get { return m_wongleRadius * m_wongleRadius; } }
    public float m_buildingRadiusSqrared { get { return m_buildingRadius * m_buildingRadius; } }

    //we need to put this inside the functions
    //private GameObject[] m_wongleObjects;

    private bool m_bEntireMapReveal = false;
    public bool m_doScale = false;

    //private GameObject[] children;
    //private List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < m_width / m_cubeSize; x++)
        {
            for(int y = 0; y < m_height / m_cubeSize; y ++)
            { 
                GameObject go = Instantiate(m_fogCube, new Vector3(transform.position.x + x * m_cubeSize, transform.position.y, transform.position.z + y * m_cubeSize), Quaternion.identity);

                go.name = "FogCube_" + x + "_" + y;
                go.transform.SetParent(transform);
            }
        }

        //unfog the map based on all the wongles on the map
        GameObject[] m_wongleObjects;
        m_wongleObjects = GameObject.FindGameObjectsWithTag("Wongle");
        for( int i = 0; i < m_wongleObjects.Length; i++)
        {
            Unfog(m_wongleObjects[i].transform.position, m_wongleRadiusSqrared);
        }

        GameObject m_treeHouse;
        m_treeHouse = GameObject.FindGameObjectWithTag("HomeBuilding");
        if(m_treeHouse)
        {
           Unfog(m_treeHouse.transform.position, m_buildingRadiusSqrared);
        }
         

        //children = new GameObject[transform.childCount];
        //int j = 0;
        //foreach(Transform child in transform)
        //{
        //    children[j] = child.gameObject;
        //    j++;
        //}

    }

    //do we still need to do this?  update every 3 frames for optimization
    private int interval = 1;

    // Update is called once per frame
    void Update()
    {
        //we need don't need to do this method anymore when the entire map is revealed
        if (!m_bEntireMapReveal)
        {
            //unfog the map based on only the moving wongles
            //maybe store all wongles inside a game manager so that we don't have to find them on every update
            //m_wongleObjects we need to clear this array
            GameObject[] m_wongleObjects;
            m_wongleObjects = GameObject.FindGameObjectsWithTag("Wongle");

            if (Time.frameCount % interval == 0)    //once every three frames
            {
                for (int i = 0; i < m_wongleObjects.Length; i++)
                {
                    NavMeshAgent agent = m_wongleObjects[i].GetComponent<NavMeshAgent>();
                    if (agent.velocity.sqrMagnitude > 0f)
                    {
                        Unfog(m_wongleObjects[i].transform.position, m_wongleRadiusSqrared);
                    }
                }
            }
        }
    }

    public void Unfog(Vector3 m_playerPosition, float m_radiusSqrared)
    {       
        int countVisibleCubes = 0;

        //Vector3 m_playerPosition = m_player.position;
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            GameObject childObject = child.gameObject;

            if (childObject.activeInHierarchy)
            {
                countVisibleCubes++;

                float distanceSquared = (child.position - m_playerPosition).sqrMagnitude;

                if (distanceSquared < m_radiusSqrared)
                {
                    //Destroy(child.gameObject);
                    //child.gameObject.SetActive(false);

                    if (child.gameObject.activeInHierarchy && !m_doScale)
                    {
                        Destroy(child.gameObject);
                        //child.gameObject.SetActive(false);
                    }
                    //if the child is active and we are scaling, turn off the scaling
                    else if (child.gameObject.activeInHierarchy && m_doScale)
                    {
                        child.GetComponent<FogScaler>().ToggleScale(false);
                    }
                }
            }
        }       

        if (countVisibleCubes == 0)
        {
            m_bEntireMapReveal = true;
        }
    }

    public static float GetSqrDistXZ(Vector3 a, Vector3 b)
    {
        // Vector3 vector = new Vector3(a.x - b.x, 0, a.z - b.z);
        //return vector.sqrMagnitude;

        float xD = a.x - b.x;
        float yD = a.y - b.y; // optional
        float zD = a.z - b.z;
        float dist2 = xD * xD + yD * yD + zD * zD;

        return dist2;
    }

}

// Iterator
//IEnumerator iter = transform.GetEnumerator();
//iter.Reset();
//while (iter.MoveNext())
//{
//    Transform child = (Transform)iter.Current;
//    //Debug.Log(t.name);

//    GameObject childObject = child.gameObject;

//    if (childObject.activeInHierarchy)
//    {
//        countVisibleCubes++;

//    //    //float distance = Vector3.Distance(child.position, m_player.position);
//    //    //float distance = GetSqrDistXZ(child.position, m_player.position);
//        float distanceSquared = (child.position - m_playerPosition).sqrMagnitude;

//        if (distanceSquared < m_radiusSqrared)
//        {
//    //        //hide the child (fog cube)
//            //if (child.gameObject.activeInHierarchy)
//           //{
//           //Destroy(child.gameObject);
//            child.gameObject.SetActive(false);
//    //        //}
//        }
//    }

//}


//var enumerator = transform.GetEnumerator();
//while (enumerator.MoveNext())
//{
//    var element = enumerator.Current;
//    // Loop body.

//    countVisibleCubes++;

//    float distance = Vector3.Distance(child.position, m_player.position);
//    //float distance = GetSqrDistXZ(child.position, m_player.position);

//    if (distance < m_radius)
//    {
//        //hide the child (fog cube)
//        //if (child.gameObject.activeInHierarchy)
//        //{
//        //Destroy(child.gameObject);
//        element.gameObject.SetActive(false);
//        element.
//        //}
//    }
//}

//foreach (Transform child in transform)
//{
//    //we need to skip the cubes that have already been deactivated
//    if (child.gameObject.activeInHierarchy)
//    {
//        countVisibleCubes++;

//        float distance = Vector3.Distance(child.position, m_player.position);
//        //float distance = GetSqrDistXZ(child.position, m_player.position);

//        if (distance < m_radius)
//        {
//            //hide the child (fog cube)
//            //if (child.gameObject.activeInHierarchy)
//            //{
//            //Destroy(child.gameObject);
//            child.gameObject.SetActive(false);
//            //}
//        }
//    }
//}

//for (int i = 0; i < transform.childCount; i++)
//{
//    GameObject childObject = children[i];
//    Transform child = childObject.transform;

//    if (childObject.activeInHierarchy)
//    {
//        countVisibleCubes++;

//        //float distance = Vector3.Distance(child.position, m_player.position);
//        //float distance = GetSqrDistXZ(child.position, m_player.position);
//        float distanceSquared = (child.position - m_playerPosition).sqrMagnitude;

//        if (distanceSquared < m_radiusSqrared)
//        {
//            //hide the child (fog cube)
//            //if (child.gameObject.activeInHierarchy)
//            //{
//            //Destroy(child.gameObject);
//            child.gameObject.SetActive(false);
//            //}
//        }
//    }
//}


