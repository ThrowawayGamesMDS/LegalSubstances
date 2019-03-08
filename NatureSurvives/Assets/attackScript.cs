using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScript : MonoBehaviour
{
    public GameObject AOEObj;
    public Transform handpos;
    public void AttackAOE()
    {
        Instantiate(AOEObj, handpos.position, AOEObj.transform.rotation);
    }
}
