using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Windows;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using DG.Tweening;



public class GameManager : MonoBehaviour
{
    SpawnerManager spawner;
    BoardManager board;
    private ShapeManager currentShape;
    FollowShapeManager followShape;
    private ShapeManager holdshape;
    public IconManager rotateicon;
    public ScoreManager scoreManager;
    public Image holdshapeImg;
    public ParticleManager[] levelUpEfcts = new ParticleManager[4];
    public ParticleManager[] gameOverEfcts = new ParticleManager[5];


    enum Direction{none,left,right,up,down}

    Direction swipeDirection = Direction.none;
    Direction swipeEndDirection = Direction.none;

    float nextTouchTime;
    float nextSwipeTime;

    [Range (0.05f, 0.5f)] public float minTouchTime = .15f;
    [Range (0.05f, 1f)] public float minSwipeTime = .3f;
    bool isTouch =false;


    [Header("Timer")]

    [Range(0.1f,1)]
    [SerializeField] private float downRate = .5f;
    private float downMeter;
    private float downLevelRate;

    [Range(0.1f,1)]
    [SerializeField] private float rightLeftTime = 0.25f;
    float rightLeftMeter;

    [SerializeField] private float turnTime = 0.25f;
    float turnMeter;

    [SerializeField] private float fastDownTime = 0.25f;
    float fastDownMeter;

    

    public bool gameOver=false;
    public bool isClockDirection;
    bool holdshapechange = true;

    public GameObject GameOverPanel;




    void Awake()
    {
        spawner = FindObjectOfType<SpawnerManager>();
        board = FindObjectOfType<BoardManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        followShape = FindObjectOfType<FollowShapeManager>();
    }

    void OnEnable()
    {
        TouchManager.DragEvent+= SwipeFNC;
        TouchManager.SwipeEvent+= SwipeEndFNC;
        TouchManager.TapEvent+= TapFNC;

    }

    void OnDisable()
    {
        TouchManager.DragEvent-= SwipeFNC;
        TouchManager.SwipeEvent-= SwipeEndFNC;
        TouchManager.TapEvent-= TapFNC;
    }
    public void StartGame()
    {
        if (spawner!=null)
        {
            spawner.AllNull();
            if (currentShape==null)
            {
                currentShape = spawner.CreateShape();
                currentShape.transform.position = VectorToInt(currentShape.transform.position);
            }

            if (holdshape==null)
            {
                holdshape = spawner.HoldShapeCreate();

                if (currentShape.name == holdshape.name)
                {
                    Destroy(holdshape.gameObject);
                    holdshape=spawner.HoldShapeCreate();

                    holdshapeImg.sprite = holdshape.shapeObj;
                    holdshape.gameObject.SetActive(false);
                }
                else
                {
                    holdshapeImg.sprite = holdshape.shapeObj;
                    holdshape.gameObject.SetActive(false);
                }
                
            }
        }

        GameOverPanel.SetActive(false);
        downLevelRate = downRate;
    }

  

    void Update()
    {
        if (!spawner || !board || gameOver==true || !scoreManager || !currentShape)
        {
            return;
        }

        MoveControl();
        
        
    }

    private void LateUpdate()
    {
        if (!currentShape || !spawner || !board || gameOver==true || !scoreManager || !followShape)
        {
            return;
        }

        if (followShape)
        {
            followShape.CreateFollowShape(currentShape,board);
        }        
    }

    void MoveControl()
    {
        if ((UnityEngine.Input.GetKey(KeyCode.D) && Time.time>rightLeftMeter) || UnityEngine.Input.GetKeyDown(KeyCode.D))
        {
            RightMoveFnc();

        }
        else if ((UnityEngine.Input.GetKey(KeyCode.A) && Time.time>rightLeftMeter) || UnityEngine.Input.GetKeyDown(KeyCode.A))
        {
            LeftMoveFnc();

        }
        else if ((UnityEngine.Input.GetKeyDown(KeyCode.W) && Time.time>turnMeter))
        {
            RotateFnc();

        }
        else if (((UnityEngine.Input.GetKey(KeyCode.S) && Time.time>fastDownMeter)) || Time.time > downMeter)
        {
            DownFastFnc();
        }

        else if((swipeEndDirection == Direction.right && Time.time>nextSwipeTime) || 
                (swipeDirection == Direction.right && Time.time>nextTouchTime))
        { 
            RightMoveFnc();
            nextTouchTime=Time.time+minTouchTime;
            nextSwipeTime=Time.time+minSwipeTime;
            //swipeDirection = Direction.none;
            //swipeEndDirection = Direction.none;
        }
        else if((swipeEndDirection == Direction.left && Time.time>nextSwipeTime) || 
                (swipeDirection == Direction.left && Time.time>nextTouchTime))
        { 
            LeftMoveFnc();
            nextTouchTime=Time.time+minTouchTime;
            nextSwipeTime=Time.time+minSwipeTime;
            //swipeDirection = Direction.none;
            //swipeEndDirection = Direction.none;
        }
        else if((swipeEndDirection == Direction.up && Time.time>nextSwipeTime) || (isTouch))
        { 
            RotateFnc();
            nextSwipeTime=Time.time+minSwipeTime;
            //swipeEndDirection = Direction.none;
        }
        else if(swipeDirection == Direction.down && Time.time>nextTouchTime)
        { 
            DownFastFnc();
            nextTouchTime=Time.time+minTouchTime;
            //swipeDirection = Direction.none;
            
        }

        swipeDirection = Direction.none;
        swipeEndDirection = Direction.none;
        isTouch =false;

    }

