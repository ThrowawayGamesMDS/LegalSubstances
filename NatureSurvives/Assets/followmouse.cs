using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followmouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Input.mousePosition; 
    }
}
