using UnityEngine;
using UnityEngine.Events;

public class NoiseLevel : SingletonBase<NoiseLevel>
{
    public event UnityAction<int> OnChange;
    
    [SerializeField] private int maxLevel;
    [SerializeField] private float resetTime;

    private int currentLevel;
    public int CurrentLevel => currentLevel;
    
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
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            Debug.Log("Increased");
            OnChange?.Invoke(currentLevel);
            timer = resetTime;
        }
    }

    private void DecreaseLevel()
    {
        if (currentLevel > 0 && timer <= 0)
        {
            currentLevel--;
            OnChange?.Invoke(currentLevel);
            timer = resetTime;
        }
    }

    public void SetZeroLevel()
    {
        currentLevel = 0;
    }
    
    private void UpdateTimer()
    {
        if(timer>0)
            timer -= Time.deltaTime;
        else
            timer = 0;
    }
}
