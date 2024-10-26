using UnityEngine;

namespace Common
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float lifeTimer;
        public float LifeTimer => lifeTimer;

        private void Start()
        {
            Destroy(gameObject, lifeTimer);
        }
    }
}