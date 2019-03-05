using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createcircle : MonoBehaviour
{
    public GameObject cube;


    void Start()
    {
        int radius = 25;
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    GameObject temp = Instantiate(cube, new Vector3(x*10,1,y*10), Quaternion.identity, transform);
                    temp.name = "Grid(" + x + ")(" + y + ")";
                    temp.GetComponent<GridObject>().xValue = x;
                    temp.GetComponent<GridObject>().zValue = y;
                }                    
            }
        }            
    }

}
