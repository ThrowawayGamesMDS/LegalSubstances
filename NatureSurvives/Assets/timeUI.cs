using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeUI : MonoBehaviour
{
    public Text txt;
    public Transform daynight;
    // Update is called once per frame
    void Update()
    {
        float result = daynight.eulerAngles.x - Mathf.CeilToInt(daynight.eulerAngles.x / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        txt.text = ((result * 360) - 180).ToString();
    }
}
