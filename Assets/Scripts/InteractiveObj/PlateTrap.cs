using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlateTrap : InteractiveObject
{
    [SerializeField] private AudioClip floorSound;
    [SerializeField] private Collider trapPlateCollider;
    [SerializeField] MeshRenderer trapPlateMesh;
    private NoiseLevel noiseLevel;
    
    void OnTriggerEnter(Collider other)
    {
        if (trapPlateCollider != null)
        {
            if (other.transform.parent.CompareTag("Player"))
            {
                noiseLevel.IncreaseLevel();
                AudioSource.PlayOneShot(floorSound);
                Debug.Log(noiseLevel.CurrentLevel);
            }
        }
    }

    public override void ShowText(){}
    
    protected override void Start()
    {
        base.Start();;
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
