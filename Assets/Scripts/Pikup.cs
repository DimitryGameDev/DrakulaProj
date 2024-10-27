using UnityEngine;

public class Pikup : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        Bag bag = other.GetComponent<Bag>();

        if(bag != null)
        {
            Destroy(gameObject);
        }
    }
}
