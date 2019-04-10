using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BroodShroomController : MonoBehaviour
{
    public GameObject Fiend;
    public Animator anim;
    public float m_fEnemyHealth;
    public bool triggered;
    public List<WongleController> targets;
    private bool lockout;
    public bool cooldown;
    public int cooldownint;

    [Header("Health Bar")]
    private Image healthBarImage;
    private float startHealth;
    private Transform healthBarCanvasTransform;
    private GameObject healthBarCanvasGameObject;

    // Use this for initialization
    void Start()
    {
        triggered = false;
        lockout = false;

        startHealth = m_fEnemyHealth;
        healthBarCanvasTransform = transform.Find("Health Bar");
        healthBarImage = healthBarCanvasTransform.GetChild(0).GetChild(0).GetComponent<Image>();
        healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
        healthBarCanvasGameObject.SetActive(false);
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


        if (triggered)
        {
            if (!cooldown)
            {
                anim.Play("spawning");
                cooldown = true;
                cooldownint = 30 -  Mathf.FloorToInt(1.5f * DayNight.DaysPlayed);
                if(cooldownint <= 0)
                {
                    cooldownint = 5;
                }
                if(DayNight.isWonder)
                {
                    cooldownint = 15;
                }
                Invoke("fCooldown", cooldownint);
            }
        }

        
        if (m_fEnemyHealth <= 0)
        {
            NotificationManager.Instance.SetNewNotification("BroodShrom is dead");
            Destroy(gameObject);
        }
    }

    void EnemyShot(float damage)
    {
        print("EnemyShot");
        m_fEnemyHealth -= damage;

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
        print("spawning enemy fiend");
        Instantiate(Fiend, transform.position, transform.rotation);
        if(DayNight.isWonder)
        {
            Instantiate(Fiend, transform.position, transform.rotation);
            Instantiate(Fiend, transform.position, transform.rotation);
        }
    }

    public void fCooldown()
    {
        cooldown = false;

    }

}
