using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class queueBar : MonoBehaviour
{
    public HomeSpawning home;
    public RectTransform me;
    public RectTransform bar;
    // Start is called before the first frame update
    void Start()
    {
        home = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>();
        me = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.localScale = new Vector3(home.timerVal/7, 1,1);
        if(home.UIObjQueue.Count<=0)
        {
            gameObject.GetComponent<Image>().enabled = false;
            bar.gameObject.GetComponent<Image>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = true;
            bar.gameObject.GetComponent<Image>().enabled = true;
        }
    }
}
