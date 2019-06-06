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
    public string WongleName;
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
    public float startHealth;
    private Transform healthBarCanvasTransform;
    public GameObject healthBarCanvasGameObject;

    [Header("Idle Selection Bool")]
    public bool m_bIdleSelected;

    [Header("Idle Selection Bool")]
    public float m_fAnimationSpeed;

    public GameObject woodEffect;
    public GameObject leafEffect;
    public GameObject foragingEffect;

    // Use this for initialization
    void Start()
    {
        isDead = false;
        type = gameObject.GetComponent<SelectableUnitComponent>().Type;
        Invoke("givename", 1);

        if (m_bIsScout)
        {
            startingSpeed = agent.speed * 2.5f;
            m_fAnimationSpeed = 2.5f;
            WongleHealth = 1;
            type = SelectableUnitComponent.workerType.Scout;
        }
        else
        {
            startingSpeed = agent.speed;
            m_fAnimationSpeed = 1.0f;
        }

        anim.speed = m_fAnimationSpeed;

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


    private void HandleWongleAnimation(bool _bUnitMoving)
    {
        switch (_bUnitMoving)
        {
            case true:
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
                                    else if (m_bIsScout)
                                    {
                                        anim.Play("WalkCycleScout");
                                    }
                                    else
                                    {
                                        anim.Play("WalkCycle");
                                    }

                                }
                            }
                        }
                    }
                    break;
                }
            case false:
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
                    break;
                }
        }

    }

    private void CheckWongleStatus()
    {
        switch (type)
        {
            case SelectableUnitComponent.workerType.Ranged:
            case SelectableUnitComponent.workerType.Melee:
                {
                    switch (Work.tag)
                    {
                        case "Army":
                            {
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
                                                if (agent.isOnNavMesh)
                                                {
                                                    agent.isStopped = true;
                                                }
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
                                                if (agent.isOnNavMesh)
                                                {
                                                    agent.isStopped = true;
                                                }
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
                                break;
                            }
                    }
                    break;
                }
            case SelectableUnitComponent.workerType.Worker:
                {
                    int _iResourceID = -1;
                    int _iDistanceFromTarget = -1;
                    int _iStoppingDistance = -1;
                    int _iOutputAmount = -1;
                    float _fOutputMultiplier = -1.0f;
                    string _sAnimationClip = "NA";
                    string _sTarget = "NA";
                    Vector3 _vec3DesiredPosition = new Vector3();
                    switch (Work.tag)
                    {
                        case "Building":
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

                                                RetainPlayerReources(2);
                                            }
                                            else
                                            {
                                                Work.GetComponent<BuildingController>().inputAmount += inputAmount;
                                                inputAmount = 0;
                                                anim.Play("FarmingLoop");

                                                float normTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;

                                                if (normTime >= 0.33f && normTime < 0.43f)
                                                {
                                                    Vector3 vec = new Vector3(transform.position.x, 0, transform.position.z);
                                                    Vector3 rot = transform.rotation.eulerAngles;
                                                    rot = new Vector3(rot.x - 25, rot.y + 180, rot.z);
                                                    Instantiate(foragingEffect, vec, Quaternion.Euler(rot));
                                                }

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
                                break;
                            }
                        case "Builder":
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
                                break;
                            }
                        case "WoodCutter":
                            {
                                _iResourceID = 0;
                                _sAnimationClip = "AxeChop";
                                _sTarget = "Wood";
                                _iDistanceFromTarget = 5;
                                _vec3DesiredPosition = Target.transform.position;
                                _iStoppingDistance = 4;
                                _iOutputAmount = 10;
                                _fOutputMultiplier = 0.6f;
                                break;
                            }
                        case "Miner":
                            {
                                _iResourceID = 1;
                                _sAnimationClip = "PickaxeSwing";
                                _sTarget = "Crystal";
                                _iDistanceFromTarget = 6;
                                _vec3DesiredPosition = Target.transform.position;
                                _iStoppingDistance = 5;
                                _iOutputAmount = 10;
                                _fOutputMultiplier = 0.55f;
                                break;
                            }
                        case "Fishermen":
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
                                                if (agent.velocity.magnitude == 0)
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
                                break;
                            }
                    }

                    if (Work.tag == "Builder" || Work.tag == "Building" | Work.tag == "Fishermen")
                        break;

                    if (Target != null)
                    {
                        if (Vector3.Distance(transform.position, _vec3DesiredPosition) > _iDistanceFromTarget)
                        {
                            if (!isGoingHome)
                            {
                                agent.isStopped = false;
                                agent.stoppingDistance = _iStoppingDistance;
                                agent.SetDestination(Target.transform.position);
                                anim.Play("WalkCycle");
                            }
                        }
                        else
                        {
                            if (!isGoingHome)
                            {
                                agent.isStopped = true;
                                if (!anim.GetCurrentAnimatorStateInfo(0).IsName(_sAnimationClip))
                                {
                                    anim.Play(_sAnimationClip);


                                }



                                Target.GetComponent<WoodScript>().WoodHealth -= 1 * Time.deltaTime;
                                outputAmount += (Time.deltaTime * _fOutputMultiplier);
                                if (Target.GetComponent<WoodScript>().WoodHealth <= 0)
                                {
                                    placeholderPosition = Target.transform.position;
                                    if (Work.tag == "WoodCutter")
                                    {
                                        Instantiate(Target.GetComponent<WoodScript>().trunk, Target.transform.position, Target.transform.rotation);
                                    }
                                    Destroy(Target);
                                }

                            }
                        }
                    }
                    else
                    {
                        FindNewTarget(_sTarget);
                    }

                    if (outputAmount >= _iOutputAmount)
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
                                    RetainPlayerReources(_iResourceID);
                                }
                            }
                        }
                    }


                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
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
            if (isDead == false)
            {
                NotificationManager.Instance.SetNewNotification("Wongle Died");
                isDead = true;
                Death();
            }

        }


        if (agent.velocity.magnitude > 0)
        {
            HandleWongleAnimation(true);
        }
        else
        {
            HandleWongleAnimation(false);
        }


        if (Work != null)
        {
            CheckWongleStatus();
        }
        if (agent.isOnNavMesh)
        {
            if (agent.isStopped)
            {
                if (Target != null)
                {
                    if (Work.tag == "Fishermen")
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

        if (isGoingHome)
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

    public GameObject FindNearTarget(string _sTag)
    {
        GameObject[] _temp = null;
        _temp = GameObject.FindGameObjectsWithTag(_sTag);
        GameObject _closest = null;

        Vector3 position = transform.position;
        foreach (GameObject go in _temp)
        {
            if (Vector3.Distance(gameObject.transform.position, go.transform.position) < 30)
            {
                _closest = go;
                print("Obtaining nearest [" + _sTag + "] Object");
                return _closest;
            }
        }
        return null;
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
        AudioHandler.m_ahHandler.UpdatedPlaySound(AudioHandler.m_soundTypes.WOOD, gameObject.GetComponent<AudioSource>());
        if (Target.GetComponent<Animator>() != null)
            Target.GetComponent<Animator>().Play("PineTreeOnHit");
        //particle
        Vector3 vec = new Vector3(Target.transform.position.x, 4, Target.transform.position.z);
        Instantiate(woodEffect, vec, gameObject.transform.rotation);
        Instantiate(leafEffect, vec, gameObject.transform.rotation);
    }
    //mining animation
    public void MineEvent()
    {
        AudioHandler.m_ahHandler.UpdatedPlaySound(AudioHandler.m_soundTypes.MINE, gameObject.GetComponent<AudioSource>());
    }
    public void StepEvent()
    {
        AudioHandler.m_ahHandler.UpdatedPlaySound(AudioHandler.m_soundTypes.FOOTSTEP, gameObject.GetComponent<AudioSource>());
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
    void givename()
    {
       WongleName = WorkerInfoUI.lines[Random.Range(0, WorkerInfoUI.lines.Length)];
    }
}