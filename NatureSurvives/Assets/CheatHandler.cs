using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatHandler : MonoBehaviour
{
    private bool m_bPlayerIsEnteringCheat;
    public GameObject m_goEntryField;
    // Start is called before the first frame update
    void Start()
    {
        m_bPlayerIsEnteringCheat = false;
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.GetComponent<InputField>().DeactivateInputField();
    }

    /// <summary>
    /// Basis is to have the same number characters per cheat so we can always determine whether they've entered a cheat 
    /// </summary>
    /// <param name="_sEnteredText"></param>

    public void HandleEnteredCheat(string _sEnteredText)
    {
        switch(_sEnteredText)
        {
            case "BYEBYEBOXES":
            case "byebyeboxes":
            case "ByeByeBoxes":
            case "Byebyeboxes":
                {
                    if (GameObject.FindGameObjectWithTag("Fog").gameObject != null)
                        GameObject.FindGameObjectWithTag("Fog").gameObject.SetActive(false);
                    break;
                }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            switch(m_bPlayerIsEnteringCheat)
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
            if (gameObject.GetComponent<InputField>().text.Length == 11)
            {
                HandleEnteredCheat(gameObject.GetComponent<InputField>().text);
                m_bPlayerIsEnteringCheat = false;
                gameObject.GetComponent<CanvasGroup>().alpha = 0;
                gameObject.GetComponent<InputField>().DeactivateInputField();
            }
        }
    }
}
