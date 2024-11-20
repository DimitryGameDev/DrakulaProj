using UnityEngine;
using UnityEngine.Events;

public class VisibleObject : MonoBehaviour
{
    // Можно подписаться из скрипта на этом же обьекте на это событие
    [HideInInspector]public UnityEvent onVision;
    [HideInInspector]public UnityEvent onHide;
    public UnityAction<VisibleObject> Ondestroy;
        
    /// <summary>
    ///Вызывается когда камера игрока в режиме сердца видит обьект
    /// </summary>
    public virtual void InCamera()
    {
        onVision.Invoke();
    }
    
    /// <summary>
    /// Вызывается когда камера игрока не видит объект
    /// </summary>
    public virtual void OutCamera()
    {
        onHide.Invoke();
    }
    
    private void OnDestroy()
    {
        onHide.RemoveAllListeners();
        onVision.RemoveAllListeners();
        Ondestroy.Invoke(this);
    }

}