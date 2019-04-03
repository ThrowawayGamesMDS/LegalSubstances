using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WongleHouse : MonoBehaviour
{
    void Start()
    {
        HomeSpawning home = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>();
        home.iMaximumWongleCount += 5;
    }
}
