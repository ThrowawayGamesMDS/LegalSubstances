using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookatcamera : MonoBehaviour
{
    public GameObject cam;
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update()
    {
        transform.LookAt(cam.transform.position);
    }

}
