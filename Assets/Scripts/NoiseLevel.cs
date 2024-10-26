using UnityEngine;
using UnityEngine.Events;

public class NoiseLevel : MonoBehaviour
{
    public event UnityAction OnIncreasedLevel;
    public event UnityAction OnDecreasedLevel;
    
    [SerializeField] private int maxLevel;
    [SerializeField] private float resetTime;

    private int currentLevel;
    public int CurrentLevel => currentLevel;
    
    private float timer;
    
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
            OnIncreasedLevel?.Invoke();
            timer = resetTime;
        }
    }

    private void DecreaseLevel()
    {
        if (currentLevel > 0 && timer <= 0)
        {
            currentLevel--;
            OnDecreasedLevel?.Invoke();
            timer = resetTime;
        }
    }

    private void UpdateTimer()
    {
        if(timer>0)
            timer -= Time.deltaTime;
        else
            timer = 0;
    }
}
