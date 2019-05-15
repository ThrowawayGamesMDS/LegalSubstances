using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour {
    public int TimeToProcess;
    public int greenAmount;
    public int m_iFoodCount;
    public int timer;
    public string typeOfPlant;
    // Use this for initialization
    void Start()
    {
        timer = 0;
        InvokeRepeating("GainResources", 0.1f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > TimeToProcess)
        {
            timer = TimeToProcess;
        }
    }

    void GainResources()
    {
        if(typeOfPlant == "Green")
        {
            if(greenAmount >0)
            {
                timer += 1;
            }
        }
        if(typeOfPlant == "White")
        {
            if(m_iFoodCount > 0)
            {
                timer += 1;
            }
        }
        
        
    }
}
