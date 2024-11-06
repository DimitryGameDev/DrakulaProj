using UnityEngine;
using Random = UnityEngine.Random;

public class Lockpick : InteractiveObject
{
    [SerializeField] private bool draculaDoor;
    
    [Header("Player")]
    [SerializeField] private Transform cameraTarget;

    [Header("Base")] 
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private float timeToSuccess;
    [SerializeField] private Animator animator;

    [Header("UI")] 
    [SerializeField] private Texture2D mouseTexture; 
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform point;
    
    [Header("SFX")]
    [SerializeField] private AudioClip successOpenSFX;
    [SerializeField] private AudioClip failOpenSFX;

    private AudioSource audioSource;
    private OnePersonCamera onePersonCamera;
    private Character character;
    private Bag bag;
    private Dracula dracula;
    
    private float timer;
    
    private Vector2 randomImagePosition;
    private Vector2 screenMousePosition;
    
    private bool isOpening;
    
    private void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        character = Character.Instance.GetComponent<Character>();
        bag = Character.Instance.GetComponent<Bag>();
        dracula = Dracula.Instance;
        onePersonCamera = OnePersonCamera.Instance;
        ResetPoint();
    }

    private void Update()
    { 
       base.Update();
       PointMove();
    }

    public override void Use()
    {
        base.Use();
        StartUnlock();

        if (bag.GetKeyAmount() <= 0)
        {
            ShowAfterText();
        }

        if (draculaDoor && bag.GetMedalAmount() <= 0)
        {
            ShowAfterText();
        }
    }

    private void StartUnlock()
    {
        if (draculaDoor && bag.GetMedalAmount() > 0)
            MiniGame();

        if (!draculaDoor)
            MiniGame();
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
                
                SuccessUnlock();
            }
        }
        else
        {
            NoiseLevel.Instance.IncreaseLevel();
            ResetPoint();
            
            audioSource.PlayOneShot(failOpenSFX);
        }
    }

    private void MiniGame()
    {
        if (bag.GetKeyAmount() > 0)
        {
            panel.SetActive(true);

            onePersonCamera.SetTarget(cameraTarget, TypeMoveCamera.WithRotation, true);
            CharacterInputController.Instance.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;

            if (dracula)
            {
                dracula.DraculaDisable();
            }
            
            timer = timeToSuccess;
            isOpening = true;
        }
    }

    private void SuccessUnlock()
    {
        if (draculaDoor)
        {
            bag.RemoveMedal();
        }

        audioSource.PlayOneShot(successOpenSFX);
        animator.SetBool("Open", true);
        
        Destroy(triggerCollider);
        Destroy(this);
    }
    
    private void ResetPoint()
    {
        if (dracula)
        {
            dracula.DraculaEnable();
        }
        
        CharacterInputController.Instance.enabled = true;
        onePersonCamera.SetTarget(character.CameraPos,TypeMoveCamera.OnlyMove,false);

        timer = 0;
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