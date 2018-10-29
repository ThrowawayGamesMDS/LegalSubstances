using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateWonderUi : MonoBehaviour {
    public GameObject wonder;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(wonder != null)
        {
            gameObject.GetComponent<Text>().text = Mathf.Round(wonder.GetComponent<WinBuilding>().endtimer).ToString();
        }
        
	}
}
