using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : SingletonBase<Player>
{
    [SerializeField] private float stress;
    [SerializeField] private float countPieceForMedal;
    public float CurrentStress => stress;

    private const float MaxStress = 100f;

    private int medalCount;
    private int medalPieceCount;
    
    public UnityEvent changeStress;
    public UnityEvent onStress;
    public UnityEvent OnChangedMedalPiece;

    public void AddMedalPiece(int pieceCount)
    {
        if (medalPieceCount + pieceCount == countPieceForMedal)
        {
            AddMedal();
            medalPieceCount = 0;
            OnChangedMedalPiece?.Invoke();
            return;
        }
        medalPieceCount += pieceCount;
        OnChangedMedalPiece?.Invoke();
    }

    public void AddMedal()
    {
        medalCount++;
    }
    
    public void RemoveMedal()
    {
        medalCount--;
    }
    
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



