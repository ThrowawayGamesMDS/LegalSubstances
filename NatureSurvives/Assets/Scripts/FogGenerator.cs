using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FogGenerator : MonoBehaviour
{
    //black cube prefab
    public GameObject m_fogCube;
    // width and height of the whole terrain
    public float m_width = 10;
    public float m_height = 10;

    public float m_cubeSize = 5;
    public float m_radius = 5;

    public GameObject[] m_wongleObjects;

    private bool m_bEntireMapReveal = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < m_width; x++)
        {
            for(int y = 0; y < m_height; y++)
            {
                GameObject go = Instantiate(m_fogCube, new Vector3(transform.position.x + x * m_cubeSize, transform.position.y, transform.position.z + y * m_cubeSize), Quaternion.identity);

                go.name = "FogCube_" + x + "_" + y;
                go.transform.SetParent(transform);
            }
        }

        //unfog the map based on all the wongles on the map
        m_wongleObjects = GameObject.FindGameObjectsWithTag("Wongle");
        for( int i = 0; i < m_wongleObjects.Length; i++)
        {
            Unfog(m_wongleObjects[i].transform);
        }      
    }

    // Update is called once per frame
    void Update()
    {
        //we need don't need to do this method anymore when the entire map is revealed
        if (!m_bEntireMapReveal)
        {
            //unfog the map based on only the moving wongles
            //maybe store all wongles inside a game manager so that we don't have to find them on every update
            //m_wongleObjects = GameObject.FindGameObjectsWithTag("Wongle");

            for (int i = 0; i < m_wongleObjects.Length; i++)
            {
                NavMeshAgent agent = m_wongleObjects[i].GetComponent<NavMeshAgent>();
                if (agent.velocity.sqrMagnitude > 0f)
                {
                    Unfog(m_wongleObjects[i].transform);
                }
            }
        }
    }

    void Unfog(Transform m_player)
    {       
        int countVisibleCubes = 0;

        foreach (Transform child in transform)
        {
            //we need to skip the cubes that have already been de-activated               
            // if (child.gameObject.activeInHierarchy)
            //{
            countVisibleCubes++;

            float distance = Vector3.Distance(child.position, m_player.position);

            if (distance < m_radius)
            {
                //hide the child (fog cube)
                if (child.gameObject.activeInHierarchy)
                {
                    //Destroy(child);
                    Destroy(child.gameObject);
                    //child.gameObject.SetActive(false);
                }
            }
            //}
        }

        if (countVisibleCubes == 0)
        {
            m_bEntireMapReveal = true;
        }

        print("countVisibleCubes " + countVisibleCubes);
    }
}
