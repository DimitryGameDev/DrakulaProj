using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlateTrap : MonoBehaviour
{
    [SerializeField] private AudioClip floorSound;
    [SerializeField] private Collider trapPlateCollider;
   // [SerializeField] Collider normalPlate;
    
    [SerializeField] MeshRenderer trapPlateMesh;
    [SerializeField] InteractiveObject interactiveObject;
    
    private AudioSource audioSource;
    // Start is called before the first frame update
    
    private NoiseLevel noiseLevel;
    void OnTriggerEnter(Collider other)
    {
        if (trapPlateCollider != null)
        {
            if (other.transform.parent.CompareTag("Player"))
            {
                noiseLevel.IncreaseLevel();
                audioSource.PlayOneShot(floorSound);
                Debug.Log(noiseLevel.CurrentLevel);
            }
        }
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
   
}
