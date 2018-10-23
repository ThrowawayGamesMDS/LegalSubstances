using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WongleController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera camera;
    public Animator anim;
    public GameObject Home;
    public GameObject Work;
    public GameObject Target;
    public GameObject attackEffect;
    public GameObject handPosition;
    public GameObject attackinstance;
    public GameObject WandEnd;
    public GameObject UnitSelect;
    public List<GameObject> Enemies;
    public bool isSelected;
    public bool isGoingHome;
    public int inputAmount;
    public float outputAmount;
    public bool canAttack;
    public float WongleHealth;
    public SelectableUnitComponent.workerType type;
    private Vector3 placeholderPosition;
    

    // Use this for initialization
    void Start()
    {
        type = gameObject.GetComponent<SelectableUnitComponent>().Type;
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        isGoingHome = true;
        canAttack = true;
    }


    void ChangePriority()
    {
        if (Work != null)
        {
            switch (Work.tag)

            {
                case "Army":
                    {
                        gameObject.GetComponent<DangerPriority>().m_iDangerPriority = 2;
                        if (!canAttack)
                        {
                            gameObject.GetComponent<DangerPriority>().m_iDangerPriority = 0;
                        }
                        break;
                    }
                default:
                    {
                        gameObject.GetComponent<DangerPriority>().m_iDangerPriority = 5;
                        break;
                    }
            }
        }
        else
        {
            gameObject.GetComponent<DangerPriority>().m_iDangerPriority = 5;
        }
        
        
    }


    // Update is called once per frame
    void Update()
    {
        ChangePriority();
        if (WongleHealth <= 0)
        {
            Destroy(gameObject);
        }


        if (agent.velocity.magnitude > 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BasicSwingAttack"))
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AxeChop"))
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PickaxeSwing"))
                    {
                            anim.Play("WalkCycle");
                    }
                }

            }

        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BasicSwingAttack"))
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AxeChop"))
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PickaxeSwing"))
                    {
                        anim.Play("Idle");
                    }
                }

            }
        }


        if (Work != null)
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
                                            Home.GetComponent<HouseController>().GreenAmount += Mathf.RoundToInt(outputAmount);
                                            outputAmount = 0;
                                            break;
                                        }
                                    case "White":
                                        {
                                            Home.GetComponent<HouseController>().WhiteAmount += Mathf.RoundToInt(outputAmount);
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

                if (Target == null)
                {
                    if (FindClosestTag("Enemy") != null)
                    {
                        GameObject obj = FindClosestTag("Enemy");
                        if (Vector3.Distance(transform.position, obj.transform.position) <= 30)
                        {
                            Target = obj;
                        }
                    }
                }


                if (Target != null)
                {
                    if(type == SelectableUnitComponent.workerType.Ranged)
                    {
                        if (Vector3.Distance(transform.position, Target.transform.position) > 20)
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

                    if(type == SelectableUnitComponent.workerType.Melee)
                    {
                        if (Vector3.Distance(transform.position, Target.transform.position) > 5)
                        {
                            agent.isStopped = false;
                            agent.stoppingDistance = 4;
                            agent.SetDestination(Target.transform.position);
                        }
                        else
                        {
                            agent.isStopped = true;
                            if (canAttack)
                            {
                                //attackinstance = Instantiate(attackEffect, handPosition.transform.position, handPosition.transform.rotation);
                                //attackinstance.transform.parent = handPosition.transform;
                                //attackinstance.GetComponent<LookAtTarget>().target = Target.transform.GetChild(1).gameObject;

                                StartCoroutine(MeleeAttack());
                                canAttack = false;
                                Invoke("AttackCooldown", 1.2f);
                            }
                        }
                    }
                    
                }
            }

            if (Work.tag == "WoodCutter")
            {

                if (Target != null)
                {
                    if (Vector3.Distance(transform.position, Target.transform.position) > 3)
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = false;
                            agent.stoppingDistance = 2;
                            agent.SetDestination(Target.transform.position);
                            anim.Play("WalkCycle");
                        }
                    }
                    else
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = true;
                            //woodchop animation
                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AxeChop"))
                            {
                                anim.Play("AxeChop");
                            }

                            Target.GetComponent<WoodScript>().WoodHealth -= 1 * Time.deltaTime;
                            outputAmount += ((Target.GetComponent<WoodScript>().yield * Time.deltaTime) / 5);
                            if (Target.GetComponent<WoodScript>().WoodHealth <= 0)
                            {
                                placeholderPosition = Target.transform.position;
                                Destroy(Target);
                            }
                        }

                    }
                }
                else
                {
                    FindNewTarget("Wood");
                }

                if (outputAmount >= 10)
                {
                    isGoingHome = true;

                }


                if (isGoingHome)
                {
                    anim.Play("WalkCycle");
                    agent.isStopped = false;
                    agent.SetDestination(Home.transform.position);

                }

                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            if (isGoingHome)
                            {
                                Home.GetComponent<HouseController>().WoodAmount += Mathf.RoundToInt(outputAmount);
                                outputAmount = 0;
                                isGoingHome = !isGoingHome;
                            }
                        }
                    }
                }
            }

            if (Work.tag == "Miner")
            {

                if (Target != null)
                {
                    if (Vector3.Distance(transform.position, Target.transform.position) > 6)
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = false;
                            agent.stoppingDistance = 5;
                            agent.SetDestination(Target.transform.position);
                            anim.Play("WalkCycle");
                        }
                    }
                    else
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = true;
                            //Mining Animation animation
                            anim.Play("PickaxeSwing");
                            Target.GetComponent<WoodScript>().WoodHealth -= 1 * Time.deltaTime;
                            outputAmount += ((Target.GetComponent<WoodScript>().yield * Time.deltaTime) / 5);
                            if (Target.GetComponent<WoodScript>().WoodHealth <= 0)
                            {
                                placeholderPosition = Target.transform.position;
                                Destroy(Target);
                            }
                        }

                    }
                }
                else
                {
                    FindNewTarget("Crystal");
                }

                if (outputAmount >= 10)
                {
                    isGoingHome = true;

                }


                if (isGoingHome)
                {
                    anim.Play("WalkCycle");
                    agent.isStopped = false;
                    agent.SetDestination(Home.transform.position);

                }

                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            if (isGoingHome)
                            {
                                Home.GetComponent<HouseController>().CrystalAmount += Mathf.RoundToInt(outputAmount);
                                outputAmount = 0;
                                isGoingHome = !isGoingHome;
                            }
                        }
                    }
                }
            }

        }

        if (agent.isStopped)
        {
            if (Target != null)
            {
                Vector3 lookpos = Target.transform.position - transform.position;
                lookpos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookpos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
            }
        }
    }







    public void AttackCooldown()
    {
        canAttack = true;
    }


    IEnumerator MeleeAttack()
    {
        anim.Play("BasicSwingAttack");
        yield return new WaitForSeconds(0.5f);
        if (Target != null)
        {
            Target.SendMessage("EnemyShot", 30);
        }
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemies.Insert(Enemies.Count, other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemies.Remove(other.gameObject);
        }
    }

    public GameObject FindClosestTag(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
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
        if (attackinstance != null)
        {
            attackinstance.transform.parent = null;
        }
    }



    void FindNewTarget(string tag)
    {
        if (FindClosestTag(tag) != null)
        {
            agent.isStopped = false;
            GameObject obj = FindClosestTag(tag);
            if (Vector3.Distance(transform.position, obj.transform.position) <= 50)
            {
                Target = obj;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(placeholderPosition);

        }
    }
}
