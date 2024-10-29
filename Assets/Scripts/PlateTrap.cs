using UnityEngine;

public class PlateTrap : MonoBehaviour
{
    [SerializeField] private Collider trapPlateCollider;
   // [SerializeField] Collider normalPlate;
    
    [SerializeField] MeshRenderer trapPlateMesh;
    [SerializeField] InteractiveObject interactiveObject;
    // Start is called before the first frame update

    
    private NoiseLevel noiseLevel;
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
        noiseLevel = NoiseLevel.Instance;
        trapPlateMesh.enabled = false;
        interactiveObject.onVision.AddListener(VisionOn);
        interactiveObject.onHide.AddListener(VisionOff);
    } 
    
    private void VisionOn()
    {
        trapPlateMesh.enabled = true;
    }

    private void VisionOff()
    {
        trapPlateMesh.enabled = false;
    }
   
}
