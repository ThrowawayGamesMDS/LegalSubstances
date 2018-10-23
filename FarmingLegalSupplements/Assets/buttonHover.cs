using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverui;
	void Start () {
        hoverui.SetActive(false);
	}
	void Update () {
		if(hoverui.activeSelf == true)
        {
            hoverui.transform.position = Input.mousePosition;
        }
	}
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hoverui.SetActive(true);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hoverui.SetActive(false);
    }
}
