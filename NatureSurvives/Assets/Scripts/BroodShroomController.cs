using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BroodShroomController : MonoBehaviour {
    public GameObject Fiend;
    public Animator anim;
    public float m_fEnemyHealth;
    private bool triggered;
    private bool lockout;
    // Use this for initialization

    [Header("Health Bar")]
    private Image healthBarImage;
    private float startHealth;
    private Transform healthBarCanvasTransform;
    private GameObject healthBarCanvasGameObject;

    void Start () {
        InvokeRepeating("repeat", 5, 20);
        triggered = false;
        lockout = false;

        startHealth = m_fEnemyHealth;
        healthBarCanvasTransform = transform.Find("Health Bar");
        healthBarImage = healthBarCanvasTransform.GetChild(0).GetChild(0).GetComponent<Image>();
        healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
        healthBarCanvasGameObject.SetActive(false);
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

        healthBarImage.fillAmount = m_fEnemyHealth / startHealth;

        if (healthBarImage.fillAmount < 1.0f && !healthBarCanvasGameObject.activeSelf)
        {
            healthBarCanvasGameObject.SetActive(true);
        }
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
