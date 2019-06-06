using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spawning_Cost : MonoBehaviour
{
    public string m_sName;
    public int m_iWoodCost;
    public int m_iCrystalCost;
    public int m_iFoodCost;
    public GameObject m_goUiDisplay;
    public GameObject m_goSpawnableUnit;
    public Spawning_Cost(string _sName, int _iWoodCost, int _iCrystalCost, int _iFoodCost, GameObject _goUiDisplay, GameObject _goSpawnableUnit)
    {
        m_sName = _sName;
        m_iWoodCost = _iWoodCost;
        m_iCrystalCost = _iCrystalCost;
        m_iFoodCost = _iFoodCost;
        m_goUiDisplay = _goUiDisplay;
        m_goSpawnableUnit = _goSpawnableUnit;

    }
}

public class HomeSpawning : MonoBehaviour {
    public static HomeSpawning m_sHomeSpawningControl;
    public List<GameObject> Prefabs;
    public int iCurrentWongleCount;
    public int iMaximumWongleCount;
    public List<string> UnitQueue;
    public List<GameObject> UIObjQueue;
    public bool hasTimer;
    public bool m_bRemoveQueueTimerCheat;
    public float timerVal;
    public float m_fSpawnTimer;
    public GameObject workerUI;
    public GameObject wizardUI;
    public GameObject knightUI;
    public GameObject m_goScoutUI; //switch to dictionary
    public Transform canvas;
    public GameObject army;
    public Vector3 g_v3WizardRally;
    public Vector3 g_v3KnightRally;
    public Vector3 g_v3WorkerRally;

    public Dictionary<string, Spawning_Cost> m_dExpenseInformation;
    private void Awake()
    {
        if (m_sHomeSpawningControl == null)
        {
            m_sHomeSpawningControl = this;

            m_bRemoveQueueTimerCheat = false;

            iCurrentWongleCount = 5;
            army = GameObject.FindGameObjectWithTag("Army");
            //g_v3WorkerRally = new Vector3(120, 0, -110);
            //g_v3KnightRally = new Vector3(100, 0, -110);
            //g_v3WizardRally = new Vector3(80, 0, -110);

            m_dExpenseInformation = new Dictionary<string, Spawning_Cost>();
            // for scout
            GameObject temp = null;
            GameObject temp2 = null;
            temp = Instantiate(Resources.Load("Prefabs/Player_Units/WongleScout"), gameObject.transform) as GameObject;
            temp2 = Instantiate(Resources.Load("Prefabs/UI_Objects/ScoutQueue"), canvas) as GameObject;
            temp.gameObject.transform.localScale = new Vector3(10, 10, 10); // so weird -> When using resources.load it scales stuff up to 10, and then setting back to 10 sets to 1, honestly idk
            //temp2.gameObject.transform.localScale = new Vector3(10, 10, 10);// -> need to setactive when instantiating...
            AddExpenseInformation("Scout", 5, 5, 5,temp2, temp);
            temp.SetActive(false);
            temp2.SetActive(false);
            //end of scout stuff

            temp = Instantiate(Resources.Load("Prefabs/Player_Units/WongleWorker"), gameObject.transform) as GameObject;
            temp.gameObject.transform.localScale = new Vector3(10, 10, 10);
            AddExpenseInformation("Worker", 0, 0, 50, workerUI, temp);
            temp.SetActive(false);

            temp = Instantiate(Resources.Load("Prefabs/Player_Units/Wizuk"), gameObject.transform) as GameObject;
            temp.gameObject.transform.localScale = new Vector3(10, 10, 10);
            AddExpenseInformation("Wizard", 25, 45, 0, wizardUI, temp);
            temp.SetActive(false);

            temp = Instantiate(Resources.Load("Prefabs/Player_Units/Knight"), gameObject.transform) as GameObject;
            temp.gameObject.transform.localScale = new Vector3(10, 10, 10);
            AddExpenseInformation("Knight", 0, 20, 60, knightUI, temp);
            temp.SetActive(false);
           /* if (temp != null)
            {
                AddExpenseInformation("Scout", 5, 5, 5, workerUI, temp);
                temp.SetActive(false);
               // Destroy(temp);
            }
            else
                AddExpenseInformation("Scout", 5, 5, 5, workerUI, Prefabs[0]);*/
        }
    }

    public void AddExpenseInformation(string _sName, int _iWoodCost, int _iCrystalCost, int _iFoodCost, GameObject _goUiDisplay, GameObject _goSpawnableUnit)
    {
        Spawning_Cost _scTemp = new Spawning_Cost(_sName, _iWoodCost, _iCrystalCost, _iFoodCost, _goUiDisplay, _goSpawnableUnit);
        m_dExpenseInformation.Add(_sName, _scTemp);

        _scTemp = null;
        if (Application.isEditor)
        {
            if (m_dExpenseInformation.TryGetValue(_sName, out _scTemp))
            {
                print("Expense Information loaded : Name: " + _scTemp.m_sName + " W/C/F Cost: " + _scTemp.m_iWoodCost + "/" + _scTemp.m_iCrystalCost + "/" + _scTemp.m_iFoodCost);
            }
            else
            {
                print("Expense Information failed to load for " + _sName + "...");
            }
        }
    }

