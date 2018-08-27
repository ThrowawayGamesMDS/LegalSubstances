using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmHandler : MonoBehaviour
{
    public enum farm_types
    {
        DEFAULT,WEED,COCAINE
    }
    public farm_types m_eFarmType;
    private int m_iFarmLevel;
    public string m_sDescription;
    public int m_iResourceCount;
    public int m_iLastDisplayedFarmLevel; // for redispay purposes so we arent constantly retexting the text...
	// Use this for initialization
	void Start ()
    {
        m_iResourceCount = gameObject.GetComponentInChildren<farmController>().Yield;
       /* switch(m_eFarmType)
        {
            case farm_types.DEFAULT:
                {
                    m_sDescription = "PLEASE SET THE SCRIPT UP CORRECTLY";
                    break;
                }
            case farm_types.WEED:
                {
                    m_sDescription = "Level " + m_iFarmLevel + " Weed plantation";
                    break;
                }
            case farm_types.COCAINE:
                {
                    m_sDescription = "Level " + m_iFarmLevel + " Cocaine production plant";
                    break;
                }

        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_iResourceCount != gameObject.GetComponentInChildren<farmController>().Yield)
        {
            m_iResourceCount = gameObject.GetComponentInChildren<farmController>().Yield;
        }
        switch (m_eFarmType)
        {
            case farm_types.DEFAULT:
                {
                    m_sDescription = "PLEASE SET THE SCRIPT UP CORRECTLY";
                    break;
                }
            case farm_types.WEED:
                {
                    m_sDescription = "Level " + m_iFarmLevel + " Weed plantation";
                    break;
                }
            case farm_types.COCAINE:
                {
                    m_sDescription = "Level " + m_iFarmLevel + " Cocaine production plant";
                    break;
                }

        }
    }
}
