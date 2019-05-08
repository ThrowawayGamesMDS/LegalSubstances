using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class queueBar : MonoBehaviour
{
    public static queueBar m_sQueueBarHandle;
    public HomeSpawning home;
    public RectTransform me;
    public RectTransform bar;
    public float m_fTimerValue;
    // Start is called before the first frame update
    void Start()
    {
        if (m_sQueueBarHandle == null)
        {
            m_sQueueBarHandle = this;
            home = GameObject.FindGameObjectWithTag("HomeBuilding").GetComponent<HomeSpawning>();
            me = gameObject.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bar.localScale = new Vector3(home.timerVal/ m_fTimerValue, 1,1);
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
