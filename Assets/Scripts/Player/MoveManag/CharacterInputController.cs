using UnityEngine;
using UnityEngine.Events;

public class CharacterInputController : SingletonBase<CharacterInputController>
{    
    [SerializeField] private float heartTimeUsage = 2f;
    [SerializeField] private float maxDistanceHitCamera = 2f;
    [SerializeField] private float timeSprint = 2f;
    [SerializeField] private AudioClip sprintEndClip;

    private Character character; 
    
    private AudioSource audioSource;
    private Vector3 playerMoveDirection;
    private float radiusCharacter;
    private float heightCharacter;
    private float timeHeart;
    public bool isMove = true;

    public bool HeartEnabled { get; private set; }

    [HideInInspector] public bool pickUpHeart;
    
    [HideInInspector] public UnityEvent heartOn;
    [HideInInspector] public UnityEvent heartOff;
    [HideInInspector] public UnityEvent draculaAnim;

    private float sprintTimer;
    public float SprintTimer => sprintTimer;
    
    public float TimeSprint => timeSprint;
    
    public bool isSprinting;
    
    public bool isLook;
    
    private void Awake()
    {
        Init();
    }

    public void Start()
    {
        HeartEnabled = false;
        
        character = GetComponent<Character>();
        audioSource = GetComponent<AudioSource>();
        radiusCharacter = character.GetComponentInChildren<CapsuleCollider>().radius;
        heightCharacter = character.GetComponentInChildren<CapsuleCollider>().height;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        AdminMove();
    }

    private void Update()
    { 
        MainRay();
        AdminCameraMove();
        HeartState();
    }

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private void AdminMove()
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
        
        if (Input.GetKey(KeyCode.LeftShift) && isSprinting && character.isMove)
        {
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= timeSprint)
            {
                isSprinting = false;
                audioSource.PlayOneShot(sprintEndClip);
                NoiseLevel.Instance.IncreaseLevel();
            }
            
            character.Move(playerMoveDirection, MoveType.Run);
            return;
        }
        else
        {
            if (sprintTimer >= 0)sprintTimer -= Time.deltaTime/2;
            if (sprintTimer <= 0)isSprinting = true;
        }

        character.Move(playerMoveDirection, MoveType.Walk);
    }

    private const string XAxis = "Mouse X";
    private const string YAxis = "Mouse Y";
    private void AdminCameraMove()
    {
        var dirY = Input.GetAxis(YAxis);
        var dirX = Input.GetAxis(XAxis);
        
        character.CameraMove(dirX, dirY);
    }


    private void HeartState()
    {
        if(!pickUpHeart) return;
        
        if (HeartEnabled == false)
        {
            timeHeart -= Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (timeHeart <= 0)
            {
                HeartEnabled = true;
                isMove = false;
                heartOn.Invoke();
                timeHeart = heartTimeUsage;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.F))
        {
            HeartEnabled = false;
            isMove = true;
            heartOff.Invoke();
        }
    }

    public void ChangeSpeedTime(float value)
    {
        timeSprint += value;
    }
    
    public void SetSpeedTime(float value)
    {
        timeSprint = value;
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
        if (Physics.Raycast(character.Camera.transform.position, character.Camera.transform.forward, out var hitCamera,
                maxDistanceHitCamera, LayerMask.NameToLayer("Player")))
        {
            Debug.DrawLine(character.Camera.transform.position, hitCamera.transform.position, Color.yellow);

            if (hitCamera.collider.transform.parent?.GetComponent<InteractiveObject>())
            {
                var hit = hitCamera.collider.transform.parent?.GetComponent<InteractiveObject>();

                if (!hit.IsAfterText)
                    hit.ShowText();

                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hit.Use();
                }

                isLook = true;
                return;
            }

        }

        isLook = false;
    }
    #endregion
    

    

}