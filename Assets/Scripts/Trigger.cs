using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{
   [Header("Включить если EnterTrigger")]
   [SerializeField] private bool isEnable;
   public bool IsEnable => isEnable;

   [Header("Выбрать что то одно")] 
   [SerializeField] private bool randomSpawn;
   [SerializeField] private DraculaPoint spawnPoint;
   [Space]
   [SerializeField] private DraculaPoint[] spawnPoints;
   
   [HideInInspector]public UnityEvent onTrigger;
   
   private Dracula dracula;
   private void SpawnDraculaSpawnPoint()
   {
      dracula.SetPoint(spawnPoint);
   }
   private void SpawnDraculaSpawnPoints()
   {
      dracula.SetPoints(spawnPoints);
   }
   private void SpawnDraculaRandomPint()
   {
      dracula.RandomPoint(); 
   }
   private void Start()
   {
      dracula = Dracula.Instance;
      
      if (randomSpawn)
      {
         onTrigger.AddListener(SpawnDraculaRandomPint);
      }
      
      if (spawnPoint)
      {
         onTrigger.AddListener(SpawnDraculaSpawnPoint);
      }
      
      if (spawnPoints.Length > 0)
      {
         onTrigger.AddListener(SpawnDraculaSpawnPoints);
      }
      
      if (!isEnable)
      {
         onTrigger.AddListener(dracula.DraculaDespawn);
         gameObject.SetActive(false);
      }
   }

   private void OnTriggerEnter(Collider collision)
   {
      if (collision.transform.parent.GetComponent<Character>())
      {
         if (dracula.enabled)return;
         onTrigger?.Invoke();
      }
   }

   public void SwitchActive()
   { 
      isEnable = !isEnable;
      if (!isEnable) gameObject.SetActive(false);
      if (isEnable) gameObject.SetActive(true);
   }
}
