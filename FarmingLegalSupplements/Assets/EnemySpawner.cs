using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private bool isinvoked;
    public GameObject obj;
	// Use this for initialization
	void Start () {
        isinvoked = false;	
	}
	
	// Update is called once per frame
	void Update () {
		if(!isinvoked)
        {
            if(!DayNight.isDay)
            {
                for(int i = 0; i < DayNight.DaysPlayed;i++)
                {
                    Instantiate(obj, transform.position, transform.rotation);
                }
                isinvoked = true;
            }

        }
        else
        {
            if(DayNight.isDay)
            {
                CancelInvoke("SpawnEnemy");
                isinvoked = false;
            }
            
        }
	}


    void SpawnEnemy()
    {
        Instantiate(obj, transform.position, transform.rotation);
    }
}
