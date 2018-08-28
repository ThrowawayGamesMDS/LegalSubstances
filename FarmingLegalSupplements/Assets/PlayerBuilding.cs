using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour {
    public float BuildingHealth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (BuildingHealth <= 0)
        {
            Destroy(gameObject);
        }
	}
}
