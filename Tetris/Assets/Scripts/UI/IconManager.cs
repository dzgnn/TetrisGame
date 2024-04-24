using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public Sprite onIcon;
    public Sprite offIcon;

    private Image iconImg;
    public bool DefaultIcon = true;

    void Start()
    {
        iconImg =  GetComponent<Image>();
        
        if (DefaultIcon)
        {
            iconImg.sprite = onIcon;
        }
        else
        {
            iconImg.sprite = offIcon;
        }
    }

    public void IconTurn(bool iconsit)
    {
        if (!iconImg || !onIcon || !offIcon)
        {
            return;
        }
        iconImg.sprite = (iconsit) ? onIcon : offIcon;
    }

}
