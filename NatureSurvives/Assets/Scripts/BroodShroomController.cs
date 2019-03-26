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
    private bool triggered;
    public List<WongleController> targets;
    private bool lockout;
    private bool cooldown;

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
                int cooldownint = 30 -  Mathf.FloorToInt(1.5f * DayNight.DaysPlayed);
                if(cooldownint <= 0)
                {
                    cooldownint = 5;
                }
                Invoke("fCooldown", cooldownint);
            }
        }

        
        if (m_fEnemyHealth <= 0)
        {
            NotificationManager.Instance.SetNewNotification("Test Notification: BroodShrom is dead");
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
            healthBarCanvasGameObject.SetActive(true);
        }
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
