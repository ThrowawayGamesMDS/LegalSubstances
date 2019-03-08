using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Name;
    public GameObject refOBJ;
    public GameObject hoverui;
    public bool isResource;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(!isResource)
        {
            hoverui.GetComponent<updatePriceUI>().UpdateFloats(refOBJ.GetComponent<costToPlace>().WoodCost, refOBJ.GetComponent<costToPlace>().CrystalCost, refOBJ.GetComponent<costToPlace>().FoodCost, Name);
            hoverui.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(!isResource)
        {
            hoverui.GetComponent<updatePriceUI>().UpdateFloats(0,0,0,"");
            hoverui.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
