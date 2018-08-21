using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SellerController : MonoBehaviour {
    public GameObject Home;
    public GameObject Work;
    public bool isGoingHome;
    public NavMeshAgent agent;
    public int GreenCarryingAmount;
    public int WhiteCarryingAmount;
    public int CashCarryingAmount;

    void Start()
    {
        Work = transform.parent.gameObject;
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        isGoingHome = true;
        GreenCarryingAmount = 0;
        WhiteCarryingAmount = 0;
        CashCarryingAmount = 0;
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
                        GreenCarryingAmount += Home.GetComponent<HouseController>().GreenProcessed;
                        Home.GetComponent<HouseController>().GreenProcessed = 0;
                        WhiteCarryingAmount += Home.GetComponent<HouseController>().WhiteProcessed;
                        Home.GetComponent<HouseController>().WhiteProcessed = 0;
                        HouseController.CashAmount += CashCarryingAmount;
                        CashCarryingAmount = 0;
                        isGoingHome = !isGoingHome;
                    }
                    else
                    {
                        CashCarryingAmount += GreenCarryingAmount * Work.GetComponent<ShopController>().CostOfGreen;
                        GreenCarryingAmount = 0;
                        CashCarryingAmount += WhiteCarryingAmount * Work.GetComponent<ShopController>().CostOfWhite;
                        WhiteCarryingAmount = 0;
                        
                        isGoingHome = !isGoingHome;
                    }
                }
            }
        }
    }
}
