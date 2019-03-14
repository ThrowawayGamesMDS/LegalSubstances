﻿using System.Collections;
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
        switch(GameObject.FindGameObjectWithTag("CheatHandler").GetComponent<CheatHandler>().m_bDisabledBuildTimer)
        {
            case false:
                {
                    if (m_fCurrentCompletion >= m_fTimeToComplete)
                    {
                        GameObject m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
                        if (m_fogOfWar)
                        {
                            FogGenerator fogComponent = m_fogOfWar.transform.GetComponent<FogGenerator>();
                            float m_buildingRadius = fogComponent.m_buildingRadius;
                            fogComponent.Unfog(transform.position, m_buildingRadius, transform.gameObject.GetInstanceID());
                        }

                        Instantiate(m_Building, transform.position, transform.rotation);
                        Destroy(gameObject);
                    }
                    break;
                }
            case true:
                {
                    GameObject m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
                    if (m_fogOfWar)
                    {
                        FogGenerator fogComponent = m_fogOfWar.transform.GetComponent<FogGenerator>();
                        float m_buildingRadius = fogComponent.m_buildingRadius;
                        fogComponent.Unfog(transform.position, m_buildingRadius, transform.gameObject.GetInstanceID());
                        // note we might need to change this instance ID to the instatiate's gameobject's intance ID below.
                    }

                    Instantiate(m_Building, transform.position, transform.rotation);
                    Destroy(gameObject);
                    break;
                }
        }
    }
}
