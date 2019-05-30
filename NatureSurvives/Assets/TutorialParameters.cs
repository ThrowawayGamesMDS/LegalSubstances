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
        E_FOUR = 4,
        E_FIVE = 5,
        E_SIX = 6,
        E_SEVEN = 7,
        E_EIGHT = 8,
        E_NINE = 9,
        E_TEN = 10
    }

    public TutorialState m_eTutorialLevel;
    public Text UIq1;
    public GameObject Enemy;
    public List<VideoClip> m_lVideoClips;
    public VideoPlayer m_goVideoPlayer;
    public GameObject scout;
    public List<string> m_lsQuestTexts;
    
    // Start is called before the first frame update
    void Start()
    {
        m_eTutorialLevel = TutorialState.E_ONE;

        m_goVideoPlayer.clip = m_lVideoClips[0];
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
            //select the scout
            case TutorialState.E_ONE:
                {
                    NotificationManager.Instance.SetNewNotification("The Scout is the little purple guy on the left, you can select him by left clicking him or holding the left mouse button down and dragging to crerate a box");

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
                        //play sound
                        UIq1.text = m_lsQuestTexts[1];
                        m_eTutorialLevel = TutorialState.E_TWO;
                        m_goVideoPlayer.clip = m_lVideoClips[1];
                        Vector3 _newPos = new Vector3(140, Camera.main.transform.position.y, -20 - (Camera.main.transform.forward.magnitude * 50));
                        cameraController.m_sCameraControl.m_fTransitionSpeed = 0.01f;
                        //cameraController.m_sCameraControl.UpdateCameraTargetForLerping(_newPos);
                        NotificationManager.Instance.SetNewNotification("Send the scout to the this location by right clicking on the black area, send the scout to the edge of the map on the left.");
                    }
                    break;
                }
            //if the scout is close to 140,0,-20
            case TutorialState.E_TWO:
                {
                    if(Vector3.Distance(scout.transform.position, new Vector3(140, 0, -20)) < 15)
                    {
                        UIq1.text = m_lsQuestTexts[2];
                        m_eTutorialLevel = TutorialState.E_THREE;                        
                        m_goVideoPlayer.clip = m_lVideoClips[2];
                        Vector3 _newPos = new Vector3(0, Camera.main.transform.position.y, -25 - (Camera.main.transform.forward.magnitude * 50));
                        cameraController.m_sCameraControl.m_fTransitionSpeed = 0.02f;
                        //cameraController.m_sCameraControl.UpdateCameraTargetForLerping(_newPos);
                        NotificationManager.Instance.SetNewNotification("Select a wongle and right click the farm to assign the wongle to the farm.");
                    }
                    break;
                }
            //send a wongle to a farm
            case TutorialState.E_THREE:
                {
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if(temp[i].GetComponent<WongleController>().Work != null)
                        {
                            if (temp[i].GetComponent<WongleController>().Work.tag == "Building")
                            {
                                UIq1.text = m_lsQuestTexts[3];
                                m_eTutorialLevel = TutorialState.E_FOUR;
                                m_goVideoPlayer.clip = m_lVideoClips[3];
                                NotificationManager.Instance.SetNewNotification("Select a wongle and right click a tree to make the wongle become a woodcutter.");
                            }
                        }                        
                    }
                    break;
                }
            //send a wongle to a tree
            case TutorialState.E_FOUR:
                {
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if(temp[i].GetComponent<WongleController>().Target != null)
                        {
                            if (temp[i].GetComponent<WongleController>().Target.tag == "Wood")
                            {
                                UIq1.text = m_lsQuestTexts[4];
                                m_eTutorialLevel = TutorialState.E_FIVE;
                                m_goVideoPlayer.clip = m_lVideoClips[4];
                                NotificationManager.Instance.SetNewNotification("Select a wongle and right click a crystal to make the wongle become a miner.");
                            }
                        }
                        
                    }
                    break;                    
                }
            //send a wongle to a crystal
            case TutorialState.E_FIVE:
                {
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].GetComponent<WongleController>().Target != null)
                        {
                            if (temp[i].GetComponent<WongleController>().Target.tag == "Crystal")
                            {
                                UIq1.text = m_lsQuestTexts[5];
                                m_eTutorialLevel = TutorialState.E_SIX;
                                m_goVideoPlayer.clip = m_lVideoClips[4];
                                NotificationManager.Instance.SetNewNotification("Select the last wongle and click the tower button in the bottom left corner of the screen and place the turret in the world, the wongle will automatically start building it.");
                            }
                        }
                    }
                    break;
                }
            //build a defence tower
            case TutorialState.E_SIX:
                {

                    int amountSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Turret");
                    
                    if (temp.Length > 0)
                    {
                        //change ui to completed version (maybe tick boxes)
                        UIq1.text = m_lsQuestTexts[6];
                        m_eTutorialLevel = TutorialState.E_SEVEN;
                        m_goVideoPlayer.clip = m_lVideoClips[6];
                        NotificationManager.Instance.SetNewNotification("Create a wizard by clicking the main house in the middle of the map and clicking the wizard button in the bottom left");
                    }
                    break;

                }
            //create a wizard
            case TutorialState.E_SEVEN:
                {

                    int amountSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].GetComponent<SelectableUnitComponent>().Type == SelectableUnitComponent.workerType.Ranged)
                        {
                            amountSelected++;
                        }
                    }
                    if (amountSelected > 0)
                    {
                        //change ui to completed version (maybe tick boxes)
                        UIq1.text = m_lsQuestTexts[7];
                        m_eTutorialLevel = TutorialState.E_EIGHT;
                        m_goVideoPlayer.clip = m_lVideoClips[7];
                        NotificationManager.Instance.SetNewNotification("Create a wizard by clicking the main house in the middle of the map and clicking the wizard button in the bottom left");
                    }
                    break;
                }
            //create a knight
            case TutorialState.E_EIGHT:
                {
                    //NotificationManager.Instance.SetNewNotification("Day time: During the day, you are safe to build things without being attacked by enemies.");

                    int amountSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].GetComponent<SelectableUnitComponent>().Type == SelectableUnitComponent.workerType.Melee)
                        {
                            amountSelected++;
                        }
                    }
                    if (amountSelected > 0)
                    {
                        //change ui to completed version (maybe tick boxes)
                        UIq1.text = m_lsQuestTexts[8];
                        m_eTutorialLevel = TutorialState.E_NINE;
                        m_goVideoPlayer.clip = m_lVideoClips[8];
                        NotificationManager.Instance.SetNewNotification("Select both the Knight and the wizard by clicking and dragging over them");
                    }
                    break;
                }
            //select the wizard and knight
            case TutorialState.E_NINE:
                {
                    //NotificationManager.Instance.SetNewNotification("Day time: During the day, you are safe to build things without being attacked by enemies.");

                    int MeleeSelected = 0;
                    int RangedSelected = 0;
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].GetComponent<SelectableUnitComponent>().isSelected && temp[i].GetComponent<SelectableUnitComponent>().Type == SelectableUnitComponent.workerType.Melee)
                        {
                            MeleeSelected++;
                        }
                        if (temp[i].GetComponent<SelectableUnitComponent>().isSelected && temp[i].GetComponent<SelectableUnitComponent>().Type == SelectableUnitComponent.workerType.Ranged)
                        {
                            RangedSelected++;
                        }
                    }
                    if (MeleeSelected > 0 && RangedSelected > 0)
                    {
                        //change ui to completed version (maybe tick boxes)
                        UIq1.text = m_lsQuestTexts[9];
                        m_eTutorialLevel = TutorialState.E_TEN;
                        m_goVideoPlayer.clip = m_lVideoClips[9];
                        //spawn enemy
                        Vector3 _newPos = new Vector3(140, Camera.main.transform.position.y, -10 - (Camera.main.transform.forward.magnitude * 50));
                        cameraController.m_sCameraControl.m_fTransitionSpeed = 0.01f;
                        //cameraController.m_sCameraControl.UpdateCameraTargetForLerping(Enemy.transform.position);
                        Enemy.GetComponent<BroodShroomController>().Unfog();
                        NotificationManager.Instance.SetNewNotification("Right click on the enemy to make the selected units attack it");
                    }
                    break;
                }
            //kill da enemy
            case TutorialState.E_TEN:
                {
                    
                    if (Enemy == null)
                    {
                        //change ui to completed version (maybe tick boxes)
                        UIq1.color = Color.green;
                        Invoke("EndTutorial", 4);
                        NotificationManager.Instance.SetNewNotification("You have completed the tutorial!");
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
