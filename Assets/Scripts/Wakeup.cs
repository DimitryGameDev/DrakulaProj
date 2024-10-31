using UnityEngine;
using UnityEngine.UI;

public class Wakeup : MonoBehaviour
{
   private Image image;
   private void Awake()
   {
      image = GetComponent<Image>();
      image.color = Color.black;
   }

   private void Update()
   {
      image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 0, Time.deltaTime / 2));
      if (image.color.a == 0) enabled = false;

   }
}
