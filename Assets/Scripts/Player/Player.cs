using UnityEngine;
using UnityEngine.Events;

public class Player : SingletonBase<Player>
{
    [SerializeField] private float stress;
    public float CurrentStress => stress;

    //[SerializeField] private float money;
    
    private float maxStress = 100f;
    public float MaxStress => maxStress;
    
    public UnityEvent changeStress;
    public UnityEvent onStress;

    
    /*
    public float CurrentMoney => money;

    public UnityEvent ChangeMoney;

    public void AddMoney(float value)
    {
        money += value;
        ChangeMoney.Invoke();
    }

    public void RemoveMoney(float value)
    {
        money -= value;
        ChangeMoney.Invoke();
    }
    */
    
    public void AddStress(float value)
    {   
        if (stress + value > maxStress)
        {           
            stress = maxStress;
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



