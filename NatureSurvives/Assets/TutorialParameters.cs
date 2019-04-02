using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialParameters : MonoBehaviour
{
    public enum TutorialState
    {
        E_ONE = 1,
        E_TWO = 2,
        E_THREE = 3
    }
    public TutorialState m_eTutorialLevel;
    // Start is called before the first frame update
    void Start()
    {
        m_eTutorialLevel = TutorialState.E_ONE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_eTutorialLevel)
        {
            //checking if the player has selected all 3 wongles
            case TutorialState.E_ONE:
                {
                    int amountSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if(temp[i].GetComponent<SelectableUnitComponent>().isSelected)
                        {
                            amountSelected++;
                        }
                    }
                    if(amountSelected >= 3)
                    {
                        //change ui to completed version (maybe tick boxes)
                        m_eTutorialLevel = TutorialState.E_TWO;
                    }
                    break;
                }
            case TutorialState.E_TWO:
                {
                    if(HouseController.WhiteAmount >= 200 && HouseController.WoodAmount >= 250 && HouseController.CrystalAmount >= 250)
                    {
                        //spawn enemy
                        //tick ui box
                        m_eTutorialLevel = TutorialState.E_THREE;
                    }
                    break;
                }
            case TutorialState.E_THREE:
                {
                    //if enemy == null
                    //show some "you finished tutorial" garbage and then send them to main level
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
