using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWongleHand : MonoBehaviour {
    public GameObject WongleMain;
    public List<GameObject> items;
    public bool isLeftHand;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(WongleMain.GetComponent<WongleController>().Work != null)
        {
            switch (WongleMain.GetComponent<WongleController>().Work.tag)
            {
                case "WoodCutter":
                    {
                        if(!isLeftHand)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            items[0].SetActive(true);
                        }
                        
                        break;
                    }
                case "Miner":
                    {

                        if (!isLeftHand)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            items[1].SetActive(true);
                        }
                        else
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            if (WongleMain.GetComponent<WongleController>().isGoingHome)
                            {
                                items[1].SetActive(true);
                            }
                        }
                        break;
                    }
                case "Building":
                    {

                        if (!isLeftHand)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            items[2].SetActive(true);
                        }
                        else
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            if (WongleMain.GetComponent<WongleController>().isGoingHome)
                            {
                                items[2].SetActive(true);
                            }
                        }
                        break;
                    }
                case "Builder":
                    {

                        if (!isLeftHand)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            items[4].SetActive(true);
                        }
                        else
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            if (WongleMain.GetComponent<WongleController>().isGoingHome)
                            {
                                //items[1].SetActive(true);
                            }
                        }
                        break;
                    }
                case "Fishermen":
                    {

                        if (!isLeftHand)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            items[5].SetActive(true);
                        }
                        else
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            if (WongleMain.GetComponent<WongleController>().isGoingHome)
                            {
                                //items[1].SetActive(true);
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (!isLeftHand)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].SetActive(false);
                            }
                            items[3].SetActive(true);
                        }
                        break;
                    }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
            items[3].SetActive(true);
        }
		
	}
}
