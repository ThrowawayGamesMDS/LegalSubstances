using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickui : MonoBehaviour {
    public CanvasGroup cg;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        cg.alpha -= Time.deltaTime;
	}
}
