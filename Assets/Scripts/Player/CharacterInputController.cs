using UnityEngine;
using UnityEngine.Events;

public class CharacterInputController : SingletonBase<CharacterInputController>
{    
    [SerializeField] private float heartTimeUsage = 2f;
    [SerializeField] private float maxDistanseHitCamera = 1f;
    [SerializeField] private float timeSprint = 2f;
    public float TimeSprint => timeSprint;
    private Character character; 
    public Character Character => character;
    
    private Vector3 playerMoveDirection;
    private float radiusCharacter;
    private float heightCharacter;
    private float timeHeart;
    public bool isMove = true;

    private bool heartEnabled;
    public bool HeartEnabled => heartEnabled;

    [HideInInspector] public bool pickUpHeart;
    
    [HideInInspector] public UnityEvent heartOn;
    [HideInInspector] public UnityEvent heartOff;
    [HideInInspector] public UnityEvent draculaAnim;

    private float sprintTimer;
    public float SprintTimer => sprintTimer;
    private bool isSprinting;
    public bool IsSprinting => isSprinting;
    
    public bool IsLook;
    private void Awake()
    {
        Init();
    }

    public void Start()
    {
        heartEnabled = false;
        
        character = GetComponent<Character>();
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
        
        if (Input.GetKey(KeyCode.LeftShift) && isSprinting && Character.isMove)
        {
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= timeSprint)  isSprinting = false;
            
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
        
        if (heartEnabled == false)
        {
            timeHeart -= Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (timeHeart <= 0)
            {
                heartEnabled = true;
                isMove = false;
                heartOn.Invoke();
                timeHeart = heartTimeUsage;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.F))
        {
            heartEnabled = false;
            isMove = true;
            heartOff.Invoke();
        }
    }

    public void ChangeSpeedTime(int value)
    {
        timeSprint += value;
    }
    #region RayLogick

    
private bool IsGrounded()
    {
        RaycastHit hitLegs;
        var vectorDown = character.transform.TransformDirection(Vector3.down);
        var maxDistance = heightCharacter / 2 + 0.1f;

        if (Physics.Raycast(character.transform.position - new Vector3(radiusCharacter, 0, 0), vectorDown, out hitLegs, maxDistance))
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
        RaycastHit hitLegs;
        var maxRadius = radiusCharacter + 0.1f;
        
        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.forward), out hitLegs, maxRadius))
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
        RaycastHit hitCamera;
 
        if (Physics.Raycast(character.Camera.transform.position, character.Camera.transform.forward, out hitCamera, maxDistanseHitCamera ,LayerMask.NameToLayer("Player")))
        {
            //Debug.DrawRay(character.Camera.transform.position, character.Camera.transform.forward * hitCamera.distance, Color.yellow, 0.01f);
            Debug.DrawLine(character.Camera.transform.position, hitCamera.transform.position, Color.yellow);
            
            if (hitCamera.collider.transform.parent?.GetComponent<InteractiveObject>())
            {
                var hit = hitCamera.collider.transform.parent.GetComponent<InteractiveObject>();

                hit.ShowText();

                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hit.Use();
                }
                IsLook = true;
                return;
            }
            
        }
        IsLook = false;
    }
    #endregion
    

    

}