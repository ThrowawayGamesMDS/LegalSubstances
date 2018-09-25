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
    public float WongleHealth;
    public List<GameObject> Enemies;
    public GameObject attackinstance;
    public GameObject WandEnd;


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
        if(WongleHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (agent.velocity.magnitude > 0)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("BasicSwingAttack"))
            {
                anim.Play("WalkCycle");
            }
            
        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BasicSwingAttack"))
            {
                anim.Play("Idle");
            }
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
                                if(Target != null)
                                {
                                    if(attackinstance != null)
                                    {
                                        Destroy(attackinstance);
                                    }
                                    Target = null;
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
                                        isGoingHome = true;
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

                if(Target == null)
                {
                    if(FindClosestEnemy() != null)
                    {
                        GameObject obj = FindClosestEnemy();
                        if (Vector3.Distance(transform.position, obj.transform.position) <= 30)
                        {
                            Target = obj;
                        }
                    }
                }


                if(Target != null)
                {

                    if(Vector3.Distance(transform.position, Target.transform.position) > 20)
                    {
                        agent.isStopped = false;
                        agent.stoppingDistance = 19;
                        agent.SetDestination(Target.transform.position);
                    }
                    else
                    {
                        agent.isStopped = true;
                        if (canAttack)
                        {
                            attackinstance = Instantiate(attackEffect, handPosition.transform.position, handPosition.transform.rotation);
                            attackinstance.transform.parent = handPosition.transform;
                            attackinstance.GetComponent<LookAtTarget>().target = Target.transform.GetChild(1).gameObject;

                            StartCoroutine(PlayAnim());

                            canAttack = false;
                            Invoke("AttackCooldown", 5f);
                        }
                    }
                }
            }
        }
          
        
        if(agent.isStopped)
        {
            if(Target != null)
            {
                Vector3 lookpos = Target.transform.position - transform.position;
                lookpos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookpos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
            }
        }
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Enemies.Insert(Enemies.Count, other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Enemies.Remove(other.gameObject);
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(2.3f);
        print("anim");
        anim.Play("BasicSwingAttack");
        yield return new WaitForSeconds(0.5f);
        if(attackinstance != null)
        {
            attackinstance.transform.parent = null;
        }
    }

}
