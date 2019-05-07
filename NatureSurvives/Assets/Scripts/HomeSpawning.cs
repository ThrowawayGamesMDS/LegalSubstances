using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning_Cost : MonoBehaviour
{
    public string m_sName;
    public int m_iWoodCost;
    public int m_iCrystalCost;
    public int m_iFoodCost;
    public GameObject m_goUiDisplay;
    public Spawning_Cost(string _sName, int _iWoodCost, int _iCrystalCost, int _iFoodCost, GameObject _goUiDisplay)
    {
        m_sName = _sName;
        m_iWoodCost = _iWoodCost;
        m_iCrystalCost = _iCrystalCost;
        m_iFoodCost = _iFoodCost;
        m_goUiDisplay = _goUiDisplay;


    }

    public GameObject InstantiateUi(GameObject canvas, int _iQueueCount)
    {
        GameObject uiobj = Instantiate(m_goUiDisplay, canvas.transform);
        uiobj.transform.SetParent(canvas.transform.parent, false);
        uiobj.GetComponent<queueUIScript>().placeInQueue = _iQueueCount;
        return uiobj;
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
    public float timerVal;
    public GameObject workerUI;
    public GameObject wizardUI;
    public GameObject knightUI;
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
            iCurrentWongleCount = 5;
            army = GameObject.FindGameObjectWithTag("Army");
            g_v3WorkerRally = new Vector3(120, 0, -110);
            g_v3WorkerRally = new Vector3(100, 0, -110);
            g_v3WorkerRally = new Vector3(80, 0, -110);

            m_dExpenseInformation = new Dictionary<string, Spawning_Cost>();

            AddExpenseInformation("Worker", 0, 0, 50, workerUI);
            AddExpenseInformation("Knight", 0, 20, 60, knightUI);
            AddExpenseInformation("Wizard", 25, 45, 0, wizardUI);
            AddExpenseInformation("Scout", 5, 5, 5, workerUI);
        }
    }

    public void AddExpenseInformation(string _sName, int _iWoodCost, int _iCrystalCost, int _iFoodCost, GameObject _goUiDisplay)
    {
        Spawning_Cost _scTemp = new Spawning_Cost(_sName, _iWoodCost, _iCrystalCost, _iFoodCost, _goUiDisplay);
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
        if (iCurrentWongleCount + UIObjQueue.Count < iMaximumWongleCount)
        {
            Spawning_Cost temp = null;
            bool _bDisplayUI = false;

            // Best optimization option
            /**
            if (m_dExpenseInformation.TryGetValue(unittype, out temp))
            {
                if (HouseController.WoodAmount >= temp.m_iWoodCost)
                {
                    if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                    {
                        if (HouseController.WhiteAmount >= temp.m_iWoodCost)
                        {
                            HouseController.WoodAmount -= temp.m_iWoodCost;
                            HouseController.CrystalAmount -= temp.m_iCrystalCost;
                            HouseController.WhiteAmount -= temp.m_iFoodCost;
                            GameObject uiobj = temp.InstantiateUi(canvas.gameObject, UIObjQueue.Count - 1);
                            uiobj.transform.SetParent(canvas, false);
                            uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                            UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                        }
                    }
                }
            }
            **/
            // Second Best
            /*
            if (m_dExpenseInformation.TryGetValue(unittype, out temp))
            {
                if (HouseController.WoodAmount >= temp.m_iWoodCost)
                {
                    if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                    {
                        if (HouseController.WhiteAmount >= temp.m_iWoodCost)
                        {
                            HouseController.WoodAmount -= temp.m_iWoodCost;
                            HouseController.CrystalAmount -= temp.m_iCrystalCost;
                            HouseController.WhiteAmount -= temp.m_iFoodCost;
                            _bDisplayUI = true;
                        }
                    }
                }
            }

            if (_bDisplayUI)
            {
                GameObject uiobj = null;
                switch (unittype)
                {
                    case "Worker":
                        {
                            uiobj = Instantiate(workerUI, canvas);
                            break;
                        }
                    case "Knight":
                        {
                            uiobj = Instantiate(knightUI, canvas);
                            break;
                        }
                    case "Wizard":
                        {
                            uiobj = Instantiate(wizardUI, canvas);
                            break;
                        }
                    case "Scout":
                        {
                            uiobj = Instantiate(workerUI, canvas);
                            break;
                        }
                    default:
                        {
                            uiobj = Instantiate(workerUI, canvas);
                            break;
                        }

                }
                if (uiobj != null)
                {
                    uiobj.transform.SetParent(canvas, false);
                    uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                    UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                }
                else
                {
                    print("ui obj is null still?!");
                }
            }
            */


            if (unittype == "Worker")
            {
                // new style
                if (m_dExpenseInformation.TryGetValue(unittype, out temp))
                {
                    if (HouseController.WoodAmount >= temp.m_iWoodCost)
                    {
                        if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                        {
                            if (HouseController.WhiteAmount >= temp.m_iWoodCost)
                            {
                                UnitQueue.Insert(UnitQueue.Count, unittype);
                                GameObject uiobj = Instantiate(workerUI, canvas);
                                uiobj.transform.SetParent(canvas, false);
                                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                                HouseController.WoodAmount -= temp.m_iWoodCost;
                                HouseController.CrystalAmount -= temp.m_iCrystalCost;
                                HouseController.WhiteAmount -= temp.m_iFoodCost;
                            }
                        }
                    }
                }

                // old style

                    /* if (HouseController.WoodAmount >= Prefabs[0].GetComponent<costToPlace>().WoodCost)
                     {
                         if (HouseController.CrystalAmount >= Prefabs[0].GetComponent<costToPlace>().CrystalCost)
                         {
                             if (HouseController.WhiteAmount >= Prefabs[0].GetComponent<costToPlace>().FoodCost)
                             {
                                 UnitQueue.Insert(UnitQueue.Count, unittype);
                                 GameObject uiobj = Instantiate(workerUI, canvas);
                                 uiobj.transform.SetParent(canvas, false);
                                 uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                                 UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                                 HouseController.WoodAmount -= Prefabs[0].GetComponent<costToPlace>().WoodCost;
                                 HouseController.CrystalAmount -= Prefabs[0].GetComponent<costToPlace>().CrystalCost;
                                 HouseController.WhiteAmount -= Prefabs[0].GetComponent<costToPlace>().FoodCost;
                             }
                         }
                     }*/

        
                }
            else if (unittype == "Wizard")
            {
                if (m_dExpenseInformation.TryGetValue(unittype, out temp))
                {
                    if (HouseController.WoodAmount >= temp.m_iWoodCost)
                    {
                        if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                        {
                            if (HouseController.WhiteAmount >= temp.m_iFoodCost)
                            {
                                UnitQueue.Insert(UnitQueue.Count, unittype);
                                GameObject uiobj = Instantiate(wizardUI, canvas);
                                uiobj.transform.SetParent(canvas, false);
                                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                                HouseController.WoodAmount -= temp.m_iWoodCost;
                                HouseController.CrystalAmount -= temp.m_iCrystalCost;
                                HouseController.WhiteAmount -= temp.m_iFoodCost;
                            }
                        }
                    }
                }
            }
            else if (unittype == "Knight")
            {
                if (m_dExpenseInformation.TryGetValue(unittype, out temp))
                {
                    if (HouseController.WoodAmount >= temp.m_iWoodCost)
                    {
                        if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                        {
                            if (HouseController.WhiteAmount >= temp.m_iFoodCost)
                            {
                                UnitQueue.Insert(UnitQueue.Count, unittype);
                                GameObject uiobj = Instantiate(knightUI, canvas);
                                uiobj.transform.SetParent(canvas, false);
                                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                                HouseController.WoodAmount -= temp.m_iWoodCost;
                                HouseController.CrystalAmount -= temp.m_iCrystalCost;
                                HouseController.WhiteAmount -= temp.m_iFoodCost;
                            }
                        }
                    }
                }
            }
            else if (unittype == "Scout")
            {
                if (m_dExpenseInformation.TryGetValue("Scout", out temp))
                {
                    if (HouseController.WoodAmount >= temp.m_iWoodCost)
                    {
                        if (HouseController.CrystalAmount >= temp.m_iCrystalCost)
                        {
                            if (HouseController.WhiteAmount >= temp.m_iFoodCost)
                            {
                                UnitQueue.Insert(UnitQueue.Count, unittype);
                                GameObject uiobj = Instantiate(workerUI, canvas);
                                uiobj.transform.SetParent(canvas, false);
                                uiobj.GetComponent<queueUIScript>().placeInQueue = UnitQueue.Count - 1;
                                UIObjQueue.Insert(UIObjQueue.Count, uiobj);
                                HouseController.WoodAmount -= temp.m_iWoodCost;
                                HouseController.CrystalAmount -= temp.m_iCrystalCost;
                                HouseController.WhiteAmount -= temp.m_iFoodCost;
                            }
                        }
                    }
                }
                else
                {
                    print("Couldn't load Scout Expense Information");
                }
            }
        }
    }

    public void SpawnUnit(string unittype)
    {        
        GameObject temp;
        switch (unittype)
        {
            case "Worker":
                {
                    
                    temp = Instantiate(Prefabs[0], transform.position, Prefabs[0].transform.rotation);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.avoidancePriority = Random.Range(0,99);
                    temp.GetComponent<WongleController>().priority = temp.GetComponent<WongleController>().agent.avoidancePriority;
                    temp.GetComponent<WongleController>().agent.SetDestination(g_v3WorkerRally);   
                    break;
                }
            case "Wizard":
                {
                    temp = Instantiate(Prefabs[1], transform.position, Prefabs[1].transform.rotation, army.transform);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.avoidancePriority = Random.Range(0, 99);
                    temp.GetComponent<WongleController>().priority = temp.GetComponent<WongleController>().agent.avoidancePriority;
                    temp.GetComponent<WongleController>().Work = army;
                    temp.GetComponent<WongleController>().agent.SetDestination(g_v3WizardRally);                                
                    GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goRangedUnits.Add(temp);                  
                    break;
                }
            case "Knight":
                {
                    
                    temp = Instantiate(Prefabs[2], transform.position, Prefabs[2].transform.rotation, army.transform);
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.avoidancePriority = Random.Range(0, 99);
                    temp.GetComponent<WongleController>().priority = temp.GetComponent<WongleController>().agent.avoidancePriority;
                    temp.GetComponent<WongleController>().Work = army;
                    temp.GetComponent<WongleController>().agent.SetDestination(g_v3KnightRally);                                
                    GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().m_goMeleeUnits.Add(temp);                   
                    break;
                }
            case "Scout":
                {

                    temp = Instantiate(Resources.Load(Application.persistentDataPath + "Assets\\Prefabs\\Wongles\\WongleKnight"), transform.position, Prefabs[0].transform.rotation) as GameObject; // worker rotation?
                    temp.GetComponent<WongleController>().agent.stoppingDistance = 7;
                    temp.GetComponent<WongleController>().agent.avoidancePriority = Random.Range(0, 99);
                    temp.GetComponent<WongleController>().priority = temp.GetComponent<WongleController>().agent.avoidancePriority;
                    temp.GetComponent<WongleController>().agent.SetDestination(g_v3WorkerRally);
                    print("Spawning scout...");
                    break;
                }
            default:
                {
                    break;
                }
        }   
    }

    private void Update()
    {
        iCurrentWongleCount = GameObject.FindGameObjectsWithTag("Wongle").Length;

        
        if(UnitQueue.Count > 0)
        {
            if(!hasTimer)
            {
                timerVal = 0;
                hasTimer = true;
            }
            else
            {
                timerVal += Time.deltaTime;
                if(timerVal >= 7)
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
        if(x == "Worker")
        {
            HouseController.WoodAmount += Prefabs[0].GetComponent<costToPlace>().WoodCost;
            HouseController.WhiteAmount += Prefabs[0].GetComponent<costToPlace>().FoodCost;
            HouseController.CrystalAmount += Prefabs[0].GetComponent<costToPlace>().CrystalCost;
        }
        if (x == "Wizard")
        {
            HouseController.WoodAmount += Prefabs[1].GetComponent<costToPlace>().WoodCost;
            HouseController.WhiteAmount += Prefabs[1].GetComponent<costToPlace>().FoodCost;
            HouseController.CrystalAmount += Prefabs[1].GetComponent<costToPlace>().CrystalCost;
        }
        if (x == "Knight")
        {
            HouseController.WoodAmount += Prefabs[2].GetComponent<costToPlace>().WoodCost;
            HouseController.WhiteAmount += Prefabs[2].GetComponent<costToPlace>().FoodCost;
            HouseController.CrystalAmount += Prefabs[2].GetComponent<costToPlace>().CrystalCost;
        }
        if (x == "Scout")
        {
            if (m_dExpenseInformation.TryGetValue("Scout", out temp))
            {
                HouseController.WoodAmount += temp.m_iWoodCost;
                HouseController.WhiteAmount += temp.m_iFoodCost;
                HouseController.CrystalAmount += temp.m_iCrystalCost;
            }
        }
    }
}
