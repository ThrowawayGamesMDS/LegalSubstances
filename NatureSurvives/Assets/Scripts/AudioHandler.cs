using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(AudioSource))]


public class Audio
{
    public List<AudioClip> m_acClips;
    public bool m_bAlterPitch;
    public float m_fNewPitch;
}
public class AudioHandler : MonoBehaviour
{
    private GameObject[] m_asSoundFiles;

    public List<AudioClip> m_lDamageSounds;
    public AudioClip[] m_arrDamageSounds;
    public AudioClip[] m_arrWoodcutSounds;
    public AudioClip[] m_arrMiningSounds;
    public List<AudioClip> m_lSuccesfulSounds;
    public List<AudioClip> m_lFailureSounds;
    public AudioSource m_asHandle;
    private bool m_bPlaySound;
    public enum m_soundTypes
    {
        DAMAGE, SUCCESS, FAILURE, WOOD, MINE, WARNING
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
                case m_soundTypes.WOOD:
                    {
                        _play = Random.Range(0, m_arrWoodcutSounds.Length);
                        m_asHandle.clip = m_arrWoodcutSounds[_play];
                        break;
                    }
                case m_soundTypes.MINE:
                    {
                        _play = Random.Range(0, m_arrMiningSounds.Length);
                        m_asHandle.clip = m_arrMiningSounds[_play];
                        break;
                    }
            }
            m_asHandle.Play();
        }
    }

    // Use this for initialization
    void Start()
    {

        //print("we here");
        //Object[] data;
        //data = AssetDatabase.LoadAllAssetsAtPath("Assets\\Sound\\Audio");
        //
        //foreach (Object o in data)
        //{
        //    Debug.Log(o);
        //    print(o.name);
        //}
        
        m_arrDamageSounds = Resources.LoadAll("Audio/Damage", typeof(AudioClip)).Cast<AudioClip>().ToArray() ;
        m_arrWoodcutSounds = Resources.LoadAll("Audio/Woodcut", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        m_arrMiningSounds = Resources.LoadAll("Audio/Mining", typeof(AudioClip)).Cast<AudioClip>().ToArray();

        /*  for (int i = 0; i < m_lDamageSounds.Count; i++)
          {
              m_arrDamageSounds[i] = m_lDamageSounds[i];
          }*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
