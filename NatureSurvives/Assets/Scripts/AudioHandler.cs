using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public List<AudioClip> m_lWoodCut;
    public List<AudioClip> m_lMine;
    public AudioClip[] m_arrDamageSounds;
    public List<AudioClip> m_lSuccesfulSounds;
    public List<AudioClip> m_lFailureSounds;
    public AudioSource m_asHandle;
    private bool m_bPlaySound;
    public enum m_soundTypes
    {
        DAMAGE, SUCCESS, FAILURE, WOOD, MINE
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
                        _play = Random.Range(0, m_lWoodCut.Count);
                        m_asHandle.clip = m_lWoodCut[_play];
                        break;
                    }
                case m_soundTypes.MINE:
                    {
                        _play = Random.Range(0, m_lMine.Count);
                        m_asHandle.clip = m_lMine[_play];
                        break;
                    }
            }
            m_asHandle.Play();
        }
    }

   /* private void AssignAndLoadSounds(string _sName)
    {
        Dictionary<string, Buttons> m_dButtons = new Dictionary<string, Buttons>();
        GameObject _goTemp = GameObject.FindGameObjectWithTag(_sName + "Button");
        //GameObject _goTemp = m_lgoUIButtons[0].gameObject;
        Buttons _cWongleButton = null;

        if (_goTemp != null)
        {
            _cWongleButton = new Buttons(_goTemp, _sName + "Button");
        }
        else
        {
            print("The button you were trying to add to the dictionary is null!");
        }
        if (_cWongleButton != null)
        {

            m_dButtons.Add(_sName, _cWongleButton);

            Buttons temp = null;
            if (m_dButtons.TryGetValue(_sName, out temp))
            {
                // print("Buttons loaded : Name:" + temp.m_sButtonID + ", " + temp.m_goButton.tag);
                print("Buttons loaded : Name:" + temp.m_goButton.tag);

                m_bButtonsLoaded = true;
            }
            else
            {
                print("Couldn't load buttons");
                m_bButtonsLoaded = false;
            }
        }
    }*/

    // Use this for initialization
    void Start()
    {

        print("we here");
        Object[] data;
        data = AssetDatabase.LoadAllAssetsAtPath("Assets\\Sound\\Audio");

        foreach (Object o in data)
        {
            Debug.Log(o);
            print(o.name);
        }
        
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
