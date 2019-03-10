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
            GameObject m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
            if (m_fogOfWar)
            {
                FogGenerator fogComponent = m_fogOfWar.transform.GetComponent<FogGenerator>();
                float m_buildingRadius = fogComponent.m_buildingRadius;
                fogComponent.Unfog(transform.position, m_buildingRadius * m_buildingRadius);
            }

            Instantiate(m_Building, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
