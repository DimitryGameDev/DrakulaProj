using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrap : MonoBehaviour
{
    [SerializeField] private Collider trapPlateCollider;
   // [SerializeField] Collider normalPlate;
    [SerializeField] private NoiseLevel noiseLevel;
    [SerializeField] MeshRenderer trapPlateMesh;
    [SerializeField] InteractiveObject interactiveObject;
    // Start is called before the first frame update



    void OnTriggerEnter(Collider other)
    {

        if (trapPlateCollider != null)
        {
            if (other.CompareTag("Player"))
            {
                noiseLevel.IncreaseLevel();
                Debug.Log(noiseLevel.CurrentLevel);
            }
           

        }


    }
    
    void Start()
    {
        trapPlateMesh.enabled = false;
        interactiveObject.onVision.AddListener(VisionOn);
        interactiveObject.onHide.AddListener(VisionOff);
    } 
    private void VisionOn()
    {
        trapPlateMesh.enabled = true;
        //  interactiveObject.onVision.RemoveListener(Found);
        //  interactiveObject.onHide.RemoveListener(Found);
        Debug.Log(interactiveObject.name);
    }

    private void VisionOff()
    {
        trapPlateMesh.enabled = false;
    }
    

   
}
