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
    private float m_extraRadius = 10;
    private float m_wongleRadiusSquared { get { return m_wongleRadius * m_wongleRadius; } }
    public float m_buildingRadiusSqrared { get { return m_buildingRadius * m_buildingRadius; } }

    //we need to put this inside the functions
    //private GameObject[] m_wongleObjects;

    private bool m_bEntireMapReveal = false;
    public bool m_doScale = false;
    private bool m_bAllCrystalsRevealed = false;

    //private GameObject[] children;
    //private List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < m_width / m_cubeSize; x++)
        {
            for (int y = 0; y < m_height / m_cubeSize; y++)
            {
                GameObject go = Instantiate(m_fogCube, new Vector3(transform.position.x + x * m_cubeSize, transform.position.y, transform.position.z + y * m_cubeSize), Quaternion.identity);

                go.name = "FogCube_" + x + "_" + y;
                go.transform.SetParent(transform);
            }
        }

        //unfog the map based on all the wongles on the map
        GameObject[] m_wongleObjects;
        m_wongleObjects = GameObject.FindGameObjectsWithTag("Wongle");
        for (int i = 0; i < m_wongleObjects.Length; i++)
        {
            GameObject m_wongle = m_wongleObjects[i];
            Unfog(m_wongle.transform.position, m_wongleRadius, m_wongle.GetInstanceID());

            //print("wongle  =  " + m_wongleObjects[i].GetInstanceID());
        }

        GameObject m_treeHouse;
        m_treeHouse = GameObject.FindGameObjectWithTag("HomeBuilding");
        if (m_treeHouse)
        {
            Unfog(m_treeHouse.transform.position, m_buildingRadius, m_treeHouse.GetInstanceID());
        }

        //hideMiningUI();

        //children = new GameObject[transform.childCount];
        //int j = 0;
        //foreach(Transform child in transform)
        //{
        //    children[j] = child.gameObject;
        //    j++;
        //}

    }

    void lightenAlpha(GameObject fogCube)
    {
        //fogCube.GetComponent<Renderer>().material.color = Color.yellow;
        Color fogColor = fogCube.GetComponent<Renderer>().material.color;
        fogColor.a = 0.3f;
        fogCube.GetComponent<Renderer>().material.color = fogColor;

        fogCube.transform.position = new Vector3(fogCube.transform.position.x, 0.0f, fogCube.transform.position.z);
        fogCube.transform.localScale = new Vector3(m_fogCube.transform.localScale.x, 0.001f, m_fogCube.transform.localScale.z);
    }
       

    //do we still need to do this?  update every 3 frames for optimization
    private int interval = 3;

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
                    //GameObject.Find("ShareButton").transform.localScale = new Vector3(0, 0, 0);
                    GameObject m_wongle = m_wongleObjects[i];
                    NavMeshAgent agent = m_wongle.GetComponent<NavMeshAgent>();
                    if (agent)
                    {
                        if (agent.velocity.sqrMagnitude > 0f)
                        {
                            Unfog(m_wongle.transform.position, m_wongleRadius, m_wongle.GetInstanceID());
                        }
                    }
                }
            }
        }
    }

   

    public void Unfog(Vector3 m_playerPosition, float m_radius, int instanceID)
    {
        //int countVisibleCubes = 0;

        //Vector3 m_playerPosition = m_player.position;
        int childCount = transform.childCount;
        float m_radiusSqrared = m_radius * m_radius;
        float maxRadiusSquared = (m_radius + m_extraRadius) * (m_radius + m_extraRadius);


        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            GameObject childObject = child.gameObject;
            float distanceSquared = (child.position - m_playerPosition).sqrMagnitude;

            //if (childObject.activeInHierarchy)
            //{
            //countVisibleCubes++;

            if (distanceSquared < m_radiusSqrared)
            {
                //Destroy(child.gameObject);
                //child.gameObject.SetActive(false);

                if (!childObject.GetComponent<FogScaler>().m_instanceIDList.Contains(instanceID))
                {
                    childObject.GetComponent<FogScaler>().m_instanceIDList.Add(instanceID);
                }

                if (childObject.activeInHierarchy && !m_doScale)
                {
                    //Destroy(child.gameObject);

                    childObject.SetActive(false);

                    if (!m_bAllCrystalsRevealed)
                    {
                        showMiningUI(child.position);
                    }
                }
            }

            //if the distance is greater than the desired distance
            else
            {
                if (distanceSquared < maxRadiusSquared)
                {


                    
                    if (!childObject.activeInHierarchy)
                    {
                        if (childObject.GetComponent<FogScaler>().m_instanceIDList.Count < 2)
                        {
                            if (childObject.GetComponent<FogScaler>().m_instanceIDList[0] == instanceID)
                            {
                                //if (childObject.GetComponent<Renderer>().material.color.a == 1.0f)
                                //{
                                //    lightenAlpha(child.gameObject);
                                //}
                                //this actually turns on the 2nd layer of fow
                                child.gameObject.SetActive(true);
                                //child.gameObject.GetComponent<FogScaler>().m_instanceIDList.Remove(instanceID);
                            }
                        }
                        child.gameObject.GetComponent<FogScaler>().m_instanceIDList.Remove(instanceID);
                    }

                    //this just lightens the surrounding radius
                    if (childObject.activeInHierarchy)
                    {
                        lightenAlpha(childObject);
                    }





                    ////this just lightens the surrounding radius
                    //if (childObject.activeInHierarchy)
                    //{
                    //    lightenAlpha(child.gameObject);
                    //}
                    //else
                    //{
                    //    if (childObject.GetComponent<FogScaler>().m_instanceIDList.Count < 2)
                    //    {
                    //        if (childObject.GetComponent<FogScaler>().m_instanceIDList[0] == instanceID)
                    //        {
                    //            if (childObject.GetComponent<Renderer>().material.color.a == 1.0f)
                    //            {
                    //                lightenAlpha(child.gameObject);
                    //            }
                    //            //this actually turns on the 2nd layer of fow
                    //            child.gameObject.SetActive(true);
                    //            //child.gameObject.GetComponent<FogScaler>().m_instanceIDList.Remove(instanceID);
                    //        }
                    //    }
                    //    child.gameObject.GetComponent<FogScaler>().m_instanceIDList.Remove(instanceID);
                    //}
                }
            }
        }

        //if (countVisibleCubes == 0)
        //{
        //    m_bEntireMapReveal = true;
        //}
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


    //for each crystal
    //loop through each cube to see if there is a cube covering the crystal
    //if found, hide the UI
    void hideMiningUI()
    {
        //for all the mines disable UI inside fog of war
        GameObject[] m_crystalObjects;
        m_crystalObjects = GameObject.FindGameObjectsWithTag("Crystal");

        for (int j = 0; j < m_crystalObjects.Length; j++)
        {
            //count the number of cubes
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                GameObject childObject = child.gameObject;

                if (childObject.activeInHierarchy)
                {
                    float distanceSquared = (child.position - m_crystalObjects[j].transform.position).sqrMagnitude;

                    if (distanceSquared < m_cubeSize * m_cubeSize)
                    {
                        m_crystalObjects[j].transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void showMiningUI(Vector3 m_fogPosition)
    {
        int m_iCountHiddenCrystals = 0;
        //add a boolean to the crystal, (maybe in the wood script) to check if it has been mined already
        //when the wongle approaches a crystal
        //actvate the UI again.
        //for each of the fog cubes we need to activate the UI again, if unfogged

        GameObject[] m_crystalObjects;
        m_crystalObjects = GameObject.FindGameObjectsWithTag("Crystal");

        for (int j = 0; j < m_crystalObjects.Length; j++)
        {
            if (!m_crystalObjects[j].GetComponent<WoodScript>().m_HasBeenMined)
            {
                m_iCountHiddenCrystals++; 

                float distanceSquared = (m_fogPosition - m_crystalObjects[j].transform.position).sqrMagnitude;

                if (distanceSquared < m_cubeSize * m_cubeSize)
                {
                    m_crystalObjects[j].transform.GetChild(1).gameObject.SetActive(true);
                    m_crystalObjects[j].GetComponent<WoodScript>().m_HasBeenMined = true;
                    break;
                }
            }
        }

        //if (m_iCountHiddenCrystals == 0)
        //{
        //    //m_bAllCrystalsRevealed = true;
        //}
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


