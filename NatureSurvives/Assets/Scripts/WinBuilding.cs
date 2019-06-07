using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinBuilding : MonoBehaviour {

    public float endtimer;
    public float buildingHealth;
    public Text winlosetext;

    // Use this for initialization
    void Start() {
        GameObject.FindGameObjectWithTag("WonderUI2").transform.GetChild(0).gameObject.SetActive(true);
        GameObject temp = GameObject.FindGameObjectWithTag("WonderUi").GetComponent<UpdateWonderUi>().wonder = gameObject;
        GameObject.FindGameObjectWithTag("DAYNIGHT").GetComponent<DayNight>().WonderMode();
        print("wonder mode start");

        if (GameObject.Find("WinLoseText") != null)
        {
            winlosetext = GameObject.Find("WinLoseText").GetComponent<Text>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        endtimer -= Time.deltaTime;
        if(endtimer <= 0)
        {
            winlosetext.text = "You Win";

            if (endtimer <= -2)
            {
                SceneManager.LoadScene(0);
            }
            
            //win game
            print("win game");
        }
		if(buildingHealth <= 0)
        {
            winlosetext.text = "You Lose";
            new WaitForSeconds(5);
            //lose game
        }
	}

    // this method is unused? But uses take damage on player building script instead?  
    // player building health = 50
    // win building health = 200
    void buildingDamaged(float damage)
    {
        buildingHealth -= damage;
    }

}
