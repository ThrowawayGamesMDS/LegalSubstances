using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSUNUI : MonoBehaviour
{
    public float daynightspeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-daynightspeed * Time.deltaTime, 0, 0);
    }
}
