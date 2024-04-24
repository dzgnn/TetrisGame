using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSettings : MonoBehaviour
{
    GameManager gameManager;
    TouchManager touchManager;

    public Slider touchSlider;
    public Slider swipeSlider;
    public Slider touchSpeedSlider;


    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        touchManager = FindObjectOfType<TouchManager>();
    }

    void Start()
    {
        if (touchSlider)
        {
            touchSlider.value = 100;
            touchSlider.minValue = 50;
            touchSlider.maxValue = 150;
        }
        
        if (swipeSlider)
        {
            swipeSlider.value = 50;
            swipeSlider.minValue = 20;
            swipeSlider.maxValue = 200;
        }

        if (touchSpeedSlider)
        {
            touchSpeedSlider.value = 0.15f;
            touchSpeedSlider.minValue = 0.05f;
            touchSpeedSlider.maxValue = 0.5f;
        }
    }

    public void UpdateSettinsPanel()
    {
        if (touchSlider != null && touchManager != null)
        {
            touchManager.minDrag=(int) touchSlider.value;
        }
        if (swipeSlider!=null && touchManager!=null)
        {
            touchManager.minSwipe = (int) swipeSlider.value;
        }
        if (touchSpeedSlider!=null && gameManager!=null)
        {
            gameManager.minTouchTime = touchSpeedSlider.value;
        }
    }
}
