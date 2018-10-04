using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private bool isinvoked;
    public GameObject obj;
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;
    public Vector3 pos4;
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
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            {
                                Instantiate(obj, pos1, transform.rotation);
                                break;
                            }
                        case 1:
                            {
                                Instantiate(obj, pos2, transform.rotation);
                                break;
                            }
                        case 2:
                            {
                                Instantiate(obj, pos3, transform.rotation);
                                break;
                            }
                        case 3:
                            {
                                Instantiate(obj, pos4, transform.rotation);
                                break;
                            }
                        default:
                            {
                                Instantiate(obj, pos4, transform.rotation);
                                break;
                            }

                    }

                    
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
