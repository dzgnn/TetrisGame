using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public delegate void TouchEventDelegate(Vector2 swipePos);

    public static event TouchEventDelegate DragEvent;
    public static event TouchEventDelegate SwipeEvent;
    public static event TouchEventDelegate TapEvent;

    private float tapMax=0f;
    public float tapScrenTime=.1f;

    private Vector2 touchMove;

    [Range(20,250)]
    public int minSwipe = 50;


    [Range(50,250)]
    public int minDrag = 100;

    
    public bool isTextActive = false;

    void Start()
    {
        
    }
    

    string SwipeKnow(Vector2 swipeMove)
    {
        string direction = "";

        if (Mathf.Abs(swipeMove.x) > Mathf.Abs(swipeMove.y))
        {
            direction = (swipeMove.x>=0)?"right":"left";

        }
        else
        {
            direction = (swipeMove.y>=0)?"top":"bot";
        }

        return direction;
    }

    void SwipeFNC()
    {
        if (DragEvent != null)
        {
            DragEvent(touchMove);
        }
    }

    void SwipeEndFNC()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(touchMove);
        }
    }

    void TapFNC()
    {
        if (TapEvent != null)
        {
            TapEvent(touchMove); 
        }
    }


    void Update()
    {
        if(Input.touchCount>0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase==TouchPhase.Began)
            {
                touchMove=Vector2.zero;
                tapMax=Time.time+tapScrenTime;
                
            }
            else if(touch.phase==TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchMove += touch.deltaPosition;

                if (touchMove.magnitude > minDrag)
                {
                    SwipeFNC();
                    
                }
            }
            else if (touch.phase==TouchPhase.Ended)
            {
                if (touchMove.magnitude > minSwipe)
                {
                    SwipeEndFNC();
                }
                else if (Time.time < tapMax)
                {
                    TapFNC();
                }
                
                
            }
        }
    }
}
