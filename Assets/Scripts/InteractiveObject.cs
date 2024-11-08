using UnityEngine;

public class InteractiveObject: VisibleObject
{
    [Header("Settings")]
    [SerializeField] private string infoText;
    [SerializeField] private string infoTextAfterUse;
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite afterIcon;
    [Tooltip("Как быстро скрывается UI, после отвода камеры")][SerializeField] private float  timeBoxHide = 0.1f;
    [Tooltip("Как быстро скрывается UI ,после применения")][SerializeField] private float  timeAfterText = 3f;
    
    private float time ;
    private float timer ;
    private bool isText;
    private bool isAfterText;
    public bool IsAfterText => isAfterText;
    
    private InteractiveBoxUI interactiveBoxUI;
    protected virtual void Start()
    {
        interactiveBoxUI = InteractiveBoxUI.Instance;
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
        interactiveBoxUI.Enable();
        interactiveBoxUI.text.text = infoText;
        if (icon != null)interactiveBoxUI.icon.sprite = icon;
        else interactiveBoxUI.HideIcon();
        timer = timeBoxHide;
        isText = true;
    }
    
    /// <summary>
    /// Показывает текст После применения
    /// </summary>
    protected virtual void ShowAfterText()
    {
        interactiveBoxUI.Enable();
        interactiveBoxUI.text.text = infoTextAfterUse;
        if (icon != null) interactiveBoxUI.icon.sprite = afterIcon;
        else interactiveBoxUI.HideIcon();
        interactiveBoxUI.HideCursor();
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
        interactiveBoxUI.Disable();
        time = 0;
        isText = false;
        isAfterText = false;
    }
    
    /// <summary>
    /// Вызывается когда игрок Применил действие
    /// </summary>
    public virtual void Use(){}
}