using System.Collections.Generic;
using UnityEngine;

public class NoiseUI : MonoBehaviour
{
    
    [SerializeField]private FadeUi noiseImagePrefab;
    private List<FadeUi> noiseObjects;
    
    private void Start()
    {
        noiseObjects = new List<FadeUi>();
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
        foreach (var t in noiseObjects)
        {
            if (value > 0)
            {
                value--;
                t.Show();
                continue;
            }
            t.Hide();
        }
    }
}