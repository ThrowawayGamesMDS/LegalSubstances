using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queueUIScript : MonoBehaviour
{
    public int placeInQueue;
    public void fDeleteQueue(string unittype)
    {
        GameObject home = GameObject.FindGameObjectWithTag("HomeBuilding");
        if(placeInQueue == 0)
        {
            home.GetComponent<HomeSpawning>().timerVal = 0;
            home.GetComponent<HomeSpawning>().hasTimer = false;
            
        }
        home.GetComponent<HomeSpawning>().UnitQueue.RemoveAt(placeInQueue);
        home.GetComponent<HomeSpawning>().UIObjQueue.RemoveAt(placeInQueue);
        Destroy(gameObject);
    }
}
