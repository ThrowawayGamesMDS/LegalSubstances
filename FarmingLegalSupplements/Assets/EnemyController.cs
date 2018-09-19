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

	// Use this for initialization
	void Start () {
        home = GameObject.FindGameObjectWithTag("HomeBuilding");
        //agent.SetDestination(home.transform.position);
	}

    // Update is called once per frame
    void Update()
    {
        if (targetsInArea.Count > 0)
        {
            if (targetsInArea[0] == null)
            {
                targetsInArea.RemoveAt(0);
            }
            //agent.SetDestination(targetsInArea[0].transform.position);
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
                    if (targetsInArea.Count > 0)
                    {
                        targetsInArea[0].GetComponent<PlayerBuilding>().BuildingHealth -= 2 * Time.deltaTime;
                    }
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("AttackLoop"))
                    {
                        anim.Play("AttackLoop");
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
    }


    void EnemyShot(float damage)
    {
        print("EnemyShot");
        m_fEnemyHealth -= damage;
    }
}
