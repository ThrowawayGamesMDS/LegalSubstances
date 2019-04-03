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

    // Use this for initialization
    void Start ()
    {
        startHealth = BuildingHealth;

        healthBarCanvasTransform = transform.Find("Health Bar");
        healthBarImage = healthBarCanvasTransform.GetChild(0).GetChild(0).GetComponent<Image>();
        healthBarCanvasGameObject = healthBarCanvasTransform.gameObject;
        healthBarCanvasGameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (BuildingHealth <= 0)
        {
            if(isWonder)
            {
                SceneManager.LoadScene(0);
            }
            NotificationManager.Instance.SetNewNotification(gameObject.name + " destroyed");

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        BuildingHealth -= damage;

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
}
