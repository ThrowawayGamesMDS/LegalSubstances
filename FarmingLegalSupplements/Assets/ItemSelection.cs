using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {
    public Camera camera;
    public GameObject SelectedObject;
    public Text name;
    public Text description;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.GetComponent<SelectableObject>() != null)
                {
                    SelectedObject = hit.transform.gameObject;
                   
                    
                }
                else
                {
                    SelectedObject = null;
                }
            }

        }


        if (SelectedObject != null)
        {
            name.text = SelectedObject.GetComponent<SelectableObject>().Name;
            description.text = SelectedObject.GetComponent<SelectableObject>().Description;
            if(SelectedObject.GetComponent<SelectableObject>().hasSpecificWindow)
            {
                //do stuff
            }
        }
        else
        {
            name.text = "";
            description.text = "";
        }
    }
}
