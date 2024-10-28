using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{
   public UnityEvent onTrigger;

   private void OnTriggerEnter(Collider collision)
   {
      if (collision.gameObject.transform.root.GetComponent<Character>())
      {
         onTrigger.Invoke();
         gameObject.SetActive(false);
      }
   }
}
