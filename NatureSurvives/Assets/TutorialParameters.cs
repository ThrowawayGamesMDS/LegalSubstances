using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialParameters : MonoBehaviour
{
    //select the scout
    //send it into the fog
    //send a wongle to a crystal
    //send a wongle to a tree
    //send a wongle to the farm
    //build a defence tower
    //create a wizard
    //create a knight
    //sellect wizard and knight with box
    //send to kill enemy



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
    public GameObject scout;

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
                    //NotificationManager.Instance.SetNewNotification("Day time: During the day, you are safe to build things without being attacked by enemies.");

                    int amountSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].GetComponent<SelectableUnitComponent>().isSelected && temp[i].GetComponent<SelectableUnitComponent>().Type == SelectableUnitComponent.workerType.Scout)
                        {
                            amountSelected++;
                            scout = temp[i];
                        }
                    }
                    if (amountSelected > 0)
                    {
                        //change ui to completed version (maybe tick boxes)
                        UIq1.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_TWO;
                        m_goVideoPlayer.clip = m_lVideoClips[1];
                        Vector3 _newPos = new Vector3(140, Camera.main.transform.position.y, -20 - (Camera.main.transform.forward.magnitude * 50));
                        cameraController.m_sCameraControl.m_fTransitionSpeed = 0.01f;
                        cameraController.m_sCameraControl.UpdateCameraTargetForLerping(_newPos);
                        NotificationManager.Instance.SetNewNotification("send the scout out into the fog by right clicking");
                    }
                    break;
                }
            case TutorialState.E_TWO:
                {
                    //NotificationManager.Instance.SetNewNotification("Left click on one wongle to select, then right click on the tree, farm or crystal.");

                    //if (HouseController.m_iFoodCount >= 170 && HouseController.WoodAmount >= 220 && HouseController.CrystalAmount >= 220)
                    //if (HouseController.m_iFoodCount >= 240 && HouseController.WoodAmount >= 100 && HouseController.CrystalAmount >= 260)
                    //{
                    //    //spawn enemy
                    //    //tick ui box
                    //    UIq2.color = Color.green;
                    //    m_eTutorialLevel = TutorialState.E_THREE;
                    //
                    //    m_goVideoPlayer.clip = m_lVideoClips[2];
                    //}


                    ///if the scout is close to 140,0,-20
                    if(Vector3.Distance(scout.transform.position, new Vector3(140, 0, -20)) < 15)
                    {
                        UIq2.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_THREE;                        
                        m_goVideoPlayer.clip = m_lVideoClips[2];
                    }
                    break;
                }
            case TutorialState.E_THREE:
                {
                    NotificationManager.Instance.SetNewNotification("Send the wongles to collect resourcesby left clicking them to select then and then right clicking on the resource.");
                    if (HouseController.m_iFoodCount >= 240 && HouseController.WoodAmount >= 100 && HouseController.CrystalAmount >= 260)
                    {
                        UIq3.color = Color.green;
                        m_eTutorialLevel = TutorialState.E_FOUR;
                        m_goVideoPlayer.clip = m_lVideoClips[3];
                    }






                        //NotificationManager.Instance.SetNewNotification("Click on the house. Then click on the knights and wizards on the bottom left corner of the screen.");
                        //
                        //if (GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goRangedUnits.Count >= 4 && GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goMeleeUnits.Count >= 4)
                        //{
                        //    UIq3.color = Color.green;
                        //    m_eTutorialLevel = TutorialState.E_FOUR;
                        //    enemyInstance = Instantiate(Enemy, new Vector3(0, 0, 60), Quaternion.Euler(0, 90, 0));
                        //    Vector3 _newPos = new Vector3(enemyInstance.transform.position.x, Camera.main.transform.position.y, enemyInstance.transform.position.z - (Camera.main.transform.forward.magnitude * 50));
                        //    cameraController.m_sCameraControl.m_fTransitionSpeed = 0.01f;
                        //    cameraController.m_sCameraControl.UpdateCameraTargetForLerping(_newPos);
                        //    m_goVideoPlayer.clip = m_lVideoClips[3];
                        //}
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
