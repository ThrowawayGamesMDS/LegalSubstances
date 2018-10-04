using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridSystem : MonoBehaviour {
    public GameObject gridObj;
    public Transform gridParent;
	// Use this for initialization
	void Start () {
        int fx = 1, fz = 1;
		for (int x = -245; x <= 245; x += 10)
        {
            for(int z = -245; z <= 245; z += 10)
            {
                GameObject temp = Instantiate(gridObj, new Vector3(x, 1, z), gridObj.transform.rotation);
                temp.transform.parent = gridParent;
                temp.name = "Grid(" + fx + ")(" + fz + ")";
                temp.GetComponent<GridObject>().xValue = fx;
                temp.GetComponent<GridObject>().zValue = fz;
                fz++;
            }
            fx++;
            fz = 1;
        }

        StartCoroutine(checkChildren());
	}



    IEnumerator checkChildren()
    {
        while(true)
        {
            foreach(Transform child in transform)
            {
                child.GetComponent<GridObject>().CheckCorrupt();
                yield return new WaitForSeconds(0.05f);
            }
            
        }
        

    }
}
