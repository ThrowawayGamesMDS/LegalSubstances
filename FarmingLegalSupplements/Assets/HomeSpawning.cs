using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSpawning : MonoBehaviour {
    public List<GameObject> Prefabs;
    
	// Use this for initialization
	void Start () {
		
	}
	
    public void SpawnUnit(string unittype)
    {
        GameObject temp;
        switch (unittype)
        {
            case "Worker":
                {
                    temp = Instantiate(Prefabs[0], transform.position, Prefabs[0].transform.rotation);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(20, 0 , -20));
                    break;
                }
            case "Wizard":
                {
                    temp = Instantiate(Prefabs[1], transform.position, Prefabs[1].transform.rotation);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(10, 0, -20));
                    break;
                }
            case "Knight":
                {
                    temp = Instantiate(Prefabs[2], transform.position, Prefabs[2].transform.rotation);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(0, 0, -20));
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

}
