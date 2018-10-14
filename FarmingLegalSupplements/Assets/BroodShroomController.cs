using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BroodShroomController : MonoBehaviour {
    public NavMeshAgent agent;
    public GameObject Fiend;
    public Animator anim;
	// Use this for initialization
	void Start () {
        agent.SetDestination(GameObject.FindGameObjectWithTag("HomeBuilding").transform.position);
        InvokeRepeating("repeat", 5, 10);
	}
	
    void repeat()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        anim.Play("spawning");
        yield return new WaitForSeconds(2);
        Instantiate(Fiend, transform.position, transform.rotation);
    }

	// Update is called once per frame
	void Update () {
	}


}
