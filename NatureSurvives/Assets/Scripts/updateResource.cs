using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class updateResource : MonoBehaviour {
    private GameObject home;
    public Text wood, crystal, food;
	// Use this for initialization
	void Start () {
        home = GameObject.FindGameObjectWithTag("HomeBuilding");
	}
	
	// Update is called once per frame
	void Update () {
        wood.text = HouseController.WoodAmount.ToString();
        crystal.text = HouseController.CrystalAmount.ToString();
        food.text = HouseController.m_iFoodCount.ToString();

	}
}
