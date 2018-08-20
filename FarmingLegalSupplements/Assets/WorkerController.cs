using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour {
    public GameObject Home;
    public GameObject Work;
	// Use this for initialization
	void Start () {
        Work = transform.parent.gameObject;
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
