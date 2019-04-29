using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialParameters : MonoBehaviour
{
    public enum TutorialState
    {
        E_ONE = 1,
        E_TWO = 2,
        E_THREE = 3,
        E_FOUR = 4
    }

    public TutorialState m_eTutorialLevel;
    public Text UIq1, UIq2, UIq3, UIq4;
    public GameObject Enemy;
    public GameObject enemyInstance;

    int startWhiteAmount;
    int startWoodAmount;
    int startCrystalAmount;

    // Start is called before the first frame update
    void Start()
    {
        m_eTutorialLevel = TutorialState.E_ONE;
        print(gameObject.name);       
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_eTutorialLevel)
        {
            //checking if the player has selected all 3 wongles
            case TutorialState.E_ONE:
                {
                    int amountSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].GetComponent<SelectableUnitComponent>().isSelected)
                        {
                            amountSelected++;
                        }
                    }
                    if (amountSelected >= 3)
                    {
                        startWhiteAmount = HouseController.WhiteAmount;
                        startWoodAmount = HouseController.WoodAmount;
                        startCrystalAmount = HouseController.CrystalAmount;

                        //change ui to completed version (maybe tick boxes)
                        UIq1.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_TWO;                      
                    }
                    break;
                }
            case TutorialState.E_TWO:
                {
                    //if (HouseController.WhiteAmount >= 170 && HouseController.WoodAmount >= 220 && HouseController.CrystalAmount >= 220)
                    if ( (HouseController.WhiteAmount >= startWhiteAmount + 50) && (HouseController.WoodAmount >= startWoodAmount + 50) && (HouseController.CrystalAmount >= startCrystalAmount + 50) )
                    {
                        //spawn enemy
                        //tick ui box
                        UIq2.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_THREE;
                    }
                    break;
                }
            case TutorialState.E_THREE:
                {
                    if (GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goRangedUnits.Count >= 4 && GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goMeleeUnits.Count >= 4)
                    {
                        UIq3.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_FOUR;
                        enemyInstance = Instantiate(Enemy, new Vector3(0, 0, 60), Quaternion.Euler(0, 90, 0));
                    }
                    break;
                }
            case TutorialState.E_FOUR:
                {
                    if (enemyInstance == null)
                    {
                        UIq4.color = Color.green;
                        Invoke("EndTutorial", 5);
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }



    public void EndTutorial()
    {
        SceneManager.LoadScene(2);
    }
}
