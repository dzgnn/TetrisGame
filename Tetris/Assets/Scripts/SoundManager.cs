using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioClip randomMusicClip;

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] fxVoice;
    [SerializeField] private AudioSource[] vocalVoice;


    public IconManager musicIcon;
    public IconManager fxIcon;

    public bool isFxActive=true;
    public bool isMusicActive=true;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        randomMusicClip =  RandomChooseMusic(musicClips);
        BackgroundMusic(randomMusicClip);
    }

    public void VocalPlay()
    {
        if (isFxActive)
        {
            AudioSource source = vocalVoice[Random.Range(0,vocalVoice.Length)];
            source.Stop();
            source.Play();
        }
    }
    public void FxPlay(int whichfx)
    {
        if (isFxActive)
        {
            fxVoice[whichfx].volume = PlayerPrefs.GetFloat("FxVolume", 0);
            fxVoice[whichfx].Stop();
            fxVoice[whichfx].Play();
        }
    }

    AudioClip RandomChooseMusic(AudioClip[] clips)
    {
        AudioClip randomClip= clips[Random.Range(0,clips.Length)];
        return randomClip;
    }

    public void BackgroundMusic(AudioClip clipp)
    {
        if (!clipp || !musicSource || !isMusicActive)
        {
            return;
        }

        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        musicSource.clip = clipp;
        musicSource.Play();
    }


    void MusicUpdate()
    {
        if (musicSource.isPlaying != isMusicActive)
        {
            if (isMusicActive)
            {
                randomMusicClip=RandomChooseMusic(musicClips);
                BackgroundMusic(randomMusicClip);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }


    public void BackgroundMusicOnOff()
    {
        isMusicActive = !isMusicActive;
        MusicUpdate();
        musicIcon.IconTurn(isMusicActive);
    }

    public void FxMusicOnOff()
    {
        isFxActive = !isFxActive;
        fxIcon.IconTurn(isFxActive);
    }
}
