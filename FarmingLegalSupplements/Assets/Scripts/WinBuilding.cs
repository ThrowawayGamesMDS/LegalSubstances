using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBuilding : MonoBehaviour {
    public float endtimer;
    public float buildingHealth;
	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("WonderUI2").transform.GetChild(0).gameObject.SetActive(true);
        GameObject temp = GameObject.FindGameObjectWithTag("WonderUi").GetComponent<UpdateWonderUi>().wonder = gameObject;
        GameObject.FindGameObjectWithTag("DAYNIGHT").GetComponent<DayNight>().WonderMode();
    }
	
	// Update is called once per frame
	void Update () {
        endtimer -= Time.deltaTime;
        if(endtimer <= 0)
        {
            SceneManager.LoadScene(0);
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
