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
	void Start () {
        if(hoverui != null)
        {
            hoverui.SetActive(false);
        }
        
	}
	void Update () {
        if(hoverui != null)
        {
            if (hoverui.activeSelf == true)
            {
                hoverui.transform.position = Input.mousePosition;
            }
        }
		
	}
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
            hoverui.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
