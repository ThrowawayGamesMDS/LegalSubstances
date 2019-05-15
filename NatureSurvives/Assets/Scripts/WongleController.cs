﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WongleController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public GameObject Home;
    public GameObject StorageBuilding;
    public GameObject Work;
    public GameObject Target;
    public GameObject attackEffect;
    public GameObject handPosition;
    public GameObject attackinstance;
    public GameObject WandEnd;
    public GameObject UnitSelect;
    public GameObject DeadWongle;
    public List<GameObject> Enemies;
    public bool isGoingHome;
    public bool m_bIsScout; // Utilizing tag as wongleworker for scouts, but this bool will be switched :D
    public int inputAmount;
    public float outputAmount;
    public bool canAttack;
    public float WongleHealth;
    public SelectableUnitComponent.workerType type;
    private Vector3 placeholderPosition;
    public int priority;
    private bool isDead;
    private float startingSpeed;

    [Header("XP Stuff")]
    public int iOverallLevel;
    public int iWoodCutLevel;
    public int iFishingLevel;
    public int iMineLevel, iFarmLevel;
    public float iTreesCut, iRocksMined, iFarmsHarvested, iFishCaught;

    [Header("Fishing Stuff")]
    public float m_fTimeToFish;
    public float m_fCurrentFishTime;
    public float m_fFishingYield;
    public Vector3 m_vFishingSpot;
    public bool animlock, animlock2 = false;
    public hidefishyboy hide;

    [Header("Health Bar")]
    private Image healthBarImage;
    private float startHealth;
    private Transform healthBarCanvasTransform;
    public GameObject healthBarCanvasGameObject;

    [Header("Idle Selection Bool")]
    public bool m_bIdleSelected;

    [Header("Idle Selection Bool")]
    public float m_fAnimationSpeed;
    // Use this for initialization
    void Start()
    {
        if (m_bIsScout)
        {
            startingSpeed = agent.speed * 2.5f;
            m_fAnimationSpeed =  2.5f;
            WongleHealth = 1;
        }
        else
        {
            startingSpeed = agent.speed;
            m_fAnimationSpeed = 1.0f;
        }

        anim.speed = m_fAnimationSpeed;
        isDead = false;
        type = gameObject.GetComponent<SelectableUnitComponent>().Type;
        Home = GameObject.FindGameObjectWithTag("HomeBuilding");
        StorageBuilding = Home;
        isGoingHome = true;
        canAttack = true;
        m_bIdleSelected = false;
        startHealth = WongleHealth;

        healthBarCanvasTransform = transform.Find("Health Bar");
        healthBarImage = healthBarCanvasTransform.GetChild(0).GetChild(0).GetComponent<Image>();
        healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
        healthBarCanvasGameObject.SetActive(false);
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
    private void RetainPlayerReources(int _case)
    {
        switch (_case)
        {
            case 0:
                {
                    HouseController.WoodAmount += Mathf.RoundToInt(outputAmount);
                    iTreesCut++;
                    break;
                }
            case 1:
                {
                    HouseController.CrystalAmount += Mathf.RoundToInt(outputAmount);
                    iRocksMined++;
                    break;
                }
            case 2:
                {
                    HouseController.m_iFoodCount += Mathf.RoundToInt(outputAmount);
                    break;
                }
            default:
                return;
        }

        outputAmount = 0;
        isGoingHome = !isGoingHome;
    }


    // Update is called once per frame
    void Update()
    {
        if(Target == null)
        {
            Target = null;
        }
        
        //check level of resource gathering
        iWoodCutLevel = Mathf.FloorToInt(Mathf.Sqrt((iTreesCut / 3)));
        iFarmLevel = Mathf.FloorToInt(Mathf.Sqrt((iFarmsHarvested / 3)));
        iMineLevel = Mathf.FloorToInt(Mathf.Sqrt((iRocksMined / 3)));
        iFishingLevel = Mathf.FloorToInt(Mathf.Sqrt((iFishCaught / 3)));
        iOverallLevel = iWoodCutLevel + iMineLevel + iFarmLevel;

        agent.speed = startingSpeed + iOverallLevel;



        ChangePriority();
        if (WongleHealth <= 0)
        {           
            if(isDead == false)
            {
                NotificationManager.Instance.SetNewNotification("Wongle Died");
                isDead = true;
                Death();
            }
            
        }


        if (agent.velocity.magnitude > 0)
        {
            if (m_bIdleSelected && Target != null)
            {
                m_bIdleSelected = false;
                Debug.Log("Reset WongleController bool 'm_bIdleSelected' of OBJ (" + gameObject + ")");
            }

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("StartMagic"))
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("magicattack"))
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AxeChop"))
                    {
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PickaxeSwing"))
                        {
                            if (type == SelectableUnitComponent.workerType.Melee)
                            {
                                anim.Play("RunCycle");
                            }
                            else
                            {
                                anim.Play("WalkCycle");
                            }

                        }
                    }
                }
            }

        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("StartMagic"))
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("magicattack"))
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AxeChop"))
                    {
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PickaxeSwing"))
                        {
                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FlossDance"))
                            {
                                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RodCast"))
                                {
                                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FishingIdle"))
                                    {
                                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("CatchReel"))
                                        {
                                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("CatchAdmireIdle"))
                                            {
                                                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FishingEnd"))
                                                {
                                                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FarmingLoop"))
                                                    {
                                                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                                                        {
                                                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                                                            {
                                                                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                                                                {
                                                                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DamageTaken"))
                                                                    {
                                                                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                                                                        {
                                                                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DeathAnimation"))
                                                                            {
                                                                                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DeathLoop"))
                                                                                {
                                                                                    anim.Play("Idle");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
                    agent.SetDestination(StorageBuilding.transform.position);
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
                                inputAmount += Work.GetComponent<BuildingController>().AmountProduced;

                                //HouseController.m_iFoodCount += Mathf.RoundToInt(outputAmount * (iFarmLevel / 3));
                                /*  if (iFarmLevel > 0)
                                  {
                                      outputAmount += ((iFarmLevel * outputAmount) / 3);
                                      print("Wongle's farm output: " + outputAmount);
                                  }*/
                                RetainPlayerReources(2);
                            }
                            else
                            {
                                Work.GetComponent<BuildingController>().inputAmount += inputAmount;
                                inputAmount = 0;
                                anim.Play("FarmingLoop");
                                Work.GetComponent<BuildingController>().isOccupied = true;
                                if (Work.GetComponent<BuildingController>().outputAmount > 0)
                                {
                                    outputAmount += Work.GetComponent<BuildingController>().outputAmount;
                                    Work.GetComponent<BuildingController>().outputAmount = 0;
                                    iFarmsHarvested++;
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
                        //gameObject.transform.LookAt(Target.transform);
                        GameObject obj = FindClosestTag("Enemy");
                        if (Vector3.Distance(transform.position, obj.transform.position) <= 30)
                        {
                            Target = obj;
                        }
                    }
                }


                if (Target != null)
                {
                    if (type == SelectableUnitComponent.workerType.Ranged)
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

                    if (type == SelectableUnitComponent.workerType.Melee)
                    {
                        if (Vector3.Distance(transform.position, Target.transform.position) > 7)
                        {
                            agent.isStopped = false;
                            agent.stoppingDistance = 6;
                            agent.SetDestination(Target.transform.position);
                        }
                        else
                        {
                            agent.isStopped = true;
                            if (canAttack)
                            {
                                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                                {
                                    anim.Play("Attack1");
                                }
                            }
                        }
                    }

                }
            }

            if (Work.tag == "WoodCutter")
            {

                if (Target != null)
                {
                    if (Vector3.Distance(transform.position, Target.transform.position) > 5)
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = false;
                            agent.stoppingDistance = 4;
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
                            ChopEvent();
                            Target.GetComponent<WoodScript>().WoodHealth -= 1 * Time.deltaTime;
                            outputAmount += (Time.deltaTime * 0.6f);
                            if (Target.GetComponent<WoodScript>().WoodHealth <= 0)
                            {
                                placeholderPosition = Target.transform.position;
                                Instantiate(Target.GetComponent<WoodScript>().trunk, Target.transform.position, Target.transform.rotation);
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
                    if (FindClosestTag("Storage") != null)
                    {
                        GameObject obj = FindClosestTag("Storage");
                        if (Vector3.Distance(Home.transform.position, transform.position) > Vector3.Distance(obj.transform.position, transform.position))
                        {
                            StorageBuilding = obj;
                        }
                        else
                        {
                            StorageBuilding = Home;
                        }
                    }
                    else
                    {
                        StorageBuilding = Home;
                    }

                    anim.Play("WalkCycle");
                    agent.isStopped = false;
                    agent.SetDestination(StorageBuilding.transform.position);

                }

                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            if (isGoingHome)
                            {
                                /* if (iWoodCutLevel > 0)
                                 {
                                     outputAmount += ((iWoodCutLevel * outputAmount) / 3);
                                     print("Wongle's wood output: " + outputAmount);
                                 }*/
                                RetainPlayerReources(0);
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
                            MineEvent();
                            Target.GetComponent<WoodScript>().WoodHealth -= 1 * Time.deltaTime;
                            outputAmount += (Time.deltaTime * 0.55f);
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
                    if (FindClosestTag("Storage") != null)
                    {
                        GameObject obj = FindClosestTag("Storage");
                        if (Vector3.Distance(Home.transform.position, transform.position) > Vector3.Distance(obj.transform.position, transform.position))
                        {
                            StorageBuilding = obj;
                        }
                        else
                        {
                            StorageBuilding = Home;
                        }
                    }
                    else
                    {
                        StorageBuilding = Home;
                    }


                    anim.Play("WalkCycle");
                    agent.isStopped = false;
                    agent.SetDestination(StorageBuilding.transform.position);

                }

                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            if (isGoingHome)
                            {
                               /* if (iMineLevel > 0)
                                {
                                    outputAmount += ((iMineLevel * outputAmount) / 3);
                                    print("Wongle's mining output: " + outputAmount);
                                }*/
                                RetainPlayerReources(1);
                            }
                        }
                    }
                }
            }

            if (Work.tag == "Builder")
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
                        agent.isStopped = true;
                        //building Animation animation
                        anim.Play("PickaxeSwing");
                        Target.GetComponent<ConstructionScript>().m_fCurrentCompletion += 1 * Time.deltaTime;
                    }
                }
                else
                {
                    placeholderPosition = transform.position;
                    FindNewTarget("Construction");
                }
            }

            if (Work.tag == "Fishermen")
            {

                if (Target != null)
                {
                    if (Vector3.Distance(transform.position, m_vFishingSpot) > 3)
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = false;
                            agent.stoppingDistance = 2;
                            agent.SetDestination(m_vFishingSpot);
                            anim.Play("WalkCycle");
                        }
                    }
                    else
                    {
                        if (!isGoingHome)
                        {
                            agent.isStopped = true;
                            //fishing animation
                            if (!animlock2)
                            {
                                if(agent.velocity.magnitude == 0)
                                {
                                    anim.Play("RodCast");
                                }      
                            }
                            m_fCurrentFishTime += Time.deltaTime;
                            if (m_fCurrentFishTime >= m_fTimeToFish)
                            {
                                if (!animlock)
                                {
                                    anim.Play("CatchReel");
                                    hide.isVisible = true;
                                    animlock = true;
                                }
                            }
                        }
                    }
                }



                if (isGoingHome)
                {
                    if (FindClosestTag("Storage") != null)
                    {
                        GameObject obj = FindClosestTag("Storage");
                        if (Vector3.Distance(Home.transform.position, transform.position) > Vector3.Distance(obj.transform.position, transform.position))
                        {
                            StorageBuilding = obj;
                        }
                        else
                        {
                            StorageBuilding = Home;
                        }
                    }
                    else
                    {
                        StorageBuilding = Home;
                    }

                    anim.Play("WalkCycle");
                    agent.isStopped = false;
                    agent.SetDestination(StorageBuilding.transform.position);

                }

                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            if (isGoingHome)
                            {
                                if (iFishingLevel > 0)
                                {
                                    outputAmount += ((iFishingLevel * outputAmount) / 3);
                                }
                                HouseController.m_iFoodCount += Mathf.RoundToInt(outputAmount);
                                outputAmount = 0;
                                m_fCurrentFishTime = 0;
                                iFishCaught++;
                                animlock = false;
                                animlock2 = false;
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
                if(Work.tag == "Fishermen")
                {
                    Vector3 lookpos2 = m_vFishingSpot - transform.position;
                    lookpos2.y = 0;
                    Quaternion rotation2 = Quaternion.LookRotation(lookpos2);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation2, Time.deltaTime * 10);
                }
                else
                {
                    Vector3 lookpos = Target.transform.position - transform.position;
                    lookpos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookpos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                }
                
            }
        }
    }


    private void LateUpdate()
    {
        if (agent.velocity.magnitude <= 0)
        {
            if (!agent.pathPending)
            {
                agent.avoidancePriority = Random.Range(90, 98);
            }
        }
        else
        {
            agent.avoidancePriority = priority;
        }

        if(isGoingHome)
        {
            //agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        else
        {
            //agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }
    }




    public void AttackCooldown()
    {
        canAttack = true;
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
        yield return new WaitForSeconds(1.2f);
        print("anim");
        anim.Play("StartMagic");        
        yield return new WaitForSeconds(0.5f);
        if (attackinstance != null)
        {
            attackinstance.transform.parent = null;
        }
    }

    public void MeleeAttack()
    {
        Target.SendMessage("EnemyShot", 20);
        //play wongle hit sound
    }

    public void TakeDamage(int damage)
    {
        WongleHealth -= damage;
        //play take damage sound

        healthBarImage.fillAmount = WongleHealth / startHealth;

        if (healthBarImage.fillAmount < 1.0f && !healthBarCanvasGameObject.activeSelf)
        {
            healthBarImage.GetComponent<Image>().color = Color.green;
            healthBarCanvasGameObject.SetActive(true);
        }

        if (healthBarImage.fillAmount < 0.5)
        {
            if (healthBarImage.GetComponent<Image>().color == Color.green)
            {
                healthBarImage.GetComponent<Image>().color = Color.red;
            }
            else
            {
                healthBarImage.GetComponent<Image>().color = Color.green;
            }
            
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
            if (placeholderPosition != null)
            {
                agent.SetDestination(placeholderPosition);
            }


        }
    }




    
    // these functions get called in certain frames of the animations animations

//woodcut anim
    public void ChopEvent()
    {
       // gameObject.GetComponent<AudioHandler>().PlaySound(AudioHandler.m_soundTypes.WOOD);
        AudioHandler.m_ahHandler.UpdatedPlaySound(AudioHandler.m_soundTypes.WOOD, gameObject.GetComponent<AudioSource>());
    }
    //mining animation
    public void MineEvent()
    {
        //gameObject.GetComponent<AudioHandler>().PlaySound(AudioHandler.m_soundTypes.MINE);
        AudioHandler.m_ahHandler.UpdatedPlaySound(AudioHandler.m_soundTypes.MINE, gameObject.GetComponent<AudioSource>());
    }

    //end of admire animation
    public void GoHome()
    {
        print("GoHome");
        outputAmount += m_fFishingYield;
        m_fCurrentFishTime = 0;
        isGoingHome = true;
    }
    //start of cast anim
    public void UnlockAnim()
    {
        animlock2 = true;
    }
    //end of admire anim
    public void Visible()
    {
        hide.isVisible = false;
    }


    
    public void Death()
    {
        //instead should instantiate a fake wongle but in the dead animation so that we dont have to remove the dead wongle from every list
        Instantiate(DeadWongle, transform.position, transform.rotation);
        
        Destroy(gameObject);
    }

    


    

}
