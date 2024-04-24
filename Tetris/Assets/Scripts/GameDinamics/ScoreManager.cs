using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int score = 0;
    int lines;
    public int level = 1;
    int linesOnLevel = 5;

    public TextMeshProUGUI linesTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI scoreTxt;

    public bool isLevelPassed = false;
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        level = 1;
        lines = linesOnLevel*level;
        TextUpdate();
    }

    public void LineScore(int n)
    {
        isLevelPassed = false;
        n= Mathf.Clamp(n,1,4);

        switch (n)
        {
            case 1:
                score += 30 * level;
                break;
            case 2:
                score += 50 * level;
                break;
            case 3:
                score += 100 * level;
                break;
            case 4:
                score += 400 * level;
                break;
        }

        lines -= n;

        if (lines <= 0)
        {
            LevelUp();
        }

        TextUpdate();
    }

    void TextUpdate()
    {
        scoreTxt.text = AddZero(score,5);
        levelTxt.text = level.ToString();
        linesTxt.text = lines.ToString();
    }

    string AddZero(int point,int zero)
    {
        string pointStr = point.ToString();
        while (pointStr.Length<zero)
        {
            pointStr = "0" + pointStr;
        }

        return pointStr;

    }

    public void LevelUp()
    {
        level++;
        lines = linesOnLevel * level;
        isLevelPassed = true;
    }
}
