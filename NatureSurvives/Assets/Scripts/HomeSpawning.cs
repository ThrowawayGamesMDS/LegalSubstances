﻿using System.Collections;
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
    private void Start()
    {
        iCurrentWongleCount = 5;
        iMaximumWongleCount = 10;
    }

    public void fQueueUnit(string unittype)
    {
        if (iCurrentWongleCount < iMaximumWongleCount)
        {
            UnitQueue.Insert(UnitQueue.Count, unittype);
            if(unittype == "Worker")
            {
                GameObject uiobj = Instantiate(workerUI, canvas);
                uiobj.transform.SetParent(canvas, false);
                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                //uiobj.transform.position = new Vector3(uiobj.transform.position.x + 35* uiobj.GetComponent<queueUIScript>().placeInQueue, uiobj.transform.position.y, uiobj.transform.position.z);
            }
            else if(unittype == "Wizard")
            {
                GameObject uiobj = Instantiate(wizardUI, canvas);
                uiobj.transform.SetParent(canvas, false);
                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                //uiobj.transform.position = new Vector3(uiobj.transform.position.x + 35 * uiobj.GetComponent<queueUIScript>().placeInQueue, uiobj.transform.position.y, uiobj.transform.position.z);
            }
            else if(unittype == "Knight")
            {
                GameObject uiobj = Instantiate(knightUI, canvas);
                uiobj.transform.SetParent(canvas, false);
                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                //uiobj.transform.position = new Vector3(uiobj.transform.position.x + 35 * uiobj.GetComponent<queueUIScript>().placeInQueue, uiobj.transform.position.y, uiobj.transform.position.z);
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
                                GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goRangedUnits.Add(temp);
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
                                GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goMeleeUnits.Add(temp);
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
}
