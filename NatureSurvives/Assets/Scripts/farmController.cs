using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmController : MonoBehaviour {
    public int TimeToHarvest;
    public int Yield;
    public float resources;
    public string typeOfFarm;
    public bool hasSeeded;
	// Use this for initialization
	void Start () {
        resources = 0;
        hasSeeded = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(resources > TimeToHarvest)
        {
            resources = TimeToHarvest;
        }
        if(hasSeeded)
        {
            resources += 1 * Time.deltaTime;
        }

	}
    
}
