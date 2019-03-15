using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class checkposofclick : MonoBehaviour
{
    public Camera cam;
    public RawImage minimapRawImage;
    private void Start()
    {
        minimapRawImage = GetComponent<RawImage>();
    }
    void Update()
    {
        Vector3[] corners = new Vector3[4];
        minimapRawImage.rectTransform.GetWorldCorners(corners);
        Rect newRect = new Rect(corners[0], corners[2] - corners[0]);

        //Get the pixel offset amount from the current mouse position to the left edge of the minimap
        //rect transform.  And likewise for the y offset position.
        float xPositionDeltaPoint = Input.mousePosition.x - newRect.x;
        float yPositionDeltaPoint = Input.mousePosition.y - newRect.y;

        //Debug.Log("The x position delta is: " + xPositionDeltaPoint);
        //Debug.Log("The y position delta is: " + yPositionDeltaPoint);


        //The value "170" is the raw image size currently
        float xPositionCameraCoordinates = (xPositionDeltaPoint);
        float yPositionCameraCoordinates = (yPositionDeltaPoint);
        Debug.Log((xPositionCameraCoordinates - 130) + ", " + (yPositionCameraCoordinates - 130));
    }
}

