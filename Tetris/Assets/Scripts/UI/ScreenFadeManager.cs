using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScreenFadeManager : MonoBehaviour
{
    public float beginAlpha = 1f;
    public float endAlpha = 0f;

    public float waitTime = 0f;
    public float fadeTime = 1f;

    void Start()
    {
        GetComponent<CanvasGroup>().alpha = beginAlpha;
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<CanvasGroup>().DOFade(endAlpha,fadeTime);
    
    }
}
