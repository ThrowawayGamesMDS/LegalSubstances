using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodScript : MonoBehaviour {
    public float WoodHealth;
    public int yield;
    public GameObject worker;
    public GameObject trunk;
    public bool m_HasBeenMined;
    bool m_bHidden = false;

    private void Start()
    {     
    }

    private void Update()
    {
        if (gameObject.tag == "Crystal")
        {
            if (!m_bHidden)
            {
                hideMiningUI();
                m_bHidden = true;
            }
            else
            {

            }
        }
    }

    void hideMiningUI()
    {
        //for all the mines disable UI inside fog of war
        GameObject m_fowGameObject;
        m_fowGameObject = GameObject.FindGameObjectWithTag("Fog");
        if (m_fowGameObject)
        {
            Transform m_fowTransform = m_fowGameObject.transform;

            int m_cubeSize = 0;
            m_cubeSize = m_fowGameObject.GetComponent<FogGenerator>().m_cubeSize;
            //count the number of cubes
            int childCount = m_fowTransform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = m_fowTransform.GetChild(i);
                GameObject childObject = child.gameObject;

                if (childObject.activeInHierarchy)
                {
                    float distanceSquared = (child.position - transform.position).sqrMagnitude;

                    if (distanceSquared < m_cubeSize * m_cubeSize)
                    {
                        transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }       
    }

    //void showMiningUI(Vector3 m_fogPosition)
    //{
    //    int m_iCountHiddenCrystals = 0;
    //    //add a boolean to the crystal, (maybe in the wood script) to check if it has been mined already
    //    //when the wongle approaches a crystal
    //    //actvate the UI again.
    //    //for each of the fog cubes we need to activate the UI again, if unfogged

    //    //GameObject[] m_crystalObjects;
    //    //m_crystalObjects = GameObject.FindGameObjectsWithTag("Crystal");

    //    //for (int j = 0; j < m_crystalObjects.Length; j++)
    //    //{
    //        if (!m_crystalObjects[j].GetComponent<WoodScript>().m_HasBeenMined)
    //        {
    //            m_iCountHiddenCrystals++;

    //            float distanceSquared = (m_fogPosition - m_crystalObjects[j].transform.position).sqrMagnitude;

    //            if (distanceSquared < m_cubeSize * m_cubeSize)
    //            {
    //                m_crystalObjects[j].transform.GetChild(1).gameObject.SetActive(true);
    //                m_crystalObjects[j].GetComponent<WoodScript>().m_HasBeenMined = true;
    //            }
    //        }
    //    //}

    //    if (m_iCountHiddenCrystals == 0)
    //    {
    //        m_bAllCrystalsRevealed = true;
    //    }
    //}
}
