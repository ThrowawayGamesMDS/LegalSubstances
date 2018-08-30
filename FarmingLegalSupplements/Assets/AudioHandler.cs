﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioClip[] m_vecacAudioClips;
    // clip 0 = successful purhcase
    public AudioSource m_asSoundSource;
	// Use this for initialization
	void Start ()
    {
    }

    public void PlaySound(string _sound)
    {
        switch(_sound)
        {
            case "PurchaseOk":
                {
                    m_asSoundSource.clip = m_vecacAudioClips[0];
                    m_asSoundSource.Play();
                    break;
                }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
