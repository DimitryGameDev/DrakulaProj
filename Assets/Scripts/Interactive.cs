using UnityEngine;
using UnityEngine.Events;

public class Interactive: MonoBehaviour
{
    // Можно подписаться из скрипта на этом же обьекте на это событие
    public UnityEvent onVision;
    public UnityEvent onHide;
    public UnityEvent onUse;
    
    /// <summary>
    ///Вызывается когда камера игрока видит одбьект
    /// </summary>
    public void Visible()
    {
        onVision.Invoke();
    }
    
    /// <summary>
    /// Вызывается когда камера игрока не видит одбьект
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
}