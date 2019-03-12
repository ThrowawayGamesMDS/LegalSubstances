using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatHandler : MonoBehaviour
{
    private bool m_bPlayerIsEnteringCheat;
    public GameObject m_goEntryField;
    private string[] m_sKnownCheats;
    private int m_iCheatCount;
    // Start is called before the first frame update
    void Start()
    {
        m_sKnownCheats = new string[2];
        m_sKnownCheats[0] = "byebyeboxes";
        m_sKnownCheats[1] = "givemedosh";
        m_iCheatCount = 2;
        m_bPlayerIsEnteringCheat = false;
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.GetComponent<InputField>().DeactivateInputField();
    }

    public void HandleEnteredCheat(string _sEnteredText)
    {
        _sEnteredText.ToLower();
        switch (_sEnteredText)
        {
            case "byebyeboxes":
                {
                    if (GameObject.FindGameObjectWithTag("Fog").gameObject != null)
                        GameObject.FindGameObjectWithTag("Fog").gameObject.SetActive(false);
                    break;
                }
            case "givemedosh":
                {
                    if (GameObject.FindGameObjectWithTag("Player").gameObject != null)
                    {
                        HouseController.WhiteAmount += 1000;
                        HouseController.CrystalAmount += 1000;
                        HouseController.WoodAmount += 1000;
                    }
                        break;
                }

        }
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
                    HandleEnteredCheat(gameObject.GetComponent<InputField>().text);
                    m_bPlayerIsEnteringCheat = false;
                    gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    gameObject.GetComponent<InputField>().DeactivateInputField();
                }
            }
        }
    }
}
