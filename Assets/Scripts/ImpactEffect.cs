
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
