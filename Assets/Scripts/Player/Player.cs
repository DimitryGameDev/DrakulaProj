using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        if (stress - value >= 0)
        {
            stress -= value;
            changeStress.Invoke();
        }
    }
}



