using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour {
    GameObject home;
    // Use this for initialization
    void Start () {
        home = GameObject.FindGameObjectWithTag("HomeBuilding");
	}
	
	// Update is called once per frame
	void Update () {
		if(home == null || home.GetComponent<PlayerBuilding>().BuildingHealth <= 0)
        {
            SceneManager.LoadScene(3);
        }
	}
}
