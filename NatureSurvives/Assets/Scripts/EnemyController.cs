using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public GameObject home;
    public NavMeshAgent agent;
    public List<GameObject> targetsInArea;
    public Animator anim;
    public float m_fEnemyHealth;
    public GameObject deadFiend;
    public bool isRanged;
    public float wanderRadius;
    public float wanderTimer;
    public GameObject shell;
    public Transform turretTransform;
    private Transform target;
    private float timer;
    public bool isPacked;
    private bool canshoot;
    [Header("Health Bar")]
    private Image healthBarImage;
    private float startHealth;
    private Transform healthBarCanvasTransform;
    private GameObject healthBarCanvasGameObject;

    // Use this for initialization
    void OnEnable()
    {
        timer = wanderTimer;
    }
 

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    
    // Use this for initialization
    void Start () {
        home = GameObject.FindGameObjectWithTag("HomeBuilding");
        //agent.SetDestination(home.transform.position);
        canshoot = true;
        isPacked = true;
        startHealth = m_fEnemyHealth;
        healthBarCanvasTransform = transform.Find("Health Bar");
        healthBarImage = healthBarCanvasTransform.GetChild(0).GetChild(0).GetComponent<Image>();
        healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
        healthBarCanvasGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //targetsInArea.Sort(SortByPriority);

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if (targetsInArea.Count <= 0)
            {
                if (DayNight.isDay)
                {
                    agent.SetDestination(newPos);
                    timer = 0;

                }
                else
                {
                    agent.SetDestination(home.transform.position);
                }
            }


        }

        for (int i = 0; i < targetsInArea.Count; i++)
        {
            if (targetsInArea[i] == null)
            {
                targetsInArea.RemoveAt(i);
            }
        }

        if (targetsInArea.Count > 0)
        {
            if(isRanged)
            {
                agent.stoppingDistance = 70;
            }
            agent.SetDestination(targetsInArea[0].transform.position);
        }
        else
        {
            agent.stoppingDistance = 5;
        }

        if (agent.velocity.magnitude > 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RunLoop"))
            {
                anim.Play("RunLoop");
            }
        }
        else
        {
            if(targetsInArea.Count <= 0)
            {

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !anim.GetCurrentAnimatorStateInfo(0).IsName("FiringCycle") || !anim.GetCurrentAnimatorStateInfo(0).IsName("Unpack") || !anim.GetCurrentAnimatorStateInfo(0).IsName("Pack"))
                {
                    anim.Play("Idle");
                }
            }
        }

        if(isRanged)
        {
            if(targetsInArea.Count > 0)
            {
                if (Vector3.Distance(transform.position, targetsInArea[0].transform.position) > 70)
                {
                    agent.isStopped = false;
                    if(!isPacked)
                    {
                        Pack();
                    }
                    agent.stoppingDistance = 69;
                    agent.SetDestination(targetsInArea[0].transform.position);
                }
                else
                {
                    agent.isStopped = true;
                    if(isPacked)
                    {
                        Unpack();
                    }
                    else
                    {
                        if (targetsInArea[0] != null)
                        {
                            transform.LookAt(targetsInArea[0].transform.position);
                            anim.Play("FiringCycle");
                        }
                    }
                }
            }
            
        }
        else
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        if (targetsInArea.Count != 0)
                        {
                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AttackLoop"))
                            {
                                anim.Play("AttackLoop");
                                NotificationManager.Instance.SetNewNotification("Enemy is attacking " + targetsInArea[0].name);
                            }
                        }
                        else
                        {
                            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                            {
                                anim.Play("Idle");
                            }
                        }


                    }
                }
            }
        }
        
        if (m_fEnemyHealth <= 0)
        {
            NotificationManager.Instance.SetNewNotification("Enemy is dead");
            Instantiate(deadFiend, transform.position, transform.rotation);
            Destroy(gameObject);
        }


        GameObject m_fogOfWar;
        m_fogOfWar = GameObject.FindGameObjectWithTag("Fog");
        if (m_fogOfWar)
        {
            int countVisibleCubes = 0;

            //Vector3 m_playerPosition = m_player.position;
            int childCount = m_fogOfWar.transform.childCount;
            float m_radius = 10.0f;
            float m_radiusSqrared = m_radius * m_radius;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = m_fogOfWar.transform.GetChild(i);
                GameObject childObject = child.gameObject;
                float distanceSquared = (child.position - transform.position).sqrMagnitude;

                //if (childObject.activeInHierarchy)
                //{
                countVisibleCubes++;

                if (distanceSquared < m_radiusSqrared)
                {

                    if (child.gameObject.activeInHierarchy)
                    {
                        HideEnemy();
                        break;
                    }
                    else
                    {
                        ShowEnemy();
                    }
                    break;
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerBuilding>() != null)
        {
            targetsInArea.Add(other.gameObject);
        }
        if (other.GetComponent<SelectableUnitComponent>() != null)
        {
            if(!isRanged)
            {
                targetsInArea.Add(other.gameObject);
            }
            
        }
    }

    public int SortByPriority(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<DangerPriority>().m_iDangerPriority.CompareTo(p2.GetComponent<DangerPriority>().m_iDangerPriority);
    }
    

    public void EnemyShot(float damage)
    {
        print("EnemyShot");
        m_fEnemyHealth -= damage;
        //play enemy damage sound

        healthBarImage.fillAmount = m_fEnemyHealth / startHealth;

        if (healthBarImage.fillAmount < 1.0f && !healthBarCanvasGameObject.activeSelf)
        {
            healthBarImage.GetComponent<Image>().color = Color.red;
            healthBarCanvasGameObject.SetActive(true);
        }

        //if (healthBarImage.fillAmount < 0.5)
        //{
        //    if (healthBarImage.GetComponent<Image>().color == Color.red)
        //    {
        //        healthBarImage.GetComponent<Image>().color = Color.black;
        //    }
        //    else
        //    {
        //        healthBarImage.GetComponent<Image>().color = Color.red;
        //    }

        //}
    }

    public void attackEnemy()
    {
        if(targetsInArea.Count > 0)
        {
            if (targetsInArea[0] != null)
            {
                targetsInArea[0].SendMessage("TakeDamage", 4);
            }
        }
        
    }

    private float requiredForceToHit(Transform target)
    {
        Vector3 forward = turretTransform.forward;
        float g = Physics.gravity.magnitude; //G force
        float y = 0.0f; //Height of the target
        float x = Vector3.Distance(target.transform.position, turretTransform.position); //distance to hit, slight off because turret is lifted up

        float tanG = forward.y / Mathf.Sqrt(forward.x * forward.x + forward.z * forward.z);
        float upper = Mathf.Sqrt(g) * Mathf.Sqrt(x) * Mathf.Sqrt(tanG * tanG + 1.0f);
        float lower = Mathf.Sqrt(2 * tanG - ((2 * y) / x));

        float velocity = upper / lower;

        return velocity;
    }

    private void fire(float force)
    {
        GameObject shellInstance = Instantiate(shell, turretTransform.position, turretTransform.rotation);
        shellInstance.GetComponent<Rigidbody>().velocity = turretTransform.forward * force;
    }

    public void ShootProjectile()
    {
        
        float temp = requiredForceToHit(targetsInArea[0].transform);
        fire(temp);
        //canshoot = false;
        //Invoke("canshootF", 2);
    }

    void Unpack()
    {
        isPacked = false;
        anim.Play("Unpack");
        print("unpack");
    }
    void Pack()
    {
        isPacked = true;
        anim.Play("Pack");
        print("pack");
    }

    void HideEnemy()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetChild(3).gameObject.SetActive(false);
        if (isRanged)
        {
            //transform.GetChild(3).gameObject.SetActive(false);
        }

    }

    void ShowEnemy()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Renderer>().enabled = true;
        transform.GetChild(3).gameObject.SetActive(true);
        if (isRanged)
        {
            //transform.GetChild(3).gameObject.SetActive(false);
        }
    }


    void canshootF()
    {
        canshoot = true;
    }
}
