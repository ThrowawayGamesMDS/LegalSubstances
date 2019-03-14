using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerWithnoGrid : MonoBehaviour
{
    public bool isinvoked;
    public GameObject obj;
    
    // Use this for initialization
    void Start()
    {
        isinvoked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (!DayNight.isDay)
        {
            print("isnight");
            if (!isinvoked)
            {
                print("isntinvoked");
                Instantiate(obj, transform.position, Quaternion.Euler(new Vector3(0, 45, 0)));
                isinvoked = true;                
            }
        }
        else
        {
            isinvoked = false;
        }


    }
}
