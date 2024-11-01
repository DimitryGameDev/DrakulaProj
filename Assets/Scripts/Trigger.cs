using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{
   [Header("Включить если EnterTrigger")]
   [SerializeField] private bool isEnable;
   public bool IsEnable => isEnable;
   
   [Header("Либо одно либо другое")]
   [SerializeField] private PatrolPoint spawnPoint;
   [Space]
   [SerializeField] private PatrolPoint[] spawnPoints;
   
   [HideInInspector]public UnityEvent onTrigger;
   
   private void Awake()
   {
      TriggerController.OnTrigger += SwitchActive;
      if (spawnPoint)
      {
         onTrigger.AddListener(SpawnDraculaSpawnPoint);
         
      }
      
      if (spawnPoints.Length > 0)
      {
         onTrigger.AddListener(SpawnDraculaSpawnPoints);
      }
   }

   private void SpawnDraculaSpawnPoint()
   {
      Dracula.Instance.DraculaSpawn(spawnPoint);
   }
   private void SpawnDraculaSpawnPoints()
   {
      Dracula.Instance.DraculaSpawns(spawnPoints);
   }
   private void Start()
   {
      if (!isEnable)
      {
         onTrigger.AddListener(Dracula.Instance.DraculaDisable);
         gameObject.SetActive(false);
      }
   }

   private void OnTriggerEnter(Collider collision)
   {
      if (collision.transform.parent.GetComponent<Character>())
      {
         onTrigger?.Invoke();
      }
   }

   private void SwitchActive()
   { 
      isEnable = !isEnable;
      if (!isEnable) gameObject.SetActive(false);
      if (isEnable) gameObject.SetActive(true);
   }
}
