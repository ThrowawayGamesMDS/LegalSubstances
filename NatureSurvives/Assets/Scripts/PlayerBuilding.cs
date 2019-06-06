using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBuilding : MonoBehaviour {
    public float BuildingHealth;
    public bool isWonder;

    [Header("Health Bar")]
    private Image healthBarImage;
    private float startHealth;
    private Transform healthBarCanvasTransform;
    public GameObject healthBarCanvasGameObject;
    public bool isHouse;

    float speed = 100.0f; //how fast it shakes
    float amount = 0.5f; //how much it shakes
    Vector3 startPosition;
    float ShakeTime = 0.2f;
    float counter = 0.0f;
    bool isShaking = false;

    public GameObject fireEffect;
    public GameObject attackEffect;
    public GameObject placedEffect;
    public GameObject fire;
    public float height;

    // Use this for initialization
    void Start ()
    {
        startHealth = BuildingHealth;
        if (healthBarCanvasTransform != null)
        {
            healthBarCanvasTransform = transform.Find("Health Bar");
            healthBarImage = healthBarCanvasTransform.GetChild(0).GetChild(0).GetComponent<Image>();
            healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
            healthBarCanvasGameObject.SetActive(false);
        }

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Vector3 vec = new Vector3(transform.position.x, 1, transform.position.z);

        if (placedEffect != null)
        {
            Instantiate(placedEffect, vec, placedEffect.transform.rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10);
        }


        if (BuildingHealth <= 0)
        {
            //Vector3 vec = new Vector3(transform.position.x, height, transform.position.z);
            //Instantiate(attackEffect, vec, attackEffect.transform.rotation);

            if (isWonder)
            {
                SceneManager.LoadScene(0);
            }
            NotificationManager.Instance.SetNewNotification(gameObject.name + " destroyed");
            if (isHouse)
            {
                HomeSpawning home = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>();
                home.iMaximumWongleCount -= 5;
            }
            Destroy(gameObject);
        }

        if(BuildingHealth < (startHealth/2))
        {
            if (fire == null)
            {
                Vector3 vec = new Vector3(gameObject.transform.position.x, height, gameObject.transform.position.z);
                fire = Instantiate(fireEffect, vec, fireEffect.transform.rotation, transform);
            }
        }
        
        if (isShaking)
        {
            counter += Time.deltaTime;
            if (counter >= ShakeTime)
            {
                isShaking = false;
                counter = 0;
            }
            else
            {
                transform.position = new Vector3(startPosition.x + Mathf.Sin(Time.time * speed) * amount, startPosition.y, startPosition.z + Mathf.Sin(Time.time * speed) * amount);
            }
        }
        
          
    }

    public void TakeDamage(int damage)
    {
        
        print("damage");

        BuildingHealth -= damage;
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = BuildingHealth / startHealth;

            if (healthBarImage.fillAmount < 1.0f && !healthBarCanvasGameObject.activeSelf)
            {
                healthBarImage.GetComponent<Image>().color = Color.green;
                healthBarCanvasGameObject.SetActive(true);
            }

            if (healthBarImage.fillAmount < 0.5)
            {
                if (healthBarImage.GetComponent<Image>().color == Color.green)
                {
                    healthBarImage.GetComponent<Image>().color = Color.red;
                }
                else
                {
                    healthBarImage.GetComponent<Image>().color = Color.green;
                }

            }
        }
        isShaking = true;
    }
}
