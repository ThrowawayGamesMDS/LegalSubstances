using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cursorChanger : MonoBehaviour
{

    public List<string> CursorTags;
    public List<Texture2D> AvailableCursors;
    public CursorMode cursorMode = CursorMode.Auto;

    // Update is called once per frame
    void Update()
    {
        RaycastHit _rh;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(Camera.main.transform.position, ray.direction, out _rh, 1000))
        {
            Cursor.SetCursor(AvailableCursors[0], Vector2.zero, cursorMode);
            if (DisplayHandler.m_sDHControl.m_bDisplayingBuildings) // nooby handle for when we ever have wongle workers selected - ones the cursor switch apply too unless its melee?
            {
                for (int i = 0; i < CursorTags.Count; i++)
                {
                    if (_rh.transform.tag == CursorTags[i])
                    {
                        Cursor.SetCursor(AvailableCursors[i], Vector2.zero, cursorMode);
                    }
                }
            }
        }
    }
}
