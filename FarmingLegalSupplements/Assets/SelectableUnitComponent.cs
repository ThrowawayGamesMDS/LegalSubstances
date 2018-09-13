using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectableUnitComponent : MonoBehaviour {
    public bool isSelected;
    public bool isAttacking;
    public GameObject selectionCircle;
    public NavMeshAgent agent;
    public Camera camera;
    public Animator anim;
    public GameObject Home;
    public GameObject Work;
    public bool isGoingHome;
    public int inputAmount;
    public int outputAmount;



    private void Start()
    {
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        isGoingHome = true;
    }

    public void Update()
    {
        if (agent.velocity.magnitude > 0)
        {
            anim.Play("WalkCycle");
        }
        else
        {

        }

        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                int layermask = LayerMask.GetMask("Ground", "Enemy", "Building");
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, layermask))
                {
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            {
                                agent.SetDestination(hit.point);
                                if (Work != null)
                                {
                                    Work.GetComponent<BuildingController>().worker = null;
                                    Work = null;
                                }
                                break;
                            }
                        case "Enemy":
                            {
                                if (Work != null)
                                {
                                    Work.GetComponent<BuildingController>().worker = null;
                                    Work = null;
                                }
                                isAttacking = true;
                                agent.SetDestination(hit.point);
                                break;
                            }
                        case "Building":
                            {
                                if (hit.transform.GetChild(0).gameObject.GetComponent<BuildingController>() != null)
                                {
                                    if (hit.transform.GetChild(0).gameObject.GetComponent<BuildingController>().worker == null)
                                    {
                                        hit.transform.GetChild(0).gameObject.GetComponent<BuildingController>().worker = gameObject;
                                        Work = hit.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        agent.SetDestination(hit.point);
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
        }

        

        if(Work != null)
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
                        if(isGoingHome)
                        {
                            switch (Work.GetComponent<BuildingController>().input)
                            {
                                case "Seeds":
                                    {
                                        inputAmount += Work.GetComponent<BuildingController>().AmountProduced;
                                        
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            switch (Work.GetComponent<BuildingController>().output)
                            {
                                case "Green":
                                    {
                                        Home.GetComponent<HouseController>().GreenAmount += outputAmount;
                                        outputAmount = 0;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            isGoingHome = !isGoingHome;
                        }
                        else
                        {
                            Work.GetComponent<BuildingController>().inputAmount += inputAmount;
                            inputAmount = 0;
                            Work.GetComponent<BuildingController>().isOccupied = true;
                            if(Work.GetComponent<BuildingController>().outputAmount > 0)
                            {
                                outputAmount += Work.GetComponent<BuildingController>().outputAmount;
                                Work.GetComponent<BuildingController>().outputAmount = 0;
                                isGoingHome = !isGoingHome;
                                Work.GetComponent<BuildingController>().isOccupied = false;
                            }
                        }
                    }
                }
            }
        }
    }            

}