    public void fQueueUnit(string unittype)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (iCurrentWongleCount + UIObjQueue.Count < iMaximumWongleCount)
        {
            Spawning_Cost temp = null;

            // Best optimization option

            if (m_dExpenseInformation.TryGetValue(unittype, out temp))
            {
                if (HouseController.WoodAmount >= temp.m_iWoodCost)
                {
                    if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                    {
                        if (HouseController.m_iFoodCount >= temp.m_iFoodCost)
                        {
                            HouseController.WoodAmount -= temp.m_iWoodCost;
                            HouseController.CrystalAmount -= temp.m_iCrystalCost;
                            HouseController.m_iFoodCount -= temp.m_iFoodCost;
                            GameObject uiobj = Instantiate(temp.m_goUiDisplay, canvas);
                            uiobj.SetActive(true);
                            uiobj.transform.SetParent(canvas, false);
                            UnitQueue.Insert(UnitQueue.Count, unittype);
                            uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                            UIObjQueue.Insert(UIObjQueue.Count, uiobj);

                        }
                        else
                        {
                            int difference = temp.m_iFoodCost - HouseController.m_iFoodCount;
                            NotificationManager.Instance.SetNewNotification("Can't add, not enough food resource.  Get " + difference + " more food.");
                        }
                    }
                    else
                    {
                        int difference = temp.m_iCrystalCost - HouseController.CrystalAmount;
                        NotificationManager.Instance.SetNewNotification("Can't add, not enough crystal resource.  Get " + difference + " more crystal.");
                    }
                }
                else
                {
                    int difference = temp.m_iWoodCost - HouseController.WoodAmount;
                    NotificationManager.Instance.SetNewNotification("Can't add, not enough wood resource.  Get " + difference + " more wood.");
                }
            }
        }
    }

    private Vector3 GetRallyPoint(string _sUnitType)
    {
        switch (_sUnitType)
        {
            case "Worker":
                {
                    return g_v3WorkerRally;
                }
            case "Knight":
                {
                    return g_v3KnightRally;
                }
            case "Wizard":
                {
                    return g_v3WizardRally;
                }
            default:
                return g_v3WorkerRally; // Scout etc
        }
    }

    public void SpawnUnit(string unittype)
    {
        Spawning_Cost _scTemp = null;
        GameObject temp = null;
        if (m_dExpenseInformation.TryGetValue(unittype,out _scTemp))
        {
            temp = Instantiate(_scTemp.m_goSpawnableUnit, transform.position, Prefabs[0].transform.rotation);
            temp.SetActive(true);
            temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
            temp.GetComponent<WongleController>().agent.avoidancePriority = Random.Range(1, 99);
            temp.GetComponent<WongleController>().priority = temp.GetComponent<WongleController>().agent.avoidancePriority;
            temp.GetComponent<WongleController>().agent.SetDestination(GetRallyPoint(unittype));
            AddtoList(unittype, temp);
        }
    }

    private void Update()
    {
        iCurrentWongleCount = GameObject.FindGameObjectsWithTag("Wongle").Length;

        
        if(UnitQueue.Count > 0)
        {
            if (!m_bRemoveQueueTimerCheat)
            {
                if (!hasTimer)
                {
                    timerVal = 0;
                    hasTimer = true;
                    m_fSpawnTimer = (float)System.Int32.Parse(UIObjQueue[0].gameObject.transform.Find("Text").GetComponent<Text>().text);
                    queueBar.m_sQueueBarHandle.m_fTimerValue = m_fSpawnTimer;
                    print("Spawn Timer: " + m_fSpawnTimer);
                }
                else
                {
                    timerVal += Time.deltaTime;
                    UIObjQueue[0].gameObject.transform.Find("Text").GetComponent<Text>().text = "" + (m_fSpawnTimer - (int)timerVal);
                    if (timerVal >= m_fSpawnTimer) // make a more intuitive timer with different vars for respectable units...
                    {
                        SpawnUnit(UnitQueue[0]);
                        hasTimer = false;
                        timerVal = 0;
                        UnitQueue.RemoveAt(0);
                        Destroy(UIObjQueue[0]);
                        UIObjQueue.RemoveAt(0);
                    }
                }
            }
            else
            {
                SpawnUnit(UnitQueue[0]);
                hasTimer = false;
                timerVal = 0;
                UnitQueue.RemoveAt(0);
                Destroy(UIObjQueue[0]);
                UIObjQueue.RemoveAt(0);
            }
        }   
        
        for (int i = 0; i < UIObjQueue.Count; i++)
        {

            if(i <= 9)
            {
                UIObjQueue[i].transform.localPosition = new Vector3(-116 + 35 * i, -160, 0);
            }
            else if (i > 9 && i <= 19)
            {
                UIObjQueue[i].transform.localPosition = new Vector3(-116 + (35 * (i - 10)), -195, 0);
            }
            else
            {
                UIObjQueue[i].transform.localPosition = new Vector3(-116 + (35 * (i - 19)), -230, 0);
            }
                    
            UIObjQueue[i].GetComponent<queueUIScript>().placeInQueue = i;
        }
    }

    public void Refund(string x)
    {
        Spawning_Cost temp = null;
        if (m_dExpenseInformation.TryGetValue(x,out temp))
        {
            HouseController.WoodAmount += temp.m_iWoodCost;
            HouseController.m_iFoodCount += temp.m_iFoodCost;
            HouseController.CrystalAmount += temp.m_iCrystalCost;
        }
    }

    public void AddtoList(string unittype, GameObject temp)
    {
        SelectionBox sb = GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>();
        switch (unittype)
        {
            case "Knight":
                {
                    sb.m_goMeleeUnits.Add(temp);
                    break;
                }
            case "Wizard":
                {
                    sb.m_goRangedUnits.Add(temp);
                    break;
                }
            default:
                {
                    break;
                }
        }

        
    }
}