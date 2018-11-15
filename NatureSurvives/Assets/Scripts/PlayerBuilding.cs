using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBuilding : MonoBehaviour {
    public float BuildingHealth;
    public bool isWonder;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (BuildingHealth <= 0)
        {
            if(isWonder)
            {
                SceneManager.LoadScene(0);
            }
            Destroy(gameObject);
        }
	}
}
