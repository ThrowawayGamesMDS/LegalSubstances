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
    public Renderer constructionRenderer;

    [Header("Progress Bar")]
    public Transform ProgressBarImage;

    private void FinalizeConstructionProcess()
    {
        GameObject m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
        if (m_fogOfWar)
        {
            FogGenerator fogComponent = m_fogOfWar.transform.GetComponent<FogGenerator>();
            float m_buildingRadius = fogComponent.m_buildingRadius;
            fogComponent.Unfog(transform.position, m_buildingRadius, gameObject.GetInstanceID());
        }
        GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<PlacementHandler>().m_goObjsPlaced.Add(Instantiate(m_Building, transform.position, transform.rotation));
        Destroy(gameObject);
    }

    private void Update()
    {
        ProgressBarImage.transform.localScale = new Vector3(m_fCurrentCompletion / m_fTimeToComplete, 1, 1);
        constructionRenderer.material.SetFloat("_ConstructY", (m_fCurrentCompletion / m_fTimeToComplete) * 10);


        if (GameObject.FindGameObjectWithTag("CheatHandler").gameObject != null)
        {
            switch (GameObject.FindGameObjectWithTag("CheatHandler").GetComponent<CheatHandler>().m_bDisabledBuildTimer)
            {
                case false:
                    {
                        if (m_fCurrentCompletion >= m_fTimeToComplete)
                        {
                            FinalizeConstructionProcess();
                        }
                        break;
                    }
                case true:
                    {
                        FinalizeConstructionProcess();
                        break;
                    }
            }
        }
        else
        {
            if (m_fCurrentCompletion >= m_fTimeToComplete)
            {
                FinalizeConstructionProcess();
            }
        }
    }
}
