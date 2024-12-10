using UnityEngine;
using UnityEngine.Events;

public class NoiseLevel : SingletonBase<NoiseLevel>
{
    public event UnityAction<int> OnChange;
    
    [SerializeField] private int maxLevel;
    [SerializeField] private float resetTime;

    public int MaxLevel => maxLevel;
    public int CurrentLevel { get; private set; }

    private float timer;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateTimer();
        DecreaseLevel();
    }

    public void IncreaseLevel()
    {
        if (CurrentLevel < maxLevel)
        {
            CurrentLevel++;
            OnChange?.Invoke(CurrentLevel);
            timer = resetTime;
        }
    }

    public void DecreaseLevel()
    {
        if (CurrentLevel > 0 && timer <= 0)
        {
            CurrentLevel--;
            OnChange?.Invoke(CurrentLevel);
            timer = resetTime;
        }
    }

    public void SetZeroLevel()
    {
        CurrentLevel = 0;
        OnChange?.Invoke(CurrentLevel);
    }

    public void SetMaxLevel()
    {
        CurrentLevel = maxLevel;
        OnChange?.Invoke(CurrentLevel);
        timer = resetTime;
    }
    private void UpdateTimer()
    {
        if(timer>0)
            timer -= Time.deltaTime;
        else
            timer = 0;
    }
}
