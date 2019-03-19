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
    
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private float timer;

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
            agent.SetDestination(targetsInArea[0].transform.position);
        }
        else
        {
            //agent.SetDestination(home.transform.position);
        }

        if (agent.velocity.magnitude > 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RunLoop"))
            {
                anim.Play("RunLoop");
            }
        }

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
        if (m_fEnemyHealth <= 0)
        {
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
                        print("hide enemy");
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
            targetsInArea.Add(other.gameObject);
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
            healthBarCanvasGameObject.SetActive(true);
        }
    }

    public void attackEnemy()
    {
        if (targetsInArea[0] != null)
        {
            targetsInArea[0].SendMessage("TakeDamage", 4);
        }
    }
    
    void HideEnemy()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetChild(3).gameObject.SetActive(false);
    }

    void ShowEnemy()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Renderer>().enabled = true;
        transform.GetChild(3).gameObject.SetActive(true);
    }

}