    private void DownFastFnc()
    {
        downMeter = Time.time + downLevelRate;
        fastDownMeter = Time.time + fastDownTime;

        if (currentShape)
        {
            currentShape.MoveDown();

            if (!board.RightPosition(currentShape))
            {
                if (board.IsGameOver(currentShape))
                {
                    currentShape.MoveUp();
                    gameOver = true;
                    if (GameOverPanel)
                    {
                        StartCoroutine(GameOverRoutine());
                    }
                    SoundManager.instance.FxPlay(6);

                }
                else
                {
                    Settle();
                    SoundManager.instance.FxPlay(5);
                }

            }
        }
    }

    private void RotateFnc()
    {
        currentShape.TurnRight();
        turnMeter = Time.time + turnTime;

        if (!board.RightPosition(currentShape))
        {
            SoundManager.instance.FxPlay(3);
            currentShape.TurnLeft();
        }
        else
        {
            isClockDirection = !isClockDirection;

            if (rotateicon)
            {
                rotateicon.IconTurn(isClockDirection);
            }

            SoundManager.instance.FxPlay(3);
        }
    }

    private void LeftMoveFnc()
    {
        currentShape.MoveLeft();
        rightLeftMeter = Time.time + rightLeftTime;

        if (!board.RightPosition(currentShape))
        {
            SoundManager.instance.FxPlay(1);
            currentShape.MoveRight();
        }
        else
        {
            SoundManager.instance.FxPlay(3);
        }
    }

    private void RightMoveFnc()
    {
        currentShape.MoveRight();
        rightLeftMeter = Time.time + rightLeftTime;

        if (!board.RightPosition(currentShape))
        {
            SoundManager.instance.FxPlay(1);
            currentShape.MoveLeft();
        }
        else
        {
            SoundManager.instance.FxPlay(3);
        }
    }

    private void Settle()
    {
        rightLeftMeter = Time.time;
        fastDownMeter = Time.time;
        rightLeftMeter = Time.time;

        currentShape.MoveUp();
        board.ShapeToGrid(currentShape);

        holdshapechange = true;

        if (spawner)
        {
            currentShape = spawner.CreateShape();

            holdshape = spawner.HoldShapeCreate();
                
            if (currentShape.name == holdshape.name)
            {
                Destroy(holdshape.gameObject);
                holdshape=spawner.HoldShapeCreate();

                holdshapeImg.sprite = holdshape.shapeObj;
                holdshape.gameObject.SetActive(false);
            }
            else
            {
                holdshapeImg.sprite = holdshape.shapeObj;
                holdshape.gameObject.SetActive(false);
            }
            
        }

        if (followShape)
        {
            followShape.ResetFnc();
        }

        StartCoroutine(board.CleanAllLine());

        if (board.completedLine > 0)
        {
            scoreManager.LineScore(board.completedLine);

            if (scoreManager.isLevelPassed)
            {
               SoundManager.instance.FxPlay(2);
               downLevelRate = downRate-Math.Clamp(((float)scoreManager.level-1)*0.1f, 0.05f,1f);

               StartCoroutine(LevelUpRoutine());
            }
            else
            {
                if (board.completedLine > 1)
                {
                SoundManager.instance.VocalPlay();
                }
                else
                {
                SoundManager.instance.FxPlay(4);
                }
            }

            

        }
    }

    Vector2 VectorToInt(Vector2 vector2)
    {
        return new Vector2(Mathf.Round(vector2.x),Mathf.Round(vector2.y));
    }


    public void RotationIcon()
    {
        isClockDirection = !isClockDirection;
        currentShape.IsClockDirection(isClockDirection);

        if (!board.RightPosition(currentShape))
        {
            currentShape.IsClockDirection(!isClockDirection);
            SoundManager.instance.FxPlay(3);
        }
        else
        {
            if (rotateicon)
            {
                rotateicon.IconTurn(isClockDirection);
                SoundManager.instance.FxPlay(1);

            }
        }
    }


    public void ChangeHoldedShape()
    {
        if (holdshapechange)
        {
            holdshapechange = false;

            currentShape.gameObject.SetActive(false);
            holdshape.gameObject.SetActive(true);

            holdshape.transform.position = currentShape.transform.position;
            currentShape = holdshape;
        }

        if (followShape)
        {
            followShape.ResetFnc();
        }
    }


    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(.2f);
        int count = 0;
        
        while (count < levelUpEfcts.Length)
        {
            levelUpEfcts[count].EffectPlay();
            yield return new WaitForSeconds(.1f);
            count++;
        }
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(.2f);
        int count = 0;
        
        while (count < gameOverEfcts.Length)
        {
            gameOverEfcts[count].EffectPlay();
            yield return new WaitForSeconds(.1f);
            count++;
        }

        yield return new WaitForSeconds(1f);

        GameOverPanel.transform.localScale = Vector3.zero;
        GameOverPanel.SetActive(true);
        GameOverPanel.transform.DOScale(1,.5f).SetEase(Ease.OutSine);
    }


    void SwipeFNC(Vector2 swipeMove)
    {
        swipeDirection = TakeDirection(swipeMove);
    }

    void SwipeEndFNC(Vector2 swipeMove)
    {
        swipeEndDirection = TakeDirection(swipeMove);
    }

    void TapFNC(Vector2 swipeMove)
    {
        isTouch = true;
    }


    Direction TakeDirection(Vector2 swipeMoving)
    {
        Direction swipeDirection = Direction.none;

        if (MathF.Abs(swipeMoving.x) > MathF.Abs(swipeMoving.y))
        {
            swipeDirection = swipeMoving.x>0?Direction.right:Direction.left;
        }
        else
        {
            swipeDirection = swipeMoving.y>0?Direction.up:Direction.down;
        }

        return swipeDirection;
    }




    
}
