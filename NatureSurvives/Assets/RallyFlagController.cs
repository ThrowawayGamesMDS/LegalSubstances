using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyFlagController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //////////////////////////
        //should probably have a system to only show the flag when you have the home building selected but for now just gonna delete it
        //////////////////////////



        GameObject temp = GameObject.FindGameObjectWithTag("RallyFlag");
        if (temp != gameObject)
        {
            Destroy(temp);
        }
        GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().g_v3KnightRally = transform.position;
        GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().g_v3WizardRally = transform.position;
        GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().g_v3WorkerRally = transform.position;
    }
    
}
