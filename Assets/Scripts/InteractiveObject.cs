using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveObject: MonoBehaviour
{
    [Header("Если Хотим Писать текст Прикрепляем UseBox и UseText")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text infoPanelText;
    [SerializeField] private string infoText;

    // Можно подписаться из скрипта на этом же обьекте на это событие
    [HideInInspector]public UnityEvent onVision;
    [HideInInspector]public UnityEvent onHide;
    [HideInInspector]public UnityEvent onUse;
    public UnityAction<InteractiveObject> Ondestroy;

    private float timer ;
    private float timeBoxHide = 0.2f;

    private void Start()
    {
        if (infoPanel!)
        {
            infoPanel.SetActive(false);
        }
        enabled = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBoxHide)
        {
            infoPanel.SetActive(false);
            timer = 0;
            enabled = false;
        }
    }

    public void ShowInfoPanel()
    {
        infoPanelText.text = infoText;
        timer = 0;
        infoPanel.SetActive(true);
        enabled = true;
    }
    
    private void HideInfoPanel()
    {
        infoPanel.SetActive(true);
    }
    /// <summary>
    ///Вызывается когда камера игрока видит обьект
    /// </summary>
    public void Visible()
    {
        onVision.Invoke();
    }
    
    /// <summary>
    /// Вызывается когда камера игрока не видит объект
    /// </summary>
    public void Hide()
    {
        onHide.Invoke();
    }
    
    /// <summary>
    /// Вызывается когда игрок Применил действие
    /// </summary>
    public void Use()
    {
        onUse.Invoke();
    }

    private void OnDestroy()
    {
        Ondestroy.Invoke(this);
    }
}