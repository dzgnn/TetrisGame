using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu2 : MonoBehaviour
{
    
    [SerializeField] AudioSource musicSource;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        fxSlider.value = 1;
    }

    public void BackVolume()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void FxVolume()
    {
        PlayerPrefs.SetFloat("FxVolume", fxSlider.value);
    }
}

