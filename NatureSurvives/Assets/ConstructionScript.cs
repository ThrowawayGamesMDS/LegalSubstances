using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionScript : MonoBehaviour
{
    public float m_fTimeToComplete;
    public float m_fCurrentCompletion;
    public GameObject worker;
    public GameObject m_Building;
    private void Update()
    {
        if(m_fCurrentCompletion >= m_fTimeToComplete)
        {
            Instantiate(m_Building, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
