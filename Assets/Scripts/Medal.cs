using UnityEngine;

public class Medal : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private int countPiece = 1;
    
    [SerializeField] private GameObject impactEffect;
    
    public void PickUp()
    {
        Destroy(view);
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
    }
}
