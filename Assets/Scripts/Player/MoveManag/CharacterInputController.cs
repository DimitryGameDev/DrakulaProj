using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterInputController : SingletonBase<CharacterInputController>
{    
    [SerializeField] private float maxDistanceHitCamera = 2f;
    [SerializeField] private float stamina = 2f;
    [SerializeField] private float heartScale = 0.5f;
    [SerializeField] private float runScale = 2f;
    [SerializeField] private float heartCooldown = 2f;
    [SerializeField] private float staminaCooldown = 2f;
    [SerializeField] private AudioClip sprintEndClip;

    private Character character; 
    private OnePersonCamera onePersonCamera;
    
    private AudioSource audioSource;
    private Vector3 playerMoveDirection;
    private float radiusCharacter;
    private float heightCharacter;
    private float timeHeart;
    private float staminaTimer;
    public float StaminaTimer => staminaTimer;
    public float Stamina => stamina;
    
    [HideInInspector] public UnityEvent heartOn;
    [HideInInspector] public UnityEvent heartOff;
    [HideInInspector] public UnityEvent rifleShoot;
    [HideInInspector] public UnityEvent draculaAnim;
    
    public bool isRiflePickup;
    public bool isStamina;
    private bool isRun;
    public bool isMove = true;
    public bool HeartEnabled { get; private set; }
    
    private void Awake()
    {
        Init();
    }

    public void Start()
    {
        HeartEnabled = false;
        onePersonCamera = OnePersonCamera.Instance;
        character = GetComponent<Character>();
        audioSource = GetComponent<AudioSource>();
        radiusCharacter = character.GetComponentInChildren<CapsuleCollider>().radius;
        heightCharacter = character.GetComponentInChildren<CapsuleCollider>().height;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        staminaTimer = stamina;
    }

    private void FixedUpdate()
    {
        CharacterMove();
        CharacterRotate();
    }

    private void Update()
    {
        StaminaUpdate();
        CameraUpdate();
        if (!HeartEnabled)MainRay();
        HeartState();
        RifleState();
    }


    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private void CharacterMove()
    {
        var dirZ = Input.GetAxis(Vertical);
        var dirX = Input.GetAxis(Horizontal);

        if (!isMove)
        {
            dirZ = 0;
            dirX = 0;
        }
        
        var ground = IsGrounded();
        
        playerMoveDirection = new Vector3(dirX, 0, dirZ);
            
        if (IsWall() && !ground) return;
        
        if (!ground)
        {
            character.Move(playerMoveDirection, MoveType.Air);
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && isStamina && character.isMove)
        {
            isRun = true;
            staminaTimer -= Time.deltaTime * runScale;
            character.Move(playerMoveDirection, MoveType.Run);
            return;
        }
        else
        {
            isRun = false;
        }
        character.Move(playerMoveDirection, MoveType.Walk);
    }

    private void StaminaUpdate()
    {
        if (!HeartEnabled && !isRun)
        {
            if (staminaTimer <= stamina) staminaTimer += Time.deltaTime/staminaCooldown;
            if (staminaTimer >= stamina) isStamina = true;
        }
        
        if (staminaTimer <= 0)
        {
            //staminaTimer = stamina;
            isStamina = false;
            audioSource.PlayOneShot(sprintEndClip,staminaCooldown);
            NoiseLevel.Instance.IncreaseLevel();
        }
    }

    private void CharacterRotate()
    {
        if (onePersonCamera.IsLocked || onePersonCamera.enabled) return;
        var rotation = new Quaternion(0, onePersonCamera.transform.rotation.y,0, onePersonCamera.transform.rotation.w);
        character.CharacterRotate(rotation);
    }
    
    private const string XAxis = "Mouse X";
    private const string YAxis = "Mouse Y";
    private void CameraUpdate()
    {
        var dirY = Input.GetAxis(YAxis);
        var dirX = Input.GetAxis(XAxis);
        
        onePersonCamera.Move(dirX, dirY);
    }
    
    private void HeartState()
    {
        if (HeartEnabled == false)
        {
            timeHeart -= Time.deltaTime;
        }
        else if (isStamina)
        {
            staminaTimer -= Time.deltaTime * heartScale;
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && isStamina)
        {
            if (timeHeart <= 0)
            {
                HeartEnabled = true;
                isMove = false;
                heartOn.Invoke();
                timeHeart = heartCooldown;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse1) && HeartEnabled || isStamina == false)
        {
            HeartEnabled = false;
            isMove = true;
            heartOff.Invoke();
        }
    }

    private void RifleState()
    {
        if (!isRiflePickup) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rifleShoot?.Invoke();
        }
    }

    public void ChangeSpeedTime(float value)
    {
        stamina += value;
    }
    
    public void SetSpeedTime(float value)
    {
        stamina = value;
    }
    
    #region RayLogick

    
private bool IsGrounded()
    {
        var vectorDown = character.transform.TransformDirection(Vector3.down);
        var maxDistance = heightCharacter / 2 + 0.1f;

        if (Physics.Raycast(character.transform.position - new Vector3(radiusCharacter, 0, 0), vectorDown, out var hitLegs, maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(radiusCharacter, 0, 0), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(-radiusCharacter, 0, 0), vectorDown, out hitLegs,maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(-radiusCharacter, 0, 0), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(0, 0, radiusCharacter), vectorDown, out hitLegs,maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(0, 0, radiusCharacter), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(0, 0, -radiusCharacter), vectorDown, out hitLegs, maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(0, 0, -radiusCharacter), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        return false;
    }

    private bool IsWall()
    {
        var maxRadius = radiusCharacter + 0.1f;
        
        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.forward), out var hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position , character.transform.TransformDirection(Vector3.forward) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.back), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position , character.transform.TransformDirection(Vector3.back) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.right), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.right) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.left), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.left) * hitLegs.distance, Color.red);
            return true;
        }

        return false;
        
    }
    private void MainRay()
    {
        if (Physics.Raycast(onePersonCamera.transform.position, onePersonCamera.transform.forward, out var hitCamera,
                maxDistanceHitCamera, LayerMask.NameToLayer("Player")))
        {
            Debug.DrawLine(onePersonCamera.transform.position, hitCamera.transform.position, Color.yellow);

            if (hitCamera.collider.transform.parent?.GetComponent<InteractiveObject>())
            {
                var hit = hitCamera.collider.transform.parent?.GetComponent<InteractiveObject>();

                if (!hit.IsAfterText)
                    hit.ShowText();

                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hit.Use();
                }
                
            }
        }

    }
    #endregion
    

    

}