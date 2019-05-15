using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
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
    public List<VideoClip> m_lVideoClips;
    public VideoPlayer m_goVideoPlayer;
    

    // Start is called before the first frame update
    void Start()
    {
        m_eTutorialLevel = TutorialState.E_ONE;
        print(gameObject.name);       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(2);
        }
        switch (m_eTutorialLevel)
        {
            //checking if the player has selected all 3 wongles
            case TutorialState.E_ONE:
                {
                    NotificationManager.Instance.SetNewNotification("Day time: During the day, you are safe to build things without being attacked by enemies.");

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
                        //change ui to completed version (maybe tick boxes)
                        UIq1.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_TWO;
                        m_goVideoPlayer.clip = m_lVideoClips[1];
                    }
                    break;
                }
            case TutorialState.E_TWO:
                {
                    NotificationManager.Instance.SetNewNotification("Left click on one wongle to select, then right click on the tree, farm or crystal.");

                    //if (HouseController.m_iFoodCount >= 170 && HouseController.WoodAmount >= 220 && HouseController.CrystalAmount >= 220)
                    if (HouseController.m_iFoodCount >= 240 && HouseController.WoodAmount >= 100 && HouseController.CrystalAmount >= 260)
                    {
                        //spawn enemy
                        //tick ui box
                        UIq2.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_THREE;

                        m_goVideoPlayer.clip = m_lVideoClips[2];
                    }
                    break;
                }
            case TutorialState.E_THREE:
                {
                    NotificationManager.Instance.SetNewNotification("Click on the house. Then click on the knights and wizards on the bottom left corner of the screen.");

                    if (GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goRangedUnits.Count >= 4 && GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goMeleeUnits.Count >= 4)
                    {
                        UIq3.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_FOUR;
                        enemyInstance = Instantiate(Enemy, new Vector3(0, 0, 60), Quaternion.Euler(0, 90, 0));
                        m_goVideoPlayer.clip = m_lVideoClips[3];
                    }
                    break;
                }
            case TutorialState.E_FOUR:
                {
                    GameObject.FindGameObjectWithTag("DAYNIGHT").GetComponent<DayNight>().ChangeToNight();

                    NotificationManager.Instance.SetNewNotification("Night time: During the night, the enemies will come.  Be prepared.");

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
