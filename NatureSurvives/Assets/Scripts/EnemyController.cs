using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
	}

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        //targetsInArea.Sort(SortByPriority);

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if(targetsInArea.Count <= 0)
            {
                if(DayNight.isDay)
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
            if (targetsInArea[0] == null)
            {
                targetsInArea.RemoveAt(0);
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
    

    void EnemyShot(float damage)
    {
        print("EnemyShot");
        m_fEnemyHealth -= damage;
        //play enemy damage sound
    }
}
