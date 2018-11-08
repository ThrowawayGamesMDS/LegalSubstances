using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioHandler : MonoBehaviour
{
    public List<AudioClip> m_lDamageSounds;
    public AudioClip[] m_arrDamageSounds;
    public List<AudioClip> m_lSuccesfulSounds;
    public List<AudioClip> m_lFailureSounds;
    public AudioSource m_asHandle;
    private bool m_bPlaySound;
    public enum m_soundTypes
    {
        DAMAGE, SUCCESS, FAILURE
    };

    public void PlaySound(m_soundTypes _playType)
    {
        if (!m_asHandle.isPlaying)
        {
            int _play;
            switch (_playType)
            {
                case m_soundTypes.DAMAGE:
                    {
                        _play = Random.Range(0, m_lDamageSounds.Count);
                        m_asHandle.clip = m_lDamageSounds[_play];
                        break;
                    }

                case m_soundTypes.SUCCESS:
                    {
                        _play = Random.Range(0, m_lSuccesfulSounds.Count);
                        m_asHandle.clip = m_lSuccesfulSounds[_play];
                        break;
                    }

                case m_soundTypes.FAILURE:
                    {
                        _play = Random.Range(0, m_lFailureSounds.Count);
                        m_asHandle.clip = m_lFailureSounds[_play];
                        break;
                    }
            }
            m_asHandle.Play();
        }
    }
    // Use this for initialization
    void Start()
    {
        /*
          for (int i = 0; i < m_lDamageSounds.Count; i++)
          {
              m_arrDamageSounds[i] = m_lDamageSounds[i];
          }*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
