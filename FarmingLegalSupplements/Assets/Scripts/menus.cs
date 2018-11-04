using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class menus : MonoBehaviour {
    public Slider Vol,Mus,SF;
    public AudioMixer mix;
    // Use this for initialization
    void Start() {
        float temp;
        mix.GetFloat("master", out temp);
        Vol.value = temp;
        mix.GetFloat("music", out temp);
        Mus.value = temp;
        mix.GetFloat("sf", out temp);
        SF.value = temp;
    }

    // Update is called once per frame
    void Update() {

    }

    public void mainVolChange(){
        if (Vol.value == 0)
        {
            mix.SetFloat("master", -80);
        }
        else
        {
            mix.SetFloat("master", Vol.value);
        }
    }
    public void musicVolChange()
    {
      
        if (Mus.value == 0)
        {
            mix.SetFloat("music", -80);
        }
        else
        {
            mix.SetFloat("music", Mus.value);
        }
    }
    public void SFVolChange()
    {
        if (SF.value == 0)
        {
            mix.SetFloat("sf", -80);
        }
        else
        {
            mix.SetFloat("sf", SF.value);
        }
    }
}
