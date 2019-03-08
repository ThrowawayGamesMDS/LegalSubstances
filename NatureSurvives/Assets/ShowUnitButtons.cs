using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnitButtons : MonoBehaviour
{
    public GameObject uiOBJ;
    public SelectableObject select;
    private void Update()
    {
        if(select.isSelected)
        {
            uiOBJ.SetActive(true);
        }
        if (!select.isSelected)
        {
            uiOBJ.SetActive(false);
        }
    }
}
