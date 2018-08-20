using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour {
    public GameObject test;
    public Camera camera;
    public GameObject obj;
	// Use this for initialization
	void Start () {
        obj = Instantiate(test);
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        int layermask = LayerMask.GetMask("TransparentFX");
        if (Physics.Raycast(ray, out hit, 100, layermask))
        {
            obj.transform.position = hit.point;
        }

        
	}
}
