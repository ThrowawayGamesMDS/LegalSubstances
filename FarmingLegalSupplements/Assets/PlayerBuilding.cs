using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBuilding : MonoBehaviour {
    public enum type {green,white,home,shop,Def}
    public type WhatAmI;
    public float BuildingHealth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        if (BuildingHealth <= 0)
        {
            if (WhatAmI == type.home)
            {
                SceneManager.LoadScene(3);
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}
}
