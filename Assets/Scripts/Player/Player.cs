using UnityEngine;
using UnityEngine.Events;

public class Player : SingletonBase<Player>
{
    [SerializeField] private float stress;
    public float CurrentStress => stress;

    private const float MaxStress = 100f;
    
    public UnityEvent changeStress;
    public UnityEvent onStress;
    
    public void AddStress(float value)
    {   
        if (stress + value > MaxStress)
        {           
            stress = MaxStress;
            changeStress.Invoke();
            onStress.Invoke();
            return;
        }
        stress += value;
        changeStress.Invoke();
    }

    public void RemoveStress(float value)
    {
        stress -= value;
        changeStress.Invoke();
    }
}



