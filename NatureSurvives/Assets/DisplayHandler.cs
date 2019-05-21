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

public class Buttons
{
    public GameObject m_goButton;
    public Sprite m_sAvailableSprite;
    public Sprite m_sUnavailableSprite;
    bool m_bIsAvailable;
    public string m_sButtonID;

    public Buttons(GameObject _goButton, Sprite _sUnavailableSprite, string _sName)
    {
        m_goButton = _goButton;
        m_sUnavailableSprite = _sUnavailableSprite;
        m_sAvailableSprite = _goButton.GetComponent<Image>().sprite;
        m_sButtonID = _sName;
        m_bIsAvailable = true; // displaying available
    }

    public void UpdateSprite(bool _bPlayerCanAfford)
    {
        switch(_bPlayerCanAfford)
        {
            case true:
                {
                    if (!m_bIsAvailable)
                    {
                        m_goButton.GetComponent<Image>().sprite = m_sAvailableSprite;
                        m_bIsAvailable = true;
                    }
                    break;
                }
            case false:
                {
                    if (m_bIsAvailable)
                    {
                        m_goButton.GetComponent<Image>().sprite = m_sUnavailableSprite;
                        m_bIsAvailable = false;
                    }
                    break;
                }
        }
    }
}

public class DisplayHandler : MonoBehaviour
{
    public static DisplayHandler m_sDHControl;

    public int m_iTotalNumber;
    public SelectableObject select;
    public bool m_bDisplayingUnit;
    public bool m_bDisplayingBuildings;

    public List<GameObject> m_goUIObj;
    public List<GameObject> m_lgoUIButtons;

    public Dictionary<string, Buttons> m_dButtons;
    private List<Buttons> m_goButtons;
    public bool m_bButtonsLoaded;

    /// <summary>
    /// ASSIGNING TEXT AT RUNTIME SO CANVAS CAN BE IMPLEMENTD EASILY...
    /// </summary>
    private List<Text> m_tWongleWorkerText;
    private enum Text_Tags { Overall_Level, WC_Level,Fish_Level,Mine_Level,Farm_Level, Trees_Cut, Fish_Caught, Rocks_Mined, Farms_Hoed };
    private List<string> m_lsBaseText;
    private Text_Tags m_ttCurrentTag;
    private GameObject m_goTextGroup;
    public bool m_bDisplayingText;
    private List<int> m_liTextMemory; //for update if certain stats have changed when a user is say watching a wongle worker
    private GameObject m_goSelected; // this will be replaced by the controlhandler's selectd obj.

