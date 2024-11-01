using System.Collections.Generic;
using UnityEngine;

public class NoiseUI : MonoBehaviour
{
    
    [SerializeField]private FadeUi noiseImagePrefab;
    private int lastValue = 0;
    private List<FadeUi> noiseObjects = new List<FadeUi>();
    
    private void Start()
    {
        NoiseLevel.Instance.OnChange += UpdateUI;
    }
    
    private void UpdateUI(int value)
    {
        /*
        if (lastValue > value)
        {
            var i = Instantiate(noiseImagePrefab);
            noiseObjects.Add(i);
        }
        
        else if (lastValue < value)
        {
            if (noiseObjects.Count > 0)
            {
                noiseObjects[0].Hide();
            }
        }
        lastValue = value;
        */
    }
}