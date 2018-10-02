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

    }

    // Update is called once per frame
    void Update() {

    }

    public void mainVolChange(){
        mix.SetFloat("master",(Vol.value));
       
    }
    public void musicVolChange()
    {
        mix.SetFloat("music", Mus.value);
    }
    public void SFVolChange()
    {
        mix.SetFloat("sf", SF.value);
    }
}
