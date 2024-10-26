using UnityEngine;
using UnityEngine.UI;

public class Lockpick : MonoBehaviour
{
    [SerializeField] private NoiseLevel noiseLevel;
    [SerializeField] private Texture2D mouseTexture;
    [SerializeField] private RectTransform rectTransform;
    
    private Camera mainCamera;
    private Vector2 randomImagePosition;
    private void Start()
    {
        mainCamera = Camera.main;
        
        randomImagePosition = new Vector2 (Random.Range(0, Screen.width - rectTransform.rect.size.x),
            Random.Range(0, Screen.height - rectTransform.rect.size.y));
    }

    private void Update()
    {
        Vector3 screenMousePosition =Input.mousePosition;
      //  Debug.Log(screenMousePosition);
      Debug.Log(randomImagePosition);
      rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, randomImagePosition, 100 * Time.deltaTime );
      if (rectTransform.anchoredPosition == randomImagePosition )
      {
          randomImagePosition = new Vector2(Random.Range(0, Screen.width - rectTransform.rect.size.x),
              Random.Range(0, Screen.height - rectTransform.rect.size.y));
      }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            noiseLevel.IncreaseLevel();
            
            Unlock();
        }
    }

    private void Unlock()
    {
        Cursor.SetCursor(mouseTexture,Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
       
    }
}
