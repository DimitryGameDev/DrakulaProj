using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(InteractiveObject))]
public class Lockpick : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private OnePersonCamera onePersonCamera;
    [SerializeField] private Transform cameraTarget;
    
    [Header("Base")]
    [SerializeField] private float timeToSuccess;
    [SerializeField] private NoiseLevel noiseLevel;
    
    [Header("UI")]
    [SerializeField] private Texture2D mouseTexture;
    [SerializeField] private GameObject infoText;
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform point;
    
    [Header("SFX")]
    [SerializeField] private AudioClip successOpenSFX;
    [SerializeField] private AudioClip failOpenSFX;

    private AudioSource audioSource;
    
    private Character character;
    private Bag bag;
    private InteractiveObject interactiveObject;
    
    private float timer;
    private float textTimer;
    
    private Vector2 randomImagePosition;
    private Vector2 screenMousePosition;
    
    private bool isOpening;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        infoText.SetActive(false);
        character = Character.Instance.GetComponent<Character>();
        bag = Character.Instance.GetComponent<Bag>();
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(StartUnlock);
        ResetPoint();
    }

    private void OnDestroy()
    {
        interactiveObject.onUse.RemoveListener(StartUnlock);
    }

    private void Update()
    { 
       PointMove();
       InfoText();
    }

    private void StartUnlock()
    {
        if (bag.GetKeyAmount() <= 0)
        {
            textTimer = timeToSuccess;
            return;
        }

        panel.SetActive(true);
        
        onePersonCamera.SetTarget(cameraTarget,TypeMoveCamera.WithRotation,true);
        CharacterInputController.Instance.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
        if (Dracula.Instance)
        {
            Dracula.Instance.DraculaDisable();
        }
        timer = timeToSuccess;
        isOpening = true;
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
            if (timer <= 0.1f)
            {
                bag.DrawKey(1);
                ResetPoint();
                Resume();
                
                SuccessUnlock();
            }
        }
        else
        {
            noiseLevel.IncreaseLevel();
            ResetPoint();
            Resume();
            
            audioSource.PlayOneShot(failOpenSFX);
        }
    }

    private void Resume()
    {
        if (Dracula.Instance)
        {
            Dracula.Instance.enabled = true; //включает дракулу
        }
        CharacterInputController.Instance.enabled = true;
        onePersonCamera.SetTarget(character.CameraPos,TypeMoveCamera.OnlyMove,false);
    }

    private void SuccessUnlock()
    {
        //OpenDoorlogic

        interactiveObject.onUse.RemoveListener(StartUnlock);
        
        audioSource.PlayOneShot(successOpenSFX);
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

    private void InfoText()
    {
        if(textTimer>=0)
            textTimer -= Time.deltaTime;

        if (textTimer > 0)
            infoText.SetActive(true);
        else
            infoText.SetActive(false);
    }
}