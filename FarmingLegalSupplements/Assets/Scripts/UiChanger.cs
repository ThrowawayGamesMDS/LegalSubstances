using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChanger : MonoBehaviour {
    public GameObject wizard;
    public GameObject knight;


    public void wizardF()
    {
        wizard.SetActive(true);
        knight.SetActive(false);
    }
    public void knightF()
    {
        wizard.SetActive(false);
        knight.SetActive(true);
    }
}
