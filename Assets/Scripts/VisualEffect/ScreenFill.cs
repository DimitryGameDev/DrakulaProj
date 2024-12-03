using UnityEngine;
using UnityEngine.UI;

public class ScreenFill : MonoBehaviour
{
   private Image image;
   
   public bool isFill;
   private float timer;
   private readonly float waitTime = 1.2f;
   
   private void Start()
   {
      image = GetComponent<Image>();
      image.color = Color.black;
   }

   private void Update()
   {
      timer += Time.deltaTime;
      if (waitTime > timer) return;

      if (isFill)
      {
         image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1, Time.deltaTime / 2));
         if (Mathf.Approximately(image.color.a, 1)) isFill = false;
      }
      else
      {
         image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 0, Time.deltaTime / 2));
      }
      
      if (image.color.a == 0) enabled = false;
   }

   public void Fill()
   {
      isFill = true;
      enabled = true;
   }
}
