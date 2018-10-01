using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWongleHand : MonoBehaviour {
    public GameObject WongleMain;
    public List<GameObject> items;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(WongleMain.GetComponent<WongleController>().Work != null)
        {
            switch (WongleMain.GetComponent<WongleController>().Work.tag)
            {
                case "Army":
                    {

                        for (int i = 0; i < items.Count; i++)
                        {
                            items[i].SetActive(false);
                        }
                        items[0].SetActive(true);
                        break;
                    }
                case "WoodCutter":
                    {

                        for (int i = 0; i < items.Count; i++)
                        {
                            items[i].SetActive(false);
                        }
                        items[1].SetActive(true);
                        break;
                    }
                default:
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            items[i].SetActive(false);
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
        }
		
	}
}
