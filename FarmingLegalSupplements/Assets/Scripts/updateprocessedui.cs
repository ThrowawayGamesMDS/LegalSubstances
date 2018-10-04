using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateprocessedui : MonoBehaviour {
    public bool isGreen;

    // Update is called once per frame
    void Update()
    {
        if (isGreen)
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HouseController>().GreenProcessed.ToString();

        }
        if (!isGreen)
        {
            gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HouseController>().WhiteProcessed.ToString();

        }
    }
}
