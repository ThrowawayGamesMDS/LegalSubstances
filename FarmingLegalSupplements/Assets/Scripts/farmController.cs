using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmController : MonoBehaviour {
    public int TimeToHarvest;
    public int Yield;
    public int resources;
    public string typeOfFarm;
	// Use this for initialization
	void Start () {
        resources = 0;
        InvokeRepeating("GainResources", 0.1f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if(resources > TimeToHarvest)
        {
            resources = TimeToHarvest;
        }

	}

    void GainResources()
    {
        resources += 1;
    }
}
