using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {
    public Camera camera;
    public GameObject SelectedObject;
    public Text name;
    public Text description;
	
	// Update is called once per frame
	void Update () {
        //if left mouse button pressed
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //create raycast to mouse position
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //if ray cast hits object with selectableobject script
                if (hit.transform.GetComponent<SelectableObject>() != null)
                {
                    //object is selected
                    SelectedObject = hit.transform.gameObject;
                }
                //if raycast doesnt hit object with selectableobject script
                else
                {
                    //unselect any objects that are set
                    SelectedObject = null;
                }
            }

        }
        
           
        
        

        //if an object is selected
        if (SelectedObject != null)
        {
            //change ui text to objects specific text
            name.text = SelectedObject.GetComponent<SelectableObject>().Name;
            description.text = SelectedObject.GetComponent<SelectableObject>().Description;
        }
        //if no object is selected
        else
        {
            //set ui text to nothing
            name.text = "";
            description.text = "";
            
        }




    }
    
}
