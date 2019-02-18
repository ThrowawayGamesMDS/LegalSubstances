using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updatePriceUI : MonoBehaviour {
    public Text wood, crystal, food, Name;
    public float fwood, fcrystal, ffood;
    public string sName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        wood.text = fwood.ToString();
        crystal.text = fcrystal.ToString();
        food.text = ffood.ToString();
        Name.text = sName;
	}
    public void UpdateFloats(float one, float two, float three, string name)
    {
        fwood = one;
        fcrystal = two;
        ffood = three;
        sName = name;

    }

}
