using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMesh : MonoBehaviour {
    public GameObject farm1;
    public GameObject farm2;
    public GameObject farm3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<BuildingController>().timer < gameObject.GetComponent<BuildingController>().TimeToComplete * 0.33f)
        {
            farm1.SetActive(true);
            farm2.SetActive(false);
            farm3.SetActive(false);
        }
        if (gameObject.GetComponent<BuildingController>().timer >= gameObject.GetComponent<BuildingController>().TimeToComplete * 0.33f)
        {
            farm1.SetActive(false);
            farm2.SetActive(true);
            farm3.SetActive(false);
        }
        if (gameObject.GetComponent<BuildingController>().timer >= gameObject.GetComponent<BuildingController>().TimeToComplete * 0.66f)
        {
            farm1.SetActive(false);
            farm2.SetActive(false);
            farm3.SetActive(true);
        }
	}
}
