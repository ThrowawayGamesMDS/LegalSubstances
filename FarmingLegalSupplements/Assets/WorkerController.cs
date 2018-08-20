﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour {
    public GameObject Home;
    public GameObject Work;
    public bool isGoingHome;
    public NavMeshAgent agent;
    public int GreenCarryingAmount;
    public int WhiteCarryingAmount;
    // Use this for initialization
    void Start () {
        Work = transform.parent.gameObject;
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        isGoingHome = true;
        GreenCarryingAmount = 0;
        WhiteCarryingAmount = 0;
    }
	
	// Update is called once per frame
	void Update () {
		if(isGoingHome)
        {
            agent.SetDestination(Home.transform.position);
        }
        else
        {
            agent.SetDestination(Work.transform.position);
        }


        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if(isGoingHome)
                    {
                        Home.GetComponent<HouseController>().GreenAmount += GreenCarryingAmount;
                        GreenCarryingAmount = 0;
                        Home.GetComponent<HouseController>().WhiteAmount += WhiteCarryingAmount;
                        WhiteCarryingAmount = 0;
                        isGoingHome = !isGoingHome;
                    }
                    else
                    {
                        if (Work.GetComponent<farmController>().typeOfFarm == "Green")
                        {
                            if(Work.GetComponent<farmController>().resources >= Work.GetComponent<farmController>().TimeToHarvest)
                            {
                                GreenCarryingAmount += Work.GetComponent<farmController>().Yield;
                                Work.GetComponent<farmController>().resources = 0;
                                isGoingHome = !isGoingHome;
                            }
                        }
                        else if (Work.GetComponent<farmController>().typeOfFarm == "White")
                        {
                            if (Work.GetComponent<farmController>().resources >= Work.GetComponent<farmController>().TimeToHarvest)
                            {
                                WhiteCarryingAmount += Work.GetComponent<farmController>().Yield;
                                Work.GetComponent<farmController>().resources = 0;
                                isGoingHome = !isGoingHome;
                            }
                        }
                    }
                }
            }
        }
    }
}

