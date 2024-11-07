using System.Collections.Generic;
using UnityEngine;

public class NoiseUI : MonoBehaviour
{
    
    [SerializeField]private FadeUi noiseImagePrefab;
    private List<FadeUi> noiseObjects = new List<FadeUi>();
    
    private void Start()
    {
        var y = transform.childCount;
        for (int i = 0; i < y; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        NoiseLevel.Instance.OnChange += UpdateUI;
        for (int i = 0; i < NoiseLevel.Instance.MaxLevel; i++)
        {
            var x = Instantiate(noiseImagePrefab, transform);
            noiseObjects.Add(x);
        }
    }
    
    private void UpdateUI(int value)
    {
        for (int i = 0; i < noiseObjects.Count; i++)
        {
            if (value > 0)
            {
                value--;
                noiseObjects[i].Show();
                continue;
            }
            noiseObjects[i].Hide();
        }
    }
}