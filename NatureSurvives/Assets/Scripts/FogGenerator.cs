using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogGenerator : MonoBehaviour
{
    public GameObject m_fogCube;
    public float m_width = 10;
    public float m_height = 10;
    public float m_cubeSize = 5;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
