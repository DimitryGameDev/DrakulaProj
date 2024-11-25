using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractiveObject: VisibleObject
{
    [Header("Settings")]
    [SerializeField] private string infoText;
    [SerializeField] private string infoTextAfterUse;
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite afterIcon;
    [Tooltip("Как быстро скрывается UI, после отвода камеры")][SerializeField] private float  timeBoxHide = 0.1f;
    [Tooltip("Как быстро скрывается UI ,после применения")][SerializeField] private float  timeAfterText = 3f;
    
    protected AudioSource AudioSource;
    protected bool wosActive;
    public bool WosActive => wosActive;
    
    private float time ;
    private float timer ;
    private bool isText;
    private bool isAfterText;
    public bool IsAfterText => isAfterText;
    
    protected InteractiveBoxUI InteractiveBoxUI;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        InteractiveBoxUI = InteractiveBoxUI.Instance;
        isText = false;
    }

    protected virtual void Update()
    {
        if (isText)
        {
            time += Time.deltaTime;
            if (time >= timer)
            {
                HideInfoPanel();
            }
        }
    }

    /// <summary>
    /// Показывает текст при наведении на обьект
    /// </summary>
    public virtual void ShowText()
    {
        InteractiveBoxUI.Enable();
        InteractiveBoxUI.text.text = infoText;
        if (icon != null)InteractiveBoxUI.icon.sprite = icon;
        else InteractiveBoxUI.HideIcon();
        timer = timeBoxHide;
        isText = true;
    }
    
    /// <summary>
    /// Показывает текст После применения
    /// </summary>
    protected virtual void ShowAfterText()
    {
        InteractiveBoxUI.Enable();
        InteractiveBoxUI.text.text = infoTextAfterUse;
        if (icon != null) InteractiveBoxUI.icon.sprite = afterIcon;
        else InteractiveBoxUI.HideIcon();
        InteractiveBoxUI.HideCursor();
        time = 0;
        timer = timeAfterText; 
        isText = true;
        isAfterText = true;
    }
    
    /// <summary>
    /// Убирает UI когда игрок отвел мышь
    /// </summary>
    protected virtual void HideInfoPanel()
    {
        InteractiveBoxUI.Disable();
        time = 0;
        isText = false;
        isAfterText = false;
    }

    /// <summary>
    /// Вызывается когда игрок Применил действие
    /// </summary>
    public virtual void Use() { }

    protected virtual void SetInfoText(string text)
    {
        infoText = text;
    }
    
    protected virtual void SetInfoTextAfterUse(string text)
    {
        infoTextAfterUse = text;
    }

    #region SaveLogic

    public void SaveState()
    {
        InteractiveState.Instance.Save(this,wosActive);
    }
    
    public void LoadState()
    {
        wosActive = InteractiveState.Instance.GetState(this);
        if (wosActive) ObjectWosActive();
    }

    protected virtual void ObjectWosActive() { }
    
    #endregion
   
}