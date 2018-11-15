using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontrotate : MonoBehaviour {
    public Quaternion OGRotation;
	// Use this for initialization
	void Start () {
        OGRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.rotation = OGRotation;
	}
}
