using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class processingWorker : MonoBehaviour {
    public GameObject Home;
    public GameObject Work;
    public bool isGoingHome;
    public NavMeshAgent agent;
    public int GreenCarryingAmount;
    public int WhiteCarryingAmount;
    public int GreenProcessedAmount;
    public int WhiteProcessedAmount;
    public bool isGreenWorker;
    // Use this for initialization
    void Start()
    {
        Work = transform.parent.gameObject;
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        if(Work.GetComponent<PlantController>().typeOfPlant == "Green")
        {
            isGreenWorker = true;
        }
        else if(Work.GetComponent<PlantController>().typeOfPlant == "White")
        {
            isGreenWorker = false;
        }
        isGoingHome = true;
        GreenCarryingAmount = 0;
        WhiteCarryingAmount = 0;
        WhiteProcessedAmount = 0;
        GreenProcessedAmount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingHome)
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
                    if (isGoingHome)
                    {
                        if(isGreenWorker)
                        {
                            GreenCarryingAmount += Home.GetComponent<HouseController>().GreenAmount;
                            Home.GetComponent<HouseController>().GreenAmount = 0;
                        }
                        else
                        {
                            WhiteCarryingAmount += Home.GetComponent<HouseController>().WhiteAmount;
                            Home.GetComponent<HouseController>().WhiteAmount = 0;
                        }
                        Home.GetComponent<HouseController>().GreenProcessed += GreenProcessedAmount;
                        GreenProcessedAmount = 0;
                        Home.GetComponent<HouseController>().WhiteProcessed += WhiteProcessedAmount;
                        WhiteProcessedAmount = 0;
                        if(WhiteCarryingAmount > 0 || GreenCarryingAmount > 0)
                        {
                            isGoingHome = !isGoingHome;
                        }
                        
                    }
                    else
                    {
                        if (Work.GetComponent<PlantController>().typeOfPlant == "Green")
                        {
                            Work.GetComponent<PlantController>().greenAmount += GreenCarryingAmount;
                            GreenCarryingAmount = 0;
                            if (Work.GetComponent<PlantController>().timer >= Work.GetComponent<PlantController>().TimeToProcess)
                            {
                                GreenProcessedAmount += Work.GetComponent<PlantController>().greenAmount;
                                Work.GetComponent<PlantController>().greenAmount = 0;
                                Work.GetComponent<PlantController>().timer = 0;
                                isGoingHome = !isGoingHome;
                            }
                        }
                        else if (Work.GetComponent<PlantController>().typeOfPlant == "White")
                        {
                            Work.GetComponent<PlantController>().whiteAmount += WhiteCarryingAmount;
                            WhiteCarryingAmount = 0;
                            if (Work.GetComponent<PlantController>().timer >= Work.GetComponent<PlantController>().TimeToProcess)
                            {
                                WhiteCarryingAmount += Work.GetComponent<PlantController>().whiteAmount;
                                Work.GetComponent<PlantController>().whiteAmount = 0;
                                Work.GetComponent<PlantController>().timer = 0;
                                isGoingHome = !isGoingHome;
                            }
                        }
                    }
                }
            }
        }
    }
}
