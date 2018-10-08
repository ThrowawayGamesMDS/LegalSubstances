using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
    public int xValue;
    public int zValue;
    public bool isCorrupted;
    public MeshRenderer rnd;
    public GameObject tobeccorrupted;
    // Use this for initialization
    void Start()
    {
        tobeccorrupted = GameObject.FindGameObjectWithTag("ToBeCorrupted");
        //InvokeRepeating("CheckCorrupt", 0.1f, 3);
        if (!isCorrupted)
        {
            if(BorderingCorrupted() > 0)
            {
                tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Insert(tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Count, gameObject);
            }
        }
    }
	
    public void Corrupt()
    {
        transform.parent = GameObject.FindGameObjectWithTag("CorruptedGrid").transform;
        isCorrupted = true;
        rnd.enabled = true;
        tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Remove(gameObject);


        foreach (Transform child in transform)
        {
            //corrupt child
            print("corrupted " + child.name);
        }
        
        GameObject temp;
        temp = GameObject.Find("Grid(" + (xValue + 1) + ")(" + zValue + ")");
        if (temp != null)
        {
            if (!temp.GetComponent<GridObject>().isCorrupted)
            {
                tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Insert(tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Count, temp);
            }
        }
        temp = GameObject.Find("Grid(" + (xValue - 1) + ")(" + zValue + ")");
        if (temp != null)
        {
            if (!temp.GetComponent<GridObject>().isCorrupted)
            {
                tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Insert(tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Count, temp);
            }
        }
        temp = GameObject.Find("Grid(" + xValue + ")(" + (zValue + 1) + ")");
        if (temp != null)
        {
            if (!temp.GetComponent<GridObject>().isCorrupted)
            {
                tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Insert(tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Count, temp);
            }
        }
        temp = GameObject.Find("Grid(" + xValue + ")(" + (zValue - 1) + ")");
        if (temp != null)
        {
            if (!temp.GetComponent<GridObject>().isCorrupted)
            {
                tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Insert(tobeccorrupted.GetComponent<ToBeCorruptedScript>().tobeCorrupted.Count, temp);
            }
        }

    }

    void Update()
    {
        if(isCorrupted)
        {
            transform.parent = GameObject.FindGameObjectWithTag("CorruptedGrid").transform;
            rnd.enabled = true;
        }
    }

    public int BorderingCorrupted()
    {
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

        return borderingcorruptions;
    }


    public void CheckCorrupt()
    {
        //print("checking corruption of " + gameObject.name);
        if(!isCorrupted)
        {
            
            int value = BorderingCorrupted();
            //if there are corrupted adjacent tiles
            if (value > 0)
            {
                int odds = Random.Range(0, 80000);
                {
                    switch(value)
                    {
                        case 1:
                        {
                            if(odds < 5)
                            {
                                Corrupt();
                            }
                            break;
                        }
                        case 2:
                        {
                            if (odds < 50)
                            {
                                Corrupt();
                            }
                            break;
                        }
                        case 3:
                        {
                            if (odds < 1000)
                            {
                                Corrupt();
                            }
                            break;
                        }
                        case 4:
                        {
                            Corrupt();
                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }
                    if(odds <= 1000)
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
