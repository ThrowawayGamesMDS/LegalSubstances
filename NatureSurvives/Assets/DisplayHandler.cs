using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/***
 * 
 * Created by Benjamin Pointer
 *  Uses SelectionBox.cs
 * 
 ***/

public class DisplayHandler : MonoBehaviour
{
    public static DisplayHandler m_sDHControl;

    public int m_iTotalNumber;
    public SelectableObject select;
    public bool m_bDisplayingUnit;
    public bool m_bDisplayingBuildings;

    public List<GameObject> m_goUIObj;


    /// <summary>
    /// ASSIGNING TEXT AT RUNTIME SO CANVAS CAN BE IMPLEMENTD EASILY...
    /// </summary>
    private List<Text> m_tWongleWorkerText;
    private enum Text_Tags { Overall_Level, WC_Level,Fish_Level,Mine_Level,Farm_Level, Trees_Cut, Fish_Caught, Rocks_Mined, Farms_Hoed };
    private List<string> m_lsBaseText;
    private Text_Tags m_ttCurrentTag;
    private GameObject m_goTextGroup;
    private bool m_bDisplayingText;
    private List<int> m_liTextMemory; //for update if certain stats have changed when a user is say watching a wongle worker
    private GameObject m_goSelected; // this will be replaced by the controlhandler's selectd obj.

    private void Awake()
    {
        if (DisplayHandler.m_sDHControl == null)
        {
            DisplayHandler.m_sDHControl = this;
            for (int i = 0; i < m_goUIObj.Count; i++)
            {
                if (m_goUIObj[i] != null)
                {
                    m_goUIObj[i].SetActive(false);
                }
            }

            m_liTextMemory = new List<int>();


            m_tWongleWorkerText = new List<Text>();
            m_lsBaseText = new List<string>();

            m_ttCurrentTag = ReturnNewTag(0);
            for (int i = 0; i < 9; i++)
            {
                m_tWongleWorkerText.Add(GameObject.FindGameObjectWithTag(m_ttCurrentTag.ToString()).GetComponent<Text>());
                m_ttCurrentTag = ReturnNewTag(i + 1);
            }
            m_goTextGroup = GameObject.FindGameObjectWithTag("WorkerInfoTextGroup");
            m_goTextGroup.GetComponent<CanvasGroup>().alpha = 0;
            m_bDisplayingText = false;
            m_bDisplayingUnit = false;
            m_bDisplayingBuildings = false;
        }
    }

    private Text_Tags ReturnNewTag(int _tagVal)
    {
        Text_Tags _newTag = Text_Tags.Overall_Level; // default;
        switch(_tagVal)
        {
            case 0:
                _newTag = Text_Tags.Overall_Level;
                m_lsBaseText.Add("Worker Level: ");
                m_liTextMemory.Add(0);
                break;
            case 1:
                _newTag = Text_Tags.WC_Level;
                m_lsBaseText.Add("Woodcut Level: ");
                m_liTextMemory.Add(0);
                break;
            case 2:
                _newTag = Text_Tags.Fish_Level;
                m_lsBaseText.Add("Fishing Level: ");
                m_liTextMemory.Add(0);
                break;
            case 3:
                _newTag = Text_Tags.Mine_Level;
                m_lsBaseText.Add("Mining Level: ");
                m_liTextMemory.Add(0);
                break;
            case 4:
                _newTag = Text_Tags.Farm_Level;
                m_lsBaseText.Add("Farming Level: ");
                m_liTextMemory.Add(0);
                break;
            case 5:
                _newTag = Text_Tags.Trees_Cut;
                m_lsBaseText.Add("Logs Gathered: ");
                m_liTextMemory.Add(0);
                break;
            case 6:
                _newTag = Text_Tags.Fish_Caught;
                m_lsBaseText.Add("Fish Gathered: ");
                m_liTextMemory.Add(0);
                break;
            case 7:
                _newTag = Text_Tags.Rocks_Mined;
                m_lsBaseText.Add("Ore Gathered: ");
                m_liTextMemory.Add(0);
                break;
            case 8:
                _newTag = Text_Tags.Farms_Hoed;
                m_lsBaseText.Add("Wheat Gathered: ");
                m_liTextMemory.Add(0);
                break;
            default:
                break;
        }
        return _newTag;
    }

