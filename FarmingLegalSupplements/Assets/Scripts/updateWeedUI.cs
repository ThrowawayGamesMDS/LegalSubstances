using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateWeedUI : MonoBehaviour {
    public bool isGreen;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(isGreen)
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HouseController>().GreenAmount.ToString();

        }
        if (!isGreen)
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HouseController>().WhiteAmount.ToString();

        }
    }
}
