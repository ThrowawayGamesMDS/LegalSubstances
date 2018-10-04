using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame : MonoBehaviour {
    public TurretAggro tur;
    public ParticleSystem particles;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(tur.myAIMode == TurretAggro.eAIMode.Aggro)
        {
            particles.Play();    
        }
        else
        {
            particles.Stop();
        }
	}
}
