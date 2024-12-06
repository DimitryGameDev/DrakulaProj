using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LockPick : InteractiveObject
{
    [Header("Door Settings")]
    [SerializeField] private bool draculaDoor;
    
    [Header("Player")]
    [SerializeField] private Transform cameraTargetFront;
    [SerializeField] private Transform cameraTargetBack;
    
    [Header("Base")] 
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private float timeToSuccess;
    [SerializeField] private float timeToPrepare;
    [SerializeField] private Animator animator;

    [Header("UI")] 
    [SerializeField] private Texture2D mouseTexture; 
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform point;
    
    [Header("SFX")]
    [SerializeField] private AudioClip successOpenSfx;
    [SerializeField] private AudioClip failOpenSfx;
    
    private OnePersonCamera onePersonCamera;
    private Character character;
    private Bag bag;
    private Dracula dracula;
    
    private float timer2;
    private float timer3 = 1f;
    
    private Vector2 randomImagePosition;
    private Vector2 screenMousePosition;
    
    private bool isOpening;
    
    private Transform currentCameraTarget;
    
    protected override void Start()
    {
        base.Start();
        character = Character.Instance.GetComponent<Character>();
        bag = Character.Instance.GetComponent<Bag>();
        dracula = Dracula.Instance;
        onePersonCamera = OnePersonCamera.Instance;
        ResetPoint();// тут надо убрать вызов включения character input.
    }

    protected override void Update()
    { 
       base.Update();
       PointMove();
    }

    public override void Use()
    {
        base.Use();
        
        if (Vector3.Distance(cameraTargetBack.position , character.transform.position)>
            Vector3.Distance(cameraTargetFront.position , character.transform.position))
            currentCameraTarget = cameraTargetFront;
        else
            currentCameraTarget = cameraTargetBack;
        
        StartUnlock();
    }

    private void StartUnlock()
    {
        if (bag.GetKeyAmount() <= 0)
        {
            ShowAfterText();
        }

        if (draculaDoor && bag.GetMedalPeaceAmount() < 3)
        {
            ShowAfterText();
        }
        
        MiniGame();
    }

    private void PointMove()
    {
        if(!isOpening) return;
        timer3 -= Time.deltaTime;
       
        if (timer3 <= 0)
        {
            if (timer2 >= 0)
                timer2 -= Time.deltaTime;
            else
                timer2 = 0;
        
            screenMousePosition = Input.mousePosition;
        
            if (point.anchoredPosition == randomImagePosition)
            {
                GenerateRandomPosition();
            }
        
            point.anchoredPosition = Vector2.MoveTowards(point.anchoredPosition, randomImagePosition, 100 * Time.deltaTime);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(point, screenMousePosition, null, out var localPoint);
        
            if (localPoint.x >= point.rect.xMin && localPoint.x <= point.rect.xMax &&
                localPoint.y >= point.rect.yMin && localPoint.y <= point.rect.yMax)
            {
                if (timer2 <= 0.1f)
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
            
                AudioSource.PlayOneShot(failOpenSfx);
            }
        }
    }

    private void MiniGame()
    {
        if (!draculaDoor && bag.GetKeyAmount() > 0)
        {
            panel.SetActive(true);

            onePersonCamera.SetTarget(currentCameraTarget, TypeMoveCamera.WithRotation, true);
            CharacterInputController.Instance.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;

            if (dracula)
            {
                dracula.DraculaDisable();
            }
            
            timer2 = timeToSuccess;
            timer3 = timeToPrepare;
            isOpening = true;
        }

        if (draculaDoor && bag.GetMedalPeaceAmount() >= 3)
        {
            isOpening = true;
        }
    }

    private void SuccessUnlock()
    {
        if (draculaDoor)
        {
            bag.RemoveMedalPiece();
        }

        OpenDoor();
    }

    public void OpenDoor()
    {
        if (!wosActive)  AudioSource.PlayOneShot(successOpenSfx);
        animator.SetBool("Open", true);
        Destroy(triggerCollider);
        wosActive = true;
    }

    protected override void ObjectWosActive()
    {
        OpenDoor();
    }

    private void ResetPoint()
    {
        if (dracula)
        {
            dracula.DraculaEnable();
        }
        
        CharacterInputController.Instance.enabled = true;
        onePersonCamera.SetTarget(character.CameraPos,TypeMoveCamera.OnlyMove,false);

        timer2 = 0;
        isOpening = false;
        
        panel.SetActive(false);
        
        Cursor.visible = false;
        Cursor.SetCursor(null,Vector2.zero, CursorMode.Auto);
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