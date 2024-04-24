using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform mainMenu, settingsMenu;
    [SerializeField] AudioSource musicSource;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;

    IntroManager introManager;

    void Awake()
    {
        introManager = FindObjectOfType<IntroManager>();
    }
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

    public void SettingsMenuOpen()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(-1200, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(0, .5f);

    }

    public void SettingsMenuClose()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(0, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(1200, .5f);
    }

    public void GamePlay()
    {
        SceneManager.LoadScene("GamePlay");
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
