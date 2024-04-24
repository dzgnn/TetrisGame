using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isGamePaused;
    public GameObject pausePanel;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }
    }

    public void PausePanelOnOff()
    {
        if (gameManager.gameOver)
        {
            return;
        }

        isGamePaused = !isGamePaused;

        pausePanel.SetActive(isGamePaused);
        SoundManager.instance.FxPlay(0);
        Time.timeScale = (isGamePaused) ? 0 : 1;
        
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HomeBtn()
    {
        SceneManager.LoadScene(0);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PausePanelOnOff();
    }
}
