using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class checkposofclick : MonoBehaviour
{
    public RectTransform rectTransform;
 
    void Update()
    {
        Vector2 localPoint;
        Rect r = rectTransform.rect;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);
        
        Debug.Log("local " + localPoint);
    }
}