    public void UpdateAndDisplayWongleWorkerText(GameObject _selectedWongle)
    {
        if (m_goTextGroup.GetComponent<CanvasGroup>().alpha != 1)
            m_goTextGroup.GetComponent<CanvasGroup>().alpha = 1;

        if (!m_bDisplayingText)
            m_bDisplayingText = true;

        m_goSelected = _selectedWongle;

        print("WongleWorkerText Count: " + m_tWongleWorkerText.Count + ", BaseText Count: " + m_lsBaseText.Count);
        for (int i = 0; i < m_tWongleWorkerText.Count; i++)
        {
            m_tWongleWorkerText[i].text = m_lsBaseText[i];
            if (i == 0) // overall
            { 
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iOverallLevel;
                m_liTextMemory[i] = _selectedWongle.GetComponent<WongleController>().iOverallLevel;
            }
            else if ( i == 1)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iWoodCutLevel;
                m_liTextMemory[i] = _selectedWongle.GetComponent<WongleController>().iWoodCutLevel;
            }
            else if (i == 2)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iFishingLevel;
                m_liTextMemory[i] = _selectedWongle.GetComponent<WongleController>().iFishingLevel;
            }
            else if (i == 3)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iMineLevel;
                m_liTextMemory[i] = _selectedWongle.GetComponent<WongleController>().iMineLevel;
            }
            else if (i == 4)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iFarmLevel;
                m_liTextMemory[i] = _selectedWongle.GetComponent<WongleController>().iFarmLevel;
            }
            else if (i == 5)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iTreesCut;
                m_liTextMemory[i] = (int)_selectedWongle.GetComponent<WongleController>().iTreesCut;
            }
            else if (i == 6)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iFishCaught;
                m_liTextMemory[i] = (int)_selectedWongle.GetComponent<WongleController>().iFishCaught;
            }
            else if (i == 7)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iRocksMined;
                m_liTextMemory[i] = (int)_selectedWongle.GetComponent<WongleController>().iRocksMined;
            }
            else if (i == 8)
            {
                m_tWongleWorkerText[i].text += _selectedWongle.GetComponent<WongleController>().iFarmsHarvested;
                m_liTextMemory[i] = (int)_selectedWongle.GetComponent<WongleController>().iFarmsHarvested;
            }
        }
    }

    private void TextUpdateHandle()
    {
        for (int i = 0; i < m_tWongleWorkerText.Count; i++)
        {
            if (i == 0) // overall
            {
                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iOverallLevel)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iOverallLevel;
                    m_liTextMemory[i] = m_goSelected.GetComponent<WongleController>().iOverallLevel;
                }
            }
            else if (i == 1)
            {

                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iWoodCutLevel)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iWoodCutLevel;
                    m_liTextMemory[i] = m_goSelected.GetComponent<WongleController>().iWoodCutLevel;
                }
            }
            else if (i == 2)
            {

                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iFishingLevel)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iFishingLevel;
                    m_liTextMemory[i] = m_goSelected.GetComponent<WongleController>().iFishingLevel;
                }
            }
            else if (i == 3)
            {
                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iMineLevel)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iMineLevel;
                    m_liTextMemory[i] = m_goSelected.GetComponent<WongleController>().iMineLevel;
                }
            }
            else if (i == 4)
            {

                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iFarmLevel)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iFarmLevel;
                    m_liTextMemory[i] = m_goSelected.GetComponent<WongleController>().iFarmLevel;
                }
            }
            else if (i == 5)
            {
                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iTreesCut)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iTreesCut;
                    m_liTextMemory[i] = (int)m_goSelected.GetComponent<WongleController>().iTreesCut;
                }
            }
            else if (i == 6)
            {

                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iFishCaught)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iFishCaught;
                    m_liTextMemory[i] = (int)m_goSelected.GetComponent<WongleController>().iFishCaught;
                }
            }
            else if (i == 7)
            {
                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iRocksMined)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iRocksMined;
                    m_liTextMemory[i] = (int)m_goSelected.GetComponent<WongleController>().iRocksMined;
                }
            }
            else if (i == 8)
            {
                if (m_liTextMemory[i] != m_goSelected.GetComponent<WongleController>().iFarmsHarvested)
                {
                    m_tWongleWorkerText[i].text = m_lsBaseText[i];
                    m_tWongleWorkerText[i].text += m_goSelected.GetComponent<WongleController>().iFarmsHarvested;
                    m_liTextMemory[i] = (int)m_goSelected.GetComponent<WongleController>().iFarmsHarvested;
                }
            }
        }
    }

    private bool GenerateRayCast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            print(hit.transform.name);
            if (hit.transform.gameObject.tag == "HomeBuilding" && !m_bDisplayingUnit)
            {
                print("set build options");
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateState(false);
                    return true;
                }
                
            }
            else if (hit.transform.gameObject.tag != "HomeBuilding" && m_bDisplayingUnit)
            {
                print("reset build options");
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateState(false);
                    return true;
                }
            }
        }
        return false;
    }

    public void UpdateState(bool _bWorker)
    {
        if (m_bDisplayingText)
        {
            if (m_goTextGroup.GetComponent<CanvasGroup>().alpha != 0)
                m_goTextGroup.GetComponent<CanvasGroup>().alpha = 0;
        }

        switch (_bWorker)
        {
            case true:
                {
                    switch(m_bDisplayingBuildings)
                    {
                        case true:
                            {
                                m_bDisplayingBuildings = false;
                                m_goUIObj[1].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                                break;
                            }
                        case false:
                            {
                                if (m_bDisplayingUnit)
                                {
                                    m_bDisplayingUnit = false;
                                    m_goUIObj[0].SetActive(false); 
                                }
                                m_bDisplayingBuildings = true;
                                m_goUIObj[1].SetActive(true); // m_goUiOBJ[1] = BuildOptions 
                                break;
                            }
                    }
                    break;
                }
            case false:
                {
                    switch (m_bDisplayingUnit)
                    {
                        case true:
                            {
                                m_bDisplayingUnit = false;
                                m_goUIObj[0].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                                print("reset unit display");
                                break;
                            }
                        case false:
                            {
                                if (m_bDisplayingBuildings)
                                {
                                    m_bDisplayingBuildings = false;
                                    m_goUIObj[1].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                                }
                                m_bDisplayingUnit = true;
                                m_goUIObj[0].SetActive(true); // m_goUiOBJ[1] = BuildOptions 
                                print("set unit display");
                                break;
                            }
                    }
                    break;
                }
        }
    }


    private void Update()
    {
        if (m_bDisplayingText)
            TextUpdateHandle();

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //  int layermask = LayerMask.NameToLayer("");
            GenerateRayCast(ray);
            /*if ( !GenerateRayCast(ray))
            {
                if (gameObject.GetComponent<SelectionBox>().m_goSelectOBJ != null && !m_bDisplayingBuildings)
                {
                    if (m_bDisplayingUnit)
                    {
                        m_bDisplayingUnit = false;
                        m_goUIObj[0].SetActive(false);
                    }
                    UpdateState(true);
                }
            }*/

        }
    }
}
