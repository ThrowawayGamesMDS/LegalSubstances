using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BroodShroomController : MonoBehaviour
{
    public GameObject Fiend;
    public Animator anim;
    public float m_fEnemyHealth;
    private bool triggered;
    public List<WongleController> targets;
    private bool lockout;
    private bool cooldown;
    // Use this for initialization
    void Start()
    {
        triggered = false;
        lockout = false;
    }

    void Update()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }
        if (targets.Count > 0)
        {
            lockout = true;
            triggered = true;
        }
        else
        {
            lockout = false;
            triggered = false;
        }


        if (DayNight.isDay)
        {
            if (!lockout)
            {
                triggered = false;
            }
        }
        else
        {
            triggered = true;
        }


        if (targets.Count > 0)
        {
            if (!cooldown)
            {
                anim.Play("spawning");
                cooldown = true;
                Invoke("fCooldown", 7);
            }
        }







        if (m_fEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void EnemyShot(float damage)
    {
        print("EnemyShot");
        m_fEnemyHealth -= damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Wongle")
        {
            targets.Insert(targets.Count, other.gameObject.GetComponent<WongleController>());
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Wongle")
        {
            targets.Remove(other.gameObject.GetComponent<WongleController>());
        }

    }

    public void fSpawnFiend()
    {
        Instantiate(Fiend, transform.position, transform.rotation);
    }

    public void fCooldown()
    {
        cooldown = false;

    }

}
