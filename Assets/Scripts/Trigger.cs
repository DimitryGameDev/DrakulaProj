using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{
   [Header("Включить если EnterTrigger")]
   [SerializeField] private bool isEnable;
   public bool IsEnable => isEnable;
   [Header("Добавить события если EnterTrigger")]
   public UnityEvent onTrigger;

   private void Awake()
   {
      TriggerController.OnTrigger += SwitchActive;
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
      if (collision.gameObject.transform.parent.GetComponent<Character>())
      {
         onTrigger?.Invoke();
         gameObject.SetActive(false);
      }
   }

   private void SwitchActive()
   {
      isEnable = !isEnable;
      if (isEnable) gameObject.SetActive(true);
      if (!isEnable) gameObject.SetActive(false);
   }
}
