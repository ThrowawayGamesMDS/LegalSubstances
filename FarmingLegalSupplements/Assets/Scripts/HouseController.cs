using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {
    public int GreenAmount;
    public int WhiteAmount;
    public int GreenProcessed;
    public int WhiteProcessed;
    public int StartingMoney;
    public static int CashAmount;

	// Use this for initialization
	void Start () {
        GreenAmount = 0;
        WhiteAmount = 0;
        GreenProcessed = 0;
        WhiteProcessed = 0;
        CashAmount = StartingMoney;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
