using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerInfoUI : MonoBehaviour
{
    public static string[] lines;
    public WongleController FocusTarget;
    public GameObject uiPanel;
    public Text uiName, uiTask;
    public Image healthbar;
    // Start is called before the first frame update
    void Awake()
    {
        //list of names stored here
        string path = "Assets/Resources/names.txt";
        lines = System.IO.File.ReadAllLines(path);

        if (uiPanel == null)
        {
            //Destroy(this);
        }
    }
    private void Update()
    {
        int amountselected = 0;
        GameObject tempGO = null;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Wongle");
        for(int i = 0; i < temp.Length; i++)
        {
            if(temp[i].GetComponent<SelectableUnitComponent>().isSelected)
            {
                amountselected++;
                tempGO = temp[i];
            }
        }
        if(amountselected == 1)
        {
            if(tempGO != null)
            {
                FocusTarget = tempGO.GetComponent<WongleController>();
            }
        }
        else
        {
            FocusTarget = null;
        }


        if(FocusTarget == null)
        {
            uiPanel.SetActive(false);
        }
        else
        {
            uiPanel.SetActive(true);
            uiName.text = FocusTarget.WongleName;
            healthbar.fillAmount = FocusTarget.WongleHealth / FocusTarget.startHealth;
            if(FocusTarget.Work != null)
            {
                switch (FocusTarget.Work.tag)
                {
                    case "Building":
                        {
                            uiTask.text = "Farming for mushrooms";
                            break;
                        }
                    case "Army":
                        {
                            if (FocusTarget.type == SelectableUnitComponent.workerType.Ranged)
                            {
                                uiTask.text = "Thinking about wizard stuff";
                            }
                            else if (FocusTarget.type == SelectableUnitComponent.workerType.Melee)
                            {
                                uiTask.text = "Daydreaming about violence lovingly";
                            }
                            break;
                        }
                    case "Miner":
                        {
                            uiTask.text = "Hitting crystals";
                            break;
                        }
                    case "WoodCutter":
                        {
                            uiTask.text = "Choppin' trees";
                            break;
                        }
                    case "Builder":
                        {
                            uiTask.text = "Constructin' stuff";
                            break;
                        }
                    default:
                        {
                            uiTask.text = "Doing wongle things";
                            break;
                        }
                }
            }
            else
            {
                uiTask.text = "Doing wongle things";
            }
            
        }
    }
}
