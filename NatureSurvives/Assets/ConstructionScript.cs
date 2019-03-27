using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionScript : MonoBehaviour
{
    public float m_fTimeToComplete;
    public float m_fCurrentCompletion;
    public GameObject worker;
    public GameObject m_Building;

    [Header("Progress Bar")]
    public Transform ProgressBarImage;
    
    private void Update()
    {
        ProgressBarImage.transform.localScale = new Vector3(m_fCurrentCompletion / m_fTimeToComplete, 1, 1);


        if (GameObject.FindGameObjectWithTag("CheatHandler").gameObject != null)
        {
            switch (GameObject.FindGameObjectWithTag("CheatHandler").GetComponent<CheatHandler>().m_bDisabledBuildTimer)
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
                                fogComponent.Unfog(transform.position, m_buildingRadius, gameObject.GetInstanceID());
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
                            fogComponent.Unfog(transform.position, m_buildingRadius, gameObject.GetInstanceID());
                        }

                        Instantiate(m_Building, transform.position, transform.rotation);
                        Destroy(gameObject);
                        break;
                    }
            }
        }
        else
        {
            if (m_fCurrentCompletion >= m_fTimeToComplete)
            {
                GameObject m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
                if (m_fogOfWar)
                {
                    FogGenerator fogComponent = m_fogOfWar.transform.GetComponent<FogGenerator>();
                    float m_buildingRadius = fogComponent.m_buildingRadius;
                    fogComponent.Unfog(transform.position, m_buildingRadius, gameObject.GetInstanceID());
                }

                Instantiate(m_Building, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
