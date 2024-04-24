using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class IntroManager : MonoBehaviour
{
    public GameObject[] numbers;

    public GameObject numbersTransform;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        Rou();
    }

    public void Rou()
    {
        StartCoroutine(OpenNumbersRoutine());
    }

    public IEnumerator OpenNumbersRoutine()
    {
        yield return new WaitForSeconds(.1f);
        numbersTransform.GetComponent<RectTransform>().DORotate(Vector3.zero, .3f).SetEase(Ease.OutBack);
        numbersTransform.GetComponent<CanvasGroup>().DOFade(1, .3f);
        
        yield return new WaitForSeconds(.2f);
        
        int timer = 0;
        while (timer<numbers.Length)
        {
            numbers[timer].GetComponent<RectTransform>().DOLocalMoveY(0,.5f);
            numbers[timer].GetComponent<CanvasGroup>().DOFade(1, .5f);

            numbers[timer].GetComponent<RectTransform>().DOScale(2f, .3f).SetEase(Ease.OutBounce).OnComplete(()=>
                numbers[timer].GetComponent<RectTransform>().DOScale(.6f,.3f).SetEase(Ease.InBack));

            yield return new WaitForSeconds(1.5f);

            timer++;

            numbers[timer-1].GetComponent<RectTransform>().DOLocalMoveY(150,.5f);

            yield return new WaitForSeconds(.1f);
        }

        numbersTransform.GetComponent<CanvasGroup>().DOFade(0,.5f).OnComplete(() =>
        {
            numbersTransform.transform.parent.gameObject.SetActive(false);
            gameManager.StartGame();
        }
        );

    }
}
