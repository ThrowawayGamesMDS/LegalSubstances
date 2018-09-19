using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectableUnitComponent : MonoBehaviour {
    public bool isSelected;
    public GameObject selectionCircle;
    public NavMeshAgent agent;
    public Camera camera;
    public Animator anim;
    public GameObject Home;
    public GameObject Work;
    public bool isGoingHome;
    public int inputAmount;
    public int outputAmount;
    public GameObject Target;
    public bool canAttack;
    public GameObject attackEffect;
    public GameObject handPosition;

    public void AttackCooldown()
    {
        canAttack = true;
    }

    private void Start()
    {
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        isGoingHome = true;
        canAttack = true;
    }

    public void Update()
    {
        if (agent.velocity.magnitude > 0)
        {
            anim.Play("WalkCycle");
        }
        else
        {
            anim.Play("Idle");
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
                    agent.isStopped = false;
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            {
                                if (Work != null)
                                {
                                    if (Work.tag == "Building")
                                    {
                                        Work.GetComponent<BuildingController>().worker = null;
                                        
                                    }
                                    Work = null;
                                }
                                agent.stoppingDistance = 7;
                                agent.SetDestination(hit.point);

                                break;
                            }
                        case "Enemy":
                            {
                                if(Work != null)
                                {
                                    if (Work.tag == "Building")
                                    {
                                        Work.GetComponent<BuildingController>().worker = null;
                                    }

                                }
                                Work = GameObject.FindGameObjectWithTag("Army");
                                transform.parent = Work.transform;
                                Target = hit.transform.gameObject;
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
            if (Work.tag == "Building")
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
                                    case "White":
                                        {
                                            Home.GetComponent<HouseController>().WhiteAmount += outputAmount;
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
                                if (Work.GetComponent<BuildingController>().outputAmount > 0)
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

            if (Work.tag == "Army")
            {
                if(Target != null)
                {

                    if(Vector3.Distance(transform.position, Target.transform.position) > 10)
                    {
                        agent.stoppingDistance = 8;
                        agent.SetDestination(Target.transform.position);
                    }
                    else
                    {
                        agent.isStopped = true;
                        if (canAttack)
                        {
                            GameObject instance = Instantiate(attackEffect, handPosition.transform.position, handPosition.transform.rotation);
                            instance.transform.LookAt(Target.transform.position);
                            canAttack = false;
                            Invoke("AttackCooldown", 5f);
                        }
                    }
                }
            }
        }
            
    }            

}
