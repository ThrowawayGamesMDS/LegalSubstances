using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSpawning : MonoBehaviour {
    public List<GameObject> Prefabs;
    public int iCurrentWongleCount;
    public int iMaximumWongleCount;
    public List<string> UnitQueue;
    public List<GameObject> UIObjQueue;
    public bool hasTimer;
    public float timerVal;
    public GameObject workerUI;
    public GameObject wizardUI;
    public GameObject knightUI;
    public Transform canvas;
    public GameObject army;
    private void Start()
    {
        iCurrentWongleCount = 5;
        army = GameObject.FindGameObjectWithTag("Army");
    }

    public void fQueueUnit(string unittype)
    {
        if (iCurrentWongleCount + UIObjQueue.Count < iMaximumWongleCount)
        {
            if (unittype == "Worker")
            {
                if (HouseController.WoodAmount >= Prefabs[0].GetComponent<costToPlace>().WoodCost)
                {
                    if (HouseController.CrystalAmount >= Prefabs[0].GetComponent<costToPlace>().CrystalCost)
                    {
                        if (HouseController.WhiteAmount >= Prefabs[0].GetComponent<costToPlace>().FoodCost)
                        {
                            UnitQueue.Insert(UnitQueue.Count, unittype);
                            GameObject uiobj = Instantiate(workerUI, canvas);
                            uiobj.transform.SetParent(canvas, false);
                            uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                            UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                            HouseController.WoodAmount -= Prefabs[0].GetComponent<costToPlace>().WoodCost;
                            HouseController.CrystalAmount -= Prefabs[0].GetComponent<costToPlace>().CrystalCost;
                            HouseController.WhiteAmount -= Prefabs[0].GetComponent<costToPlace>().FoodCost;
                        }
                    }
                }
            }
            else if (unittype == "Wizard")
            {
                if (HouseController.WoodAmount >= Prefabs[1].GetComponent<costToPlace>().WoodCost)
                {
                    if (HouseController.CrystalAmount >= Prefabs[1].GetComponent<costToPlace>().CrystalCost)
                    {
                        if (HouseController.WhiteAmount >= Prefabs[1].GetComponent<costToPlace>().FoodCost)
                        {
                            UnitQueue.Insert(UnitQueue.Count, unittype);
                            GameObject uiobj = Instantiate(wizardUI, canvas);
                            uiobj.transform.SetParent(canvas, false);
                            uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                            UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                            HouseController.WoodAmount -= Prefabs[1].GetComponent<costToPlace>().WoodCost;
                            HouseController.CrystalAmount -= Prefabs[1].GetComponent<costToPlace>().CrystalCost;
                            HouseController.WhiteAmount -= Prefabs[1].GetComponent<costToPlace>().FoodCost;
                        }
                    }
                }
            }
            else if (unittype == "Knight")
            {
                if (HouseController.WoodAmount >= Prefabs[2].GetComponent<costToPlace>().WoodCost)
                {
                    if (HouseController.CrystalAmount >= Prefabs[2].GetComponent<costToPlace>().CrystalCost)
                    {
                        if (HouseController.WhiteAmount >= Prefabs[2].GetComponent<costToPlace>().FoodCost)
                        {
                            UnitQueue.Insert(UnitQueue.Count, unittype);
                            GameObject uiobj = Instantiate(knightUI, canvas);
                            uiobj.transform.SetParent(canvas, false);
                            uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                            UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                            HouseController.WoodAmount -= Prefabs[2].GetComponent<costToPlace>().WoodCost;
                            HouseController.CrystalAmount -= Prefabs[2].GetComponent<costToPlace>().CrystalCost;
                            HouseController.WhiteAmount -= Prefabs[2].GetComponent<costToPlace>().FoodCost;
                        }
                    }
                }
            }
        }
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
                    temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(120, 0, -100));   
                    break;
                }
            case "Wizard":
                {
                    temp = Instantiate(Prefabs[1], transform.position, Prefabs[1].transform.rotation, army.transform);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().Work = army;
                    temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(105, 0, -20));                                
                    GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goRangedUnits.Add(temp);                  
                    break;
                }
            case "Knight":
                {
                    
                    temp = Instantiate(Prefabs[2], transform.position, Prefabs[2].transform.rotation, army.transform);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().Work = army;
                    temp.GetComponent<WongleController>().agent.SetDestination(new Vector3(85, 0, -20));                                
                    GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goMeleeUnits.Add(temp);                   
                    break;
                }
            default:
                {
                    break;
                }
        }   
    }

    private void Update()
    {
        iCurrentWongleCount = GameObject.FindGameObjectsWithTag("Wongle").Length;

        
        if(UnitQueue.Count > 0)
        {
            if(!hasTimer)
            {
                timerVal = 0;
                hasTimer = true;
            }
            else
            {
                timerVal += Time.deltaTime;
                if(timerVal >= 7)
                {
                    SpawnUnit(UnitQueue[0]);
                    hasTimer = false;
                    timerVal = 0;
                    UnitQueue.RemoveAt(0);
                    Destroy(UIObjQueue[0]);
                    UIObjQueue.RemoveAt(0);
                }
            }
        }   
        
        for (int i = 0; i < UIObjQueue.Count; i++)
        {

            if(i <= 9)
            {
                UIObjQueue[i].transform.localPosition = new Vector3(-116 + 35 * i, -160, 0);
            }
            else if (i > 9 && i <= 19)
            {
                UIObjQueue[i].transform.localPosition = new Vector3(-116 + (35 * (i - 10)), -195, 0);
            }
            else
            {
                UIObjQueue[i].transform.localPosition = new Vector3(-116 + (35 * (i - 19)), -230, 0);
            }
                    
            UIObjQueue[i].GetComponent<queueUIScript>().placeInQueue = i;
        }
    }

    public void Refund(string x)
    {
        if(x == "Worker")
        {
            HouseController.WoodAmount += Prefabs[0].GetComponent<costToPlace>().WoodCost;
            HouseController.WhiteAmount += Prefabs[0].GetComponent<costToPlace>().FoodCost;
            HouseController.CrystalAmount += Prefabs[0].GetComponent<costToPlace>().CrystalCost;
        }
        if (x == "Wizard")
        {
            HouseController.WoodAmount += Prefabs[1].GetComponent<costToPlace>().WoodCost;
            HouseController.WhiteAmount += Prefabs[1].GetComponent<costToPlace>().FoodCost;
            HouseController.CrystalAmount += Prefabs[1].GetComponent<costToPlace>().CrystalCost;
        }
        if (x == "Knight")
        {
            HouseController.WoodAmount += Prefabs[2].GetComponent<costToPlace>().WoodCost;
            HouseController.WhiteAmount += Prefabs[2].GetComponent<costToPlace>().FoodCost;
            HouseController.CrystalAmount += Prefabs[2].GetComponent<costToPlace>().CrystalCost;
        }
    }
}
