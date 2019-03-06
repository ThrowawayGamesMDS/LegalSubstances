using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeScript : MonoBehaviour
{

    public List<GameObject> treeNodes;














    public void scrollUI(Scrollbar slider)
    {
        transform.localPosition = new Vector3(((slider.value * -567) - 400), 18, 0);
    }
}
