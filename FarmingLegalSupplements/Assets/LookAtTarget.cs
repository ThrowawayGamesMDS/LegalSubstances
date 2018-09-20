using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(target != null)
        {
            transform.LookAt(target.transform.position);
            transform.GetChild(0).GetChild(2).localScale = new Vector3(Vector3.Distance(transform.position, target.transform.position) / 9, 1, 1);
        }
        else
        {
            Destroy(gameObject);
        }
        
	}
}
