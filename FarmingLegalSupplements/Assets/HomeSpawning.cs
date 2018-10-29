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
                    if(HouseController.WoodAmount >= Prefabs[0].GetComponent<costToPlace>().WoodCost)
                    {
                        if (HouseController.CrystalAmount >= Prefabs[0].GetComponent<costToPlace>().CrystalCost)
                        {
                            if (HouseController.WhiteAmount >= Prefabs[0].GetComponent<costToPlace>().FoodCost)
                            {
                                temp = Instantiate(Prefabs[0], transform.position, Prefabs[0].transform.rotation);
                                temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                                temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(20, 0, -20));
                                HouseController.WoodAmount -= Prefabs[0].GetComponent<costToPlace>().WoodCost;
                                HouseController.CrystalAmount -= Prefabs[0].GetComponent<costToPlace>().CrystalCost;
                                HouseController.WhiteAmount -= Prefabs[0].GetComponent<costToPlace>().FoodCost;

                            }
                        }
                    }
                    
                    break;
                }
            case "Wizard":
                {
                    if (HouseController.WoodAmount >= Prefabs[1].GetComponent<costToPlace>().WoodCost)
                    {
                        if (HouseController.CrystalAmount >= Prefabs[1].GetComponent<costToPlace>().CrystalCost)
                        {
                            if (HouseController.WhiteAmount >= Prefabs[1].GetComponent<costToPlace>().FoodCost)
                            {
                                temp = Instantiate(Prefabs[1], transform.position, Prefabs[1].transform.rotation);
                                temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                                temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(10, 0, -20));
                                HouseController.WoodAmount -= Prefabs[1].GetComponent<costToPlace>().WoodCost;
                                HouseController.CrystalAmount -= Prefabs[1].GetComponent<costToPlace>().CrystalCost;
                                HouseController.WhiteAmount -= Prefabs[1].GetComponent<costToPlace>().FoodCost;

                            }
                        }
                    }

                    
                    break;
                }
            case "Knight":
                {

                    if (HouseController.WoodAmount >= Prefabs[2].GetComponent<costToPlace>().WoodCost)
                    {
                        if (HouseController.CrystalAmount >= Prefabs[2].GetComponent<costToPlace>().CrystalCost)
                        {
                            if (HouseController.WhiteAmount >= Prefabs[0].GetComponent<costToPlace>().FoodCost)
                            {
                                temp = Instantiate(Prefabs[2], transform.position, Prefabs[2].transform.rotation);
                                temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                                temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(0, 0, -20));
                                HouseController.WoodAmount -= Prefabs[2].GetComponent<costToPlace>().WoodCost;
                                HouseController.CrystalAmount -= Prefabs[2].GetComponent<costToPlace>().CrystalCost;
                                HouseController.WhiteAmount -= Prefabs[2].GetComponent<costToPlace>().FoodCost;

                            }
                        }
                    }
                    
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

}
