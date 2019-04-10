using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class checkposofclick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Transform player;
    public bool isHovered;
    private Camera cam;
    private RawImage minimapRawImage;
    private void Start()
    {
        isHovered = false;
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        float xPositionCameraCoordinates = (xPositionDeltaPoint-165);
        float yPositionCameraCoordinates = (yPositionDeltaPoint-165);
        //Debug.Log((xPositionCameraCoordinates - 130) + ", " + (yPositionCameraCoordinates - 130));
        


        if(isHovered)
        {
            //print("yes");
            print((xPositionCameraCoordinates) + ", " + (yPositionCameraCoordinates));
            if(!GameObject.FindGameObjectWithTag("Player").GetComponent<SelectionBox>().isSelecting)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    player.position = new Vector3((((xPositionCameraCoordinates) / 165) * 250), player.position.y, (((yPositionCameraCoordinates) / 165) * 250));
                }
            }
            
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                int _iTotalWongles = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>().iCurrentWongleCount;
                GameObject[] _goUnits;
                _goUnits = GameObject.FindGameObjectsWithTag("Wongle");
                for (int i = 0; i < _iTotalWongles; i++)
                {
                    _goUnits[i].GetComponent<SelectableUnitComponent>().minimapRightClick((((xPositionCameraCoordinates) / 165) * 250), (((yPositionCameraCoordinates) / 165) * 250));
                }
            }
        }
    }


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHovered = false;
    }
}

