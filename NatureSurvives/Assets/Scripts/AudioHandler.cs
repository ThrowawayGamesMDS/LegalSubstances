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
    public static AudioHandler m_ahHandler;
    public AudioClip[] m_arrSuccessSounds;
    public AudioClip[] m_arrFailureSounds;
    public AudioClip[] m_arrDamageSounds;
    public AudioClip[] m_arrWoodcutSounds;
    public AudioClip[] m_arrMiningSounds;
    public AudioClip[] m_arrDayMusic;
    public AudioClip[] m_arrNightMusic;
    public AudioClip[] m_arrMasterMusic;
    public AudioSource m_asPlayerSF;
    public AudioSource m_asMusicMaster;
    private bool m_bPlaySound;
    private bool m_bDayMusicPlaying;
    public enum m_soundTypes
    {
        DAMAGE, SUCCESS, FAILURE, WOOD, MINE, WARNING, MUSIC
    };

    public void PlaySound(m_soundTypes _playType)
    {

        if (_playType != m_soundTypes.MUSIC) // Is a sound effect
        {
            if (m_asPlayerSF.isPlaying) // Sound effect is playing, we don't want to update the effect?..
            {
                return; // could add to like a queue system here, but fuck having delayed sounds lol
            }
            else
            {
                //alter pitch of music?
                m_asMusicMaster.pitch = m_asMusicMaster.pitch = 0.5f;
            }
        }
        
        int _play;
        switch (_playType)
        {
            case m_soundTypes.MUSIC:
                {
                    if (DayNight.isDay) 
                    {
                        _play = Random.Range(0, m_arrDayMusic.Length);
                        m_asMusicMaster.clip = m_arrDayMusic[_play];
                        m_bDayMusicPlaying = true;
                        //invoke song reset at clip length to avoid an update func?
                    }
                    else
                    {
                        _play = Random.Range(0, m_arrNightMusic.Length);
                        m_asMusicMaster.clip = m_arrNightMusic[_play];
                        m_bDayMusicPlaying = false;
                    }
                    break;
                }
            case m_soundTypes.DAMAGE:
                {
                    _play = Random.Range(0, m_arrDamageSounds.Length);
                    m_asPlayerSF.clip = m_arrDamageSounds[_play];
                    break;
                }

            case m_soundTypes.SUCCESS:
                {
                    _play = Random.Range(0, m_arrSuccessSounds.Length);
                    m_asPlayerSF.clip = m_arrSuccessSounds[_play];
                    break;
                }

            case m_soundTypes.FAILURE:
                {
                    _play = Random.Range(0, m_arrFailureSounds.Length);
                    m_asPlayerSF.clip = m_arrFailureSounds[_play];
                    break;
                }
            case m_soundTypes.WOOD:
                {
                    _play = Random.Range(0, m_arrWoodcutSounds.Length);
                    m_asPlayerSF.clip = m_arrWoodcutSounds[_play];
                    break;
                }
            case m_soundTypes.MINE:
                {
                    _play = Random.Range(0, m_arrMiningSounds.Length);
                    m_asPlayerSF.clip = m_arrMiningSounds[_play];
                    break;
                }
        }

        switch(_playType)
        {
            case m_soundTypes.MUSIC:
                {
                    m_asMusicMaster.Play();
                    break;
                }
            default:
                m_asPlayerSF.Play();
                break;
        }
    }


    public void UpdatedPlaySound(m_soundTypes _playType, AudioSource _asUpdateAudio)
    {

        if (!_asUpdateAudio.isPlaying)
        {
            int _play;
            switch (_playType)
            {
                case m_soundTypes.DAMAGE:
                    {
                        _play = Random.Range(0, m_arrDamageSounds.Length);
                        _asUpdateAudio.clip = m_arrDamageSounds[_play];
                        break;
                    }

                case m_soundTypes.SUCCESS:
                    {
                        _play = Random.Range(0, m_arrSuccessSounds.Length);
                        _asUpdateAudio.clip = m_arrSuccessSounds[_play];
                        break;
                    }

                case m_soundTypes.FAILURE:
                    {
                        _play = Random.Range(0, m_arrFailureSounds.Length);
                        _asUpdateAudio.clip = m_arrFailureSounds[_play];
                        break;
                    }
                case m_soundTypes.WOOD:
                    {
                        _play = Random.Range(0, m_arrWoodcutSounds.Length);
                        _asUpdateAudio.clip = m_arrWoodcutSounds[_play];
                        break;
                    }
                case m_soundTypes.MINE:
                    {
                        _play = Random.Range(0, m_arrMiningSounds.Length);
                        _asUpdateAudio.clip = m_arrMiningSounds[_play];
                        break;
                    }
            }
            _asUpdateAudio.Play();
        }
    }

    // Use this for initialization
    void Start()
    {
        if (m_ahHandler == null)
        {
            m_ahHandler = this;
            m_arrSuccessSounds = Resources.LoadAll("Audio/Success", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            m_arrFailureSounds = Resources.LoadAll("Audio/Failure", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            m_arrDamageSounds = Resources.LoadAll("Audio/Damage", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            m_arrWoodcutSounds = Resources.LoadAll("Audio/Woodcut", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            m_arrMiningSounds = Resources.LoadAll("Audio/Mining", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            m_arrDayMusic = Resources.LoadAll("Audio/Music/Day", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            m_arrNightMusic = Resources.LoadAll("Audio/Music/Night", typeof(AudioClip)).Cast<AudioClip>().ToArray();


            /***
             * 
             * Weird bug - READ THE PRINT
             * 
             ***/
            m_asPlayerSF = null;
            m_asMusicMaster = null;
            var _audioSources = GetComponents(typeof(AudioSource)).Cast<AudioSource>().ToArray();
         if (_audioSources.Length == 1)
            {
                print("PLEASE RE-ASSIGN THE AUDIOHANDLER COMPONENT TO THE PLAYER OBJECT!!!");
            }
            m_asPlayerSF = _audioSources[0];
            m_asMusicMaster = _audioSources[1];

            m_asMusicMaster.pitch = 0.6f;
            m_asMusicMaster.volume = 0.2f;

            m_bDayMusicPlaying = false;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_asMusicMaster != null)
        {
            if (!m_asMusicMaster.isPlaying)
            {
                PlaySound(m_soundTypes.MUSIC);
            }

            if (!m_bDayMusicPlaying &&  DayNight.isDay )
            {
                PlaySound(m_soundTypes.MUSIC);
            }
            else if (m_bDayMusicPlaying && !DayNight.isDay)
            {
                PlaySound(m_soundTypes.MUSIC);
            }

        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaySound(m_soundTypes.MUSIC);
        }
       
    }
}
