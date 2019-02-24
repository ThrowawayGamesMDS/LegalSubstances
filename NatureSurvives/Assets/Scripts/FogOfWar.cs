using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public Camera m_camera;
    public GameObject m_fogOfWar;
    public Transform m_player;
    //public List<GameObject> m_allWongles;
    
    public LayerMask m_fogLayer;
    public float m_radius;
    private float m_radiusSqr {  get { return m_radius * m_radius; } }

    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private Color[] m_colors;

    public GameObject[] m_wongleObjects;
   

    // Start is called before the first frame update
    void Start()
    {
        Initialise();

        //m_wongleObjects = GameObject.FindGameObjectsWithTag("WongleController");
    }

    // Update is called once per frame
    void Update()
    {
        for (int j = 0; j < m_wongleObjects.Length; j++)
        {
            m_player = m_wongleObjects[j].transform;

            Ray r = new Ray(m_camera.transform.position, m_player.position - m_camera.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
            {
                for (int i = 0; i < m_vertices.Length; i++)
                {
                    Vector3 v = m_fogOfWar.transform.TransformPoint(m_vertices[i]);
                    float dist = Vector3.SqrMagnitude(v - hit.point);

                    if (dist < m_radiusSqr)
                    {
                        float alpha = Mathf.Min(m_colors[i].a, dist / m_radiusSqr);
                        m_colors[i].a = alpha;
                    }
                }
                UpdateColor();
            }


        }

        
    }

    void Initialise()
    {
        m_mesh = m_fogOfWar.GetComponent<MeshFilter>().mesh;
        m_vertices = m_mesh.vertices;
        m_colors = new Color[m_vertices.Length];
        for (int i = 0; i < m_colors.Length; i++)
        {
            m_colors[i] = Color.black;
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        m_mesh.colors = m_colors;
    }
}
