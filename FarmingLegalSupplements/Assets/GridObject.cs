using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    public int xValue;
    public int zValue;
    public bool isCorrupted;
    public MeshRenderer rnd;
	// Use this for initialization
	void Start () {
        //InvokeRepeating("CheckCorrupt", 0.1f, 3);
	}
	
    public void Corrupt()
    {
        transform.parent = GameObject.FindGameObjectWithTag("CorruptedGrid").transform;
        rnd.enabled = true;
        foreach (Transform child in transform)
        {
            //corrupt child
            print("corrupted " + child.name);
        }
    }

    void Update()
    {
        if(isCorrupted)
        {
            transform.parent = GameObject.FindGameObjectWithTag("CorruptedGrid").transform;
        }
    }

    public void CheckCorrupt()
    {
        print("checking corruption of " + gameObject.name);
        if(!isCorrupted)
        {
            //checking how many adjacent tiles are corrupted
            int borderingcorruptions = 0;
            GameObject temp;
            temp = GameObject.Find("Grid(" + (xValue + 1) + ")(" + zValue + ")");
            if (temp != null)
            {
                if (temp.GetComponent<GridObject>().isCorrupted)
                {
                    borderingcorruptions++;
                }
            }
            temp = GameObject.Find("Grid(" + (xValue - 1) + ")(" + zValue + ")");
            if (temp != null)
            {
                if (temp.GetComponent<GridObject>().isCorrupted)
                {
                    borderingcorruptions++;
                }
            }
            temp = GameObject.Find("Grid(" + xValue + ")(" + (zValue + 1) + ")");
            if (temp != null)
            {
                if (temp.GetComponent<GridObject>().isCorrupted)
                {
                    borderingcorruptions++;
                }
            }
            temp = GameObject.Find("Grid(" + xValue + ")(" + (zValue - 1) + ")");
            if (temp != null)
            {
                if (temp.GetComponent<GridObject>().isCorrupted)
                {
                    borderingcorruptions++;
                }
            }


            //if there are corrupted adjacent tiles
            if (borderingcorruptions > 0)
            {
                int odds = Random.Range(0, 1000 / borderingcorruptions);
                {
                    if(odds <= 25)
                    {
                        Corrupt();
                    }
                }
            }
        }
        

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            other.transform.parent = transform;
        }
    }
}
