using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmHandler : MonoBehaviour
{
    public string m_sDescription;
    public int m_iResourceCount;
	// Use this for initialization
	void Start ()
    {
        m_iResourceCount = gameObject.GetComponentInChildren<farmController>().Yield;

    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_iResourceCount != gameObject.GetComponentInChildren<farmController>().Yield)
        {
            m_iResourceCount = gameObject.GetComponentInChildren<farmController>().Yield;
        }
	}
}
