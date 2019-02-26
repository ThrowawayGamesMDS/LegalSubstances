using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidefishyboy : MonoBehaviour
{
    public GameObject fish;
    public bool isVisible = false;
    private void Start()
    {
        fish.SetActive(false);
    }
    private void Update()
    {
        if (isVisible)
        {
            fish.SetActive(true); 
        }
        else
        {
            fish.SetActive(false);
        } 
    }
}
