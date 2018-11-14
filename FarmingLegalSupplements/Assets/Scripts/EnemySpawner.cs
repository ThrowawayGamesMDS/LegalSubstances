using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private bool isinvoked;
    public GameObject obj;
    
    public GameObject corruptedGrids;
	// Use this for initialization
	void Start () {
        isinvoked = false;	
	}
	
	// Update is called once per frame
	void Update () {
        
            if (!DayNight.isDay)
            {
                if (!isinvoked)
                {
                    for (int i = 0; i < DayNight.DaysPlayed - 1; i++)
                    {
                        int temp = Random.Range(0, corruptedGrids.transform.childCount - 1);
                        Instantiate(obj, corruptedGrids.transform.GetChild(temp).position, Quaternion.Euler(new Vector3(0, 45, 0)));
                        isinvoked = true;
                    }
                }
            }
            else
            {
                isinvoked = false;
            }
        
        
	}


}
