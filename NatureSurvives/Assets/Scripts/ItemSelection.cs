﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {
    public Camera Ccamera;
    public GameObject SelectedObject;
    public Text nametxt;
    public Text description;
    
	
	// Update is called once per frame
	void Update () {
        //if left mouse button pressed
        
       if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            /***
             * 
             * the hell is this 
             * 
             ***/

            //create raycast to mouse position
            Ray ray = Ccamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layermask =~ LayerMask.GetMask("GridSquare");
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //print("You clicked: " + hit.transform.name);
                //if ray cast hits object with selectableobject script
                print(hit.transform.gameObject.name);
                if (hit.transform.GetComponent<SelectableObject>() != null)
                {
                    //object is selected
                    SelectedObject = hit.transform.gameObject;
                    hit.transform.GetComponent<SelectableObject>().isSelected = true;
                    if (SelectedObject.gameObject.GetComponent<SelectableUnitComponent>().Type == SelectableUnitComponent.workerType.Worker)
                    {
                        // display building
                        if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == false)
                        {
                            gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                        }
                    }
                    else if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == true && SelectedObject.gameObject.GetComponent<SelectableUnitComponent>().Type != SelectableUnitComponent.workerType.Worker)
                    {
                        gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                    }
                    print("HIT A TRANSFORM WITH SELECTABLE CNTS");
                }
                else
                {
                    print("DIDNT HIT SHIT");
                    if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == true)
                    {
                        gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                    }
                    //unselect any objects that are set
                    if (SelectedObject != null)
                    {
                        if (gameObject.GetComponent<DisplayHandler>().m_bDisplayingBuildings == true)
                        {
                            gameObject.GetComponent<DisplayHandler>().UpdateState(true); // is worker
                        }
                        SelectedObject.GetComponent<SelectableObject>().isSelected = false;
                        SelectedObject = null;
                    }

                }


                /***
                     * 
                     * Bundy handle for camera - de-selects currently set obj for rotation provided one is set
                     * 
                ***/
                /*    if (hit.transform.GetComponent<SelectableUnitComponent>() == null && gameObject.GetComponent<cameraController>().m_bCamSelObjRotation == true)
                    // obj selected isn't a unit that's selectable yet the camera is attempting to rotate around an object...
                    {
                        gameObject.GetComponent<cameraController>().m_bCamSelObjRotation = false;
                        gameObject.GetComponent<cameraController>().m_bCamReset = true;
                        print("Resteting the camera rotation obj");
                    }*/
            }

        }
        
           
        
        

        //if an object is selected
        if (SelectedObject != null)
        {
            //change ui text to objects specific text
            //nametxt.text = SelectedObject.GetComponent<SelectableObject>().Name;
            //description.text = SelectedObject.GetComponent<SelectableObject>().Description;
        }
        //if no object is selected
        else
        {
            //set ui text to nothing
            //nametxt.text = "";
            //description.text = "";
            
        }

    


    }
    
}
