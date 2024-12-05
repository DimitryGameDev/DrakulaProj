using System;
using UnityEngine;
using UnityEngine.UI;

public class AimUI : SingletonBase<AimUI>
{
    [SerializeField]private Image fillImage;
    private void Awake()
    {
        Init();
    }

    public void Fill(float value)
    {
        fillImage.fillAmount = value;
    }
}
