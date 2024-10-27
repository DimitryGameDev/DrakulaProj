using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesTrap : MonoBehaviour
{
    [SerializeField] Collider trapPlate;
   // [SerializeField] Collider normalPlate;
    [SerializeField] private NoiseLevel noiseLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
            if (trapPlate != null)
            {
                noiseLevel.IncreaseLevel();
                Debug.Log(noiseLevel.CurrentLevel);
                
            }
        
       
    }

   
}
