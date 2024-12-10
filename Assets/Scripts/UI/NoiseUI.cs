using UnityEngine;
using UnityEngine.UI;

public class NoiseUI : MonoBehaviour
{
    [SerializeField]private Image fill;
    [SerializeField]private float speedFill;
    private int current;
    private int maxNoise;
    private void Start()
    {
        NoiseLevel.Instance.OnChange += UpdateUI;
        maxNoise = NoiseLevel.Instance.MaxLevel;
    }

    private void Update()
    {
        var target = (float) current / maxNoise;
        fill.fillAmount = Mathf.MoveTowards(fill.fillAmount, target, Time.deltaTime/speedFill);
        if (Mathf.Approximately(fill.fillAmount, target))enabled = false;
    }

    private void UpdateUI(int value)
    {
        current = value;
        enabled = true;
    }
}