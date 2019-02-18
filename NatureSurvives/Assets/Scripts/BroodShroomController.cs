using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BroodShroomController : MonoBehaviour {
    public GameObject Fiend;
    public Animator anim;
    public float m_fEnemyHealth;
    private bool triggered;
    private bool lockout;
    // Use this for initialization
    void Start () {
        InvokeRepeating("repeat", 5, 20);
        triggered = false;
        lockout = false;
	}
	
    void repeat()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if(triggered)
        {
            anim.Play("spawning");
            yield return new WaitForSeconds(2);
            Instantiate(Fiend, transform.position, transform.rotation);
        }
        
    }


    void Update()
    {
        if (m_fEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
        if(DayNight.isDay)
        {
            if(!lockout)
            {
                triggered = false;
            }
        }
        else
        {
            triggered = true;
        }
    }

    void EnemyShot(float damage)
    {
        print("EnemyShot");
        m_fEnemyHealth -= damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Wongle")
        {
            triggered = true;
            lockout = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Wongle")
        {
            triggered = false;
            lockout = false;
        }

    }

}
