using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatHandler : MonoBehaviour
{
    public static CheatHandler m_sCheatHandler;
    public bool m_bPlayerIsEnteringCheat;
    public GameObject m_goEntryField;
    private string[] m_sKnownCheats;
    private int m_iCheatCount;
    public bool m_bDisabledBuildTimer;
    public bool m_bEnabledTimeIncrease;
    // Start is called before the first frame update
    void Start()
    {
        if (m_sCheatHandler == null)
        {
            m_sCheatHandler = this;
            m_iCheatCount = 6;
            m_sKnownCheats = new string[m_iCheatCount];
            m_sKnownCheats[0] = "byebyeboxes";
            m_sKnownCheats[1] = "givemedosh";
            m_sKnownCheats[2] = "builderpro";
            m_sKnownCheats[3] = "nomorequeues";
            m_sKnownCheats[4] = "boosttimepls";
            m_sKnownCheats[5] = "gimmiepower";
            m_bDisabledBuildTimer = false;
            m_bPlayerIsEnteringCheat = false;
            m_bEnabledTimeIncrease = false;
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
            gameObject.GetComponent<InputField>().DeactivateInputField();
        }
        else
            Destroy(this); //this = gameObject.component?
    }

    public void HandleEnteredCheat(string _sEnteredText)
    {
        string _sCheatResult = "Cheat ";
        switch (_sEnteredText)
        {
            case "byebyeboxes":
                {
                    if (GameObject.FindGameObjectWithTag("Fog").gameObject != null)
                        GameObject.FindGameObjectWithTag("Fog").gameObject.SetActive(false);
                    _sCheatResult += "Enabled: Remove Fog of War!";
                    break;
                }
            case "givemedosh":
                {
                    if (GameObject.FindGameObjectWithTag("Player").gameObject != null)
                    {
                        HouseController.m_iFoodCount += 1000;
                        HouseController.CrystalAmount += 1000;
                        HouseController.WoodAmount += 1000;
                        _sCheatResult += "Enabled: Give Resources!";
                    }
                        break;
                }
            case "builderpro":
                {
                    if (GameObject.FindGameObjectWithTag("Player").gameObject != null)
                    {
                        switch(m_bDisabledBuildTimer)
                        {
                            case true:
                                {
                                    m_bDisabledBuildTimer = false;
                                    _sCheatResult += "Disabled: Remove Build Timer!";
                                    break;
                                }
                            case false:
                                {
                                    m_bDisabledBuildTimer = true;
                                    _sCheatResult += "Enabled: Remove Build Timer!";
                                    break;
                                }
                        }
                    }
                    break;
                }
            case "nomorequeues":
                {
                    switch (HomeSpawning.m_sHomeSpawningControl.m_bRemoveQueueTimerCheat)
                    {
                        case false:
                            {
                                HomeSpawning.m_sHomeSpawningControl.m_bRemoveQueueTimerCheat = true;
                                _sCheatResult += "Enabled: Remove Queue Timer!";
                                break;
                            }
                        case true:
                            {
                                HomeSpawning.m_sHomeSpawningControl.m_bRemoveQueueTimerCheat = false;
                                _sCheatResult += "Disabled: Remove Queue Timer!";
                                break;
                            }
                    }
                    break;
                }
            case "boosttimepls":
                {
                    switch (m_bEnabledTimeIncrease)
                    {
                        case true:
                            {
                                Time.timeScale = 1.0f;
                                m_bEnabledTimeIncrease = false;
                                NotificationManager.Instance.AdjustFadeTime(m_bEnabledTimeIncrease);
                                break;
                            }
                        case false:
                            {
                                Time.timeScale = 3.0f;
                                m_bEnabledTimeIncrease = true;
                                NotificationManager.Instance.AdjustFadeTime(m_bEnabledTimeIncrease);
                                NotificationManager.Instance.SetWarningNotification("Time-Scale-Cheat speeds time up, game events will occur faster!");
                                break;
                            }
                    }
                    _sCheatResult += "Time-Scale-Cheat: " + m_bEnabledTimeIncrease;
                    break;
                }
            case "gimmiepower":
                {
                    break;
                }
        }
        if (_sEnteredText!= "boosttimepls")
            NotificationManager.Instance.SetNewNotification(_sCheatResult);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            gameObject.GetComponent<InputField>().text = "";
            switch (m_bPlayerIsEnteringCheat)
            {
                case false:
                    {
                        m_bPlayerIsEnteringCheat = true;
                        gameObject.GetComponent<CanvasGroup>().alpha = 1;
                        gameObject.GetComponent<InputField>().ActivateInputField();
                        break;
                    }
                case true:
                    {
                        m_bPlayerIsEnteringCheat = false;
                        gameObject.GetComponent<CanvasGroup>().alpha = 0;
                        gameObject.GetComponent<InputField>().DeactivateInputField();
                        break;
                    }
            }
        }

        if (m_bPlayerIsEnteringCheat)
        {
            //if (gameObject.GetComponent<InputField>().text.Length == 11)
            for (int i = 0; i < m_iCheatCount; i++)
            {
                if (gameObject.GetComponent<InputField>().text.ToLower() == m_sKnownCheats[i].ToLower())
                {
                    HandleEnteredCheat(gameObject.GetComponent<InputField>().text.ToLower());
                    m_bPlayerIsEnteringCheat = false;
                    gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    gameObject.GetComponent<InputField>().DeactivateInputField();
                }
            }
        }
    }
}
