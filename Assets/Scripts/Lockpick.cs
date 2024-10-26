using UnityEngine;

public class Lockpick : MonoBehaviour
{
    [SerializeField] private float timeToSuccess;
    
    [SerializeField] private NoiseLevel noiseLevel;
    [SerializeField] private Texture2D mouseTexture;

    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform point;

    private float timer;
    
    private Vector2 randomImagePosition;
    private Vector2 screenMousePosition;
    
    private bool isOpening;
    private bool isSuccess;

    private void Start()
    {
        ResetPoint();
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartUnlock();
        }

       PointMove();
    }

    private void StartUnlock()
    {
        panel.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(mouseTexture,Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;

        isOpening = true;

        timer = timeToSuccess;
    }

    private void PointMove()
    {
        if(!isOpening) return;

        if (timer >= 0)
            timer -= Time.deltaTime;
        else
            timer = 0;
        
        screenMousePosition = Input.mousePosition;
        
        if (point.anchoredPosition == randomImagePosition)
        {
            GenerateRandomPosition();
        }
        
        point.anchoredPosition = Vector2.MoveTowards(point.anchoredPosition, randomImagePosition, 100 * Time.deltaTime);
        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(point, screenMousePosition, null, out localPoint);
        
        if (localPoint.x >= point.rect.xMin && localPoint.x <= point.rect.xMax &&
            localPoint.y >= point.rect.yMin && localPoint.y <= point.rect.yMax)
        {
            //Debug.Log("Курсор внутри point");
            if (timer <= 0.1f)
            {
                isSuccess = true;
                ResetPoint();
            }
        }
        else
        {
            //Debug.Log("Курсор вне point");
            //isSuccess = false;
            //timer = timeToSuccess;
            
            //if(timer == timeToSuccess)
            noiseLevel.IncreaseLevel();
            ResetPoint();
        }
    }

    private void ResetPoint()
    {
        isOpening = false;
        
        panel.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        point.anchoredPosition = Vector2.zero;
    }

    private void GenerateRandomPosition()
    {
        randomImagePosition = new Vector2(
            Random.Range(background.rect.x + point.rect.size.x/2, background.rect.xMax - point.rect.xMax), 
            Random.Range(background.rect.y + point.rect.size.y/2, background.rect.yMax - point.rect.yMax)
        );
    }
}