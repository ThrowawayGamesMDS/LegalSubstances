using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WongleCountUI : MonoBehaviour
{
    public Text current;
    public Text maximum;
    private HomeSpawning homespawningOBJ;



    private void Start()
    {
        homespawningOBJ = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>();
    }
    // Update is called once per frame
    void Update()
    {
        current.text = homespawningOBJ.iCurrentWongleCount.ToString();
        maximum.text = homespawningOBJ.iMaximumWongleCount.ToString();
    }
}
