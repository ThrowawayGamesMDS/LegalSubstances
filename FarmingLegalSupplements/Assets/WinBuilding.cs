using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBuilding : MonoBehaviour {
    public float endtimer;
    public float buildingHealth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        endtimer -= Time.deltaTime;
        if(endtimer <= 0)
        {
            //win game
            print("win game");
        }
		if(buildingHealth <= 0)
        {
            //lose game
        }
	}


    void buildingDamaged(float damage)
    {
        buildingHealth -= damage;
    }

}