    private void Awake()
    {
        if (DisplayHandler.m_sDHControl == null)
        {
            DisplayHandler.m_sDHControl = this;

            m_dButtons = new Dictionary<string, Buttons>();
            AssignAndLoadButton("Worker");
            AssignAndLoadButton("Knight");
            AssignAndLoadButton("Wizard");

            for (int i = 0; i < m_goUIObj.Count; i++)
            {
                if (m_goUIObj[i] != null)
                {
                    m_goUIObj[i].SetActive(false);
                }
            }

            m_goTextGroup = GameObject.FindGameObjectWithTag("WorkerInfoTextGroup");
            if (m_goTextGroup != null)
            {
                m_goTextGroup.GetComponent<CanvasGroup>().alpha = 0;

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
    }


    private void AssignAndLoadButton(string _sName)
    {
       // Dictionary<string, Buttons> m_dButtons = new Dictionary<string, Buttons>();
        GameObject _goTemp = GameObject.FindGameObjectWithTag(_sName + "Button");
        // Sprite _sTemp = Resources.Load("UI_Objects/" + _sName, typeof(Sprite).Cast<Sprite>();
        Sprite _sTemp = Resources.Load<Sprite>("Sprites/UI_Objects/" + _sName + "/" + _sName + "Greyed");
        print(_sTemp.name);
        //GameObject _goTemp = m_lgoUIButtons[0].gameObject;
        Buttons _cWongleButton = null;

        if (_goTemp != null)
        {
            _cWongleButton = new Buttons(_goTemp, _sTemp, _sName + "Button" );
        }
        else
        {
            print("The button you were trying to add to the dictionary is null!");
        }
        if (_cWongleButton != null)
        {

            m_dButtons.Add(_sName, _cWongleButton);

            Buttons temp = null;
            if (m_dButtons.TryGetValue(_sName, out temp))
            {
                // print("Buttons loaded : Name:" + temp.m_sButtonID + ", " + temp.m_goButton.tag);
                print("Buttons loaded : Name:" + temp.m_goButton.tag);
            }
            else
            {
                print("Couldn't load buttons");
            }
        }
    }

    /// <summary>
    /// Return's true if the alpha is set to 1.0, thus they are supposedly being displayed...
    /// </summary>
    /// <param name="_sUnitName"></param>
    /// <returns></returns>
    public bool CheckAlphaOfUI(string _sUnitName)
    {
        Buttons temp = null;
        print("Attempting to get : " + _sUnitName);
        if (m_dButtons.TryGetValue(_sUnitName, out temp))
        {
            if (temp.m_goButton.GetComponent<CanvasGroup>().alpha == 1.0f)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
           
    }

    public void AlterUIAlpha(bool _bFade,string _sUnitName)
    {
        Buttons temp = null;
        if (m_dButtons.TryGetValue(_sUnitName, out temp))
        {
            // only do if displayed?
            switch (_bFade)
            {
                case true:
                    {
                        temp.m_goButton.gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
                        break;
                    }
                case false:
                    {
                        temp.m_goButton.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
                        break;
                    }
            }
        }
        else
        {
            print("Couldn't update the alpha because we weren't able to access the Dictionary");
        }
     /*   switch (_sUnitName) // might rename to buttons
        {
            case "Knight":
                {
                    m_dButtons["Knight"].m_goButton
                    break;
                }
        }*/
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
        if (m_goTextGroup == null)
            return;
        if (m_goTextGroup.GetComponent<CanvasGroup>().alpha != 1)
            m_goTextGroup.GetComponent<CanvasGroup>().alpha = 1;

        if (!m_bDisplayingText)
            m_bDisplayingText = true;

        m_goSelected = _selectedWongle;
        
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
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateState(false);
                    return true;
                }
                
            }
            else if (hit.transform.gameObject.tag != "HomeBuilding" && m_bDisplayingUnit)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateState(false);
                    return true;
                }
            }
        }
        return false;
    }

    public void ResetState(bool _bWorker)
    {
        switch (_bWorker)
        {
            case true:
                {
                    if (m_bDisplayingText)
                    {
                        if (m_goTextGroup.GetComponent<CanvasGroup>().alpha != 0)
                            m_goTextGroup.GetComponent<CanvasGroup>().alpha = 0;

                        m_bDisplayingText = false;
                    }
                    if (m_bDisplayingBuildings)
                    {
                        m_bDisplayingBuildings = false;
                        m_goUIObj[1].SetActive(false); // m_goUiOBJ[1] = BuildOptions 
                    }
                    break;
                }
            default:
                break;
        }
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

    private void CheckUnitButtonDisplay()
    {
       /* Buttons temp = null;
        Spawning_Cost scTemp = null;
        bool _failed = false;
        if (m_dButtons.TryGetValue(_sName, out temp))
        {
            if (HomeSpawning.m_sHomeSpawningControl.m_dExpenseInformation.TryGetValue(_sName, out scTemp))
            {
                if (scTemp.m_iCrystalCost > HouseController.CrystalAmount)
                {
                    _failed = true;
                }

                else if (scTemp.m_iFoodCost > HouseController.m_iFoodCount)
                {
                    _failed = true;
                }

                else if (scTemp.m_iWoodCost > HouseController.WoodAmount)
                {
                    _failed = true;
                }
            }

            if (_failed)
            {
                temp.m_goButton.GetComponent<Image>().sprite = temp.m_sUnavailableSprite;
            }
        }
        else
        {
            print("Unable to attain button information for: " + _sName);
        }*/
           string[] _vars = { "Worker", "Knight", "Wizard" };
           Buttons temp = null;
           Spawning_Cost scTemp = null;
           for (int i = 0; i < _vars.Length; i++)
           {
               bool _failed = false;
               if (m_dButtons.TryGetValue(_vars[i], out temp))
               {
                    if (HomeSpawning.m_sHomeSpawningControl.m_dExpenseInformation.TryGetValue(_vars[i], out scTemp))
                    {
                        if (scTemp.m_iCrystalCost > HouseController.CrystalAmount)
                        {
                            _failed = true;
                        }

                        else if (scTemp.m_iFoodCost > HouseController.m_iFoodCount)
                        {
                            _failed = true;
                        }

                        else if (scTemp.m_iWoodCost > HouseController.WoodAmount)
                        {
                            _failed = true;
                        }
                    }

                    if (_failed)
                    {
                        // temp.m_goButton.GetComponent<Image>().sprite = temp.m_sUnavailableSprite;
                        temp.UpdateSprite(false);
                    }
                    else
                    {
                        temp.UpdateSprite(true);
                    }
               }
               else
               {
                   print("Unable to attain button information for: " + _vars[i]);
               }
           }
    }


    private void Update()
    {
        if (m_bDisplayingText)
            TextUpdateHandle();
        else 
        {
            if (m_goTextGroup.GetComponent<CanvasGroup>().alpha != 0)
                m_goTextGroup.GetComponent<CanvasGroup>().alpha = 0;
        }

        /***
         * 
         * So many ways we could handle this, use this update and always check to see if we need to update the sprite.
         * or alternatively, run a function each time that the disply is utilized, and thereafter each time a player succesfully
         * purchases a unit. IDK
         * 
         ***/


        if (m_bDisplayingUnit)
        {
            CheckUnitButtonDisplay();
        }
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

       /* if (!m_bButtonsLoaded)
        {
            //GameObject _goTemp = GameObject.FindGameObjectWithTag("WorkerButton");
            //GameObject _goTemp = m_lgoUIButtons[0].gameObject;
            Buttons _cWongleButton = null;

            if (_goTemp != null)
            {
                _cWongleButton = new Buttons(_goTemp, "WongleButton");
            }
            else
            {
                print("The button you were trying to add to the dictionary is null!");
            }

            if (_cWongleButton != null)
            {
                m_goButtons.Add(_cWongleButton);

                m_dButtons.Add("Worker", m_goButtons[0]);

                Buttons temp = null;
                if (m_dButtons.TryGetValue("Worker", out temp))
                {
                    // print("Buttons loaded : Name:" + temp.m_sButtonID + ", " + temp.m_goButton.tag);
                    print("Buttons loaded : Name:" + temp.m_goButton.tag);

                    m_bButtonsLoaded = true;
                }
                else
                {
                    print("Couldn't load buttons");
                    m_bButtonsLoaded = false;
                }
            }
            
        }*/
    }
}
