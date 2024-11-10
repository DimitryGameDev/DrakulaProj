using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlateTrap : VisibleObject
{
    [SerializeField] private AudioClip floorSound;
    [SerializeField] private Collider trapPlateCollider;
   // [SerializeField] Collider normalPlate;
    [SerializeField] MeshRenderer trapPlateMesh;
    
    private AudioSource audioSource;
    private NoiseLevel noiseLevel;
    
    void OnTriggerEnter(Collider other)
    {
        if (trapPlateCollider != null)
        {
            if (other.transform.parent.CompareTag("Player"))
            {
                noiseLevel.IncreaseLevel();
                audioSource.PlayOneShot(floorSound);
                //Debug.Log(noiseLevel.CurrentLevel);
            }
        }
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        noiseLevel = NoiseLevel.Instance;
        trapPlateMesh.enabled = false;
    } 
    
    public override void InCamera()
    {
        trapPlateMesh.enabled = true;
    }

    public override void OutCamera()
    {
        trapPlateMesh.enabled = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
   
}
