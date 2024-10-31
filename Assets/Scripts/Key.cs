using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class Key : MonoBehaviour
{
    [SerializeField] private int keyCount;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;

    private InteractiveObject interactiveObject;
    
    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(PickUp);
    }
    
    private void PickUp()
    {
        Character.Instance.GetComponent<Bag>().AddKey(keyCount);

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }

}