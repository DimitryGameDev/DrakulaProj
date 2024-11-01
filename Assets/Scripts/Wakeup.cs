using UnityEngine;
using UnityEngine.UI;

public class Wakeup : MonoBehaviour
{
   private Image image;
   
   public bool isWakeup;
   
   private void Awake()
   {
      image = GetComponent<Image>();
      image.color = Color.black;
   }

   private void Update()
   {
      if (isWakeup)
      {
         image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1, Time.deltaTime / 2));
         if (Mathf.Approximately(image.color.a, 1)) isWakeup = false;
      }
      else
      {
         image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 0, Time.deltaTime / 2));
      }
      
      if (image.color.a == 0) enabled = false;
   }

   public void WakeUp()
   {
      isWakeup = true;
      enabled = true;
   }
}
