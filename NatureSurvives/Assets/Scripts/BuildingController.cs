using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {
    public string input;
    public string output;
    public float TimeToComplete;
    public float timer;
    public int inputAmount;
    public int outputAmount;
    public int AmountProduced;
    public bool isOccupied;
    public GameObject worker;
    private void Start()
    {
        isOccupied = false;
    }
    void Update () {
		if(isOccupied)
        {
            timer += 1 * Time.deltaTime;
            if(timer >= TimeToComplete)
            {
                timer = TimeToComplete;
                if(inputAmount >= AmountProduced)
                {
                    outputAmount += AmountProduced;
                    inputAmount -= AmountProduced;
                    timer = 0;
                }
            }
        }
	}
}
