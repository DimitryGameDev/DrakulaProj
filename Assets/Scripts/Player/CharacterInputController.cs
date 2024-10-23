using UnityEngine;

/*
public enum State
{
    None,
    AdminPlace,
    Monitor,
}
*/
[RequireComponent (typeof(CapsuleCollider))]
public class CharacterInputController : MonoBehaviour
{
    //[SerializeField] private float maxDistanseHitCamera = 1f;
    
    private Character character; 
    public Character Character => character;
    
    private Vector3 playerMoveDirection;
    private float radiusAdmin;
    private float hightAdmin;   
    
    //public State stateMove;
    public void Start()
    {
        character = GetComponent<Character>();
        radiusAdmin = character.GetComponent<CapsuleCollider>().radius;
        hightAdmin = character.GetComponent<CapsuleCollider>().height;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        AdminMove();
    }

    private void Update()
    { 
        AdminCameraMove();
        
        /*
        if (stateMove == State.None)
        {
            
            MainRay();
        }

        if (stateMove == State.AdminPlace)
        {
            AdminInPlaceCameraMove();
            RayInPlace();            
        }

        if (stateMove == State.Monitor)
        {
            RayInPlace();
        }  
        */
    }

    const string Horizontal = "Horizontal";
    const string Vertical = "Vertical";

    private void AdminMove()
    {
        float dirZ = Input.GetAxis(Vertical);
        float dirX = Input.GetAxis(Horizontal);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            character.Jump();
        }
        
        playerMoveDirection = new Vector3(dirX, 0, dirZ);

        if (IsWall() && !IsGrounded()) return;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            character.Move(playerMoveDirection, IsGrounded(),MoveType.Run);
            return;
        }
        
        character.Move(playerMoveDirection, IsGrounded(),MoveType.Walk);
    }

    const string XAxis = "Mouse X";
    const string YAxis = "Mouse Y";
    private void AdminCameraMove()
    {
        float dirY = Input.GetAxis(YAxis);
        float dirX = Input.GetAxis(XAxis);
        
        character.CameraMove(dirX, dirY);
    }
    private bool IsGrounded()
    {
        RaycastHit hitLegs;

        Vector3 vectordown = character.transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(character.transform.position - new Vector3(radiusAdmin, 0, 0), vectordown, out hitLegs, hightAdmin/2 + 0.01f))
        {
            Debug.DrawRay(character.transform.position - new Vector3(radiusAdmin, 0, 0), vectordown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(-radiusAdmin, 0, 0), vectordown, out hitLegs, hightAdmin / 2 + 0.01f))
        {
            Debug.DrawRay(character.transform.position - new Vector3(-radiusAdmin, 0, 0), vectordown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(0, 0, radiusAdmin), vectordown, out hitLegs, hightAdmin / 2 + 0.01f))
        {
            Debug.DrawRay(character.transform.position - new Vector3(0, 0, radiusAdmin), vectordown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(0, 0, -radiusAdmin), vectordown, out hitLegs, hightAdmin / 2 + 0.01f))
        {
            Debug.DrawRay(character.transform.position - new Vector3(0, 0, -radiusAdmin), vectordown * hitLegs.distance, Color.red);
            return true;
        }

        return false;
    }
    private bool IsWall()
    {
        RaycastHit hitLegs;

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.forward), out hitLegs, radiusAdmin + 0.1f))
        {
            Debug.DrawRay(character.transform.position , character.transform.TransformDirection(Vector3.forward) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.back), out hitLegs, radiusAdmin + 0.1f))
        {
            Debug.DrawRay(character.transform.position , character.transform.TransformDirection(Vector3.back) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.right), out hitLegs, radiusAdmin + 0.1f))
        {
            Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.right) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.left), out hitLegs, radiusAdmin + 0.1f))
        {
            Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.left) * hitLegs.distance, Color.red);
            return true;
        }

        return false;
    }
    
#if UNITY_STANDALONE || UNITY_EDITOR 
/*
    private void AdminInPlaceCameraMove()
    {
        float dirY = Input.GetAxis(yAxis);
        float dirX = Input.GetAxis(xAxis);

        admin.Camera.Rotate(dirX, dirY);

        if (Input.GetKeyDown(KeyCode.E))
        {
            adminPlace.StandUp();
            stateMove = State.None;
        }
    }

    private RaycastHit lastHitCamera;

    private void MainRay()
    {
        RaycastHit hitCamera;

        if (Physics.Raycast(admin.Camera.transform.position, admin.Camera.transform.forward, out hitCamera, maxDistanseHitCamera))
        {
            Debug.DrawRay(admin.Camera.transform.position, admin.Camera.transform.TransformDirection(Vector3.forward) * hitCamera.distance, Color.yellow);

            if (hitCamera.collider.transform.parent?.GetComponent<AdminPlace>())
            {
                adminPlace = hitCamera.collider.transform.parent?.GetComponent<AdminPlace>();
                adminPlace.EnableOutline();

                if (Input.GetButtonDown("Fire1"))
                {
                    adminPlace.SeatDown();
                    stateMove = State.AdminPlace;
                }
            }
        }
    }


    private void RayInPlace()
    {
        RaycastHit hitCamera;
        
        if (Physics.Raycast(admin.Camera.transform.position, admin.Camera.transform.forward, out hitCamera, maxDistanseHitCamera))
        {
            Debug.DrawRay(admin.Camera.transform.position, admin.Camera.transform.TransformDirection(Vector3.forward) * hitCamera.distance, Color.yellow);

            if (hitCamera.collider.transform?.GetComponent<MonitorController>())
            {
                monitorController = hitCamera.collider.transform?.GetComponent<MonitorController>();

                if(stateMove == State.AdminPlace)
                {
                    monitorController.EnableOutline();
                }

                if (Input.GetButtonDown("Fire1") && stateMove != State.Monitor)
                {
                    monitorController.EnterMonitor(adminPlace);
                    stateMove = State.Monitor;
                }

                if (Input.GetButtonDown("Fire2") && stateMove == State.Monitor && (Input.GetKeyDown(KeyCode.LeftAlt) == false))
                {
                    monitorController.ExitMonitor();
                    stateMove = State.AdminPlace;
                }

            }
        }
    } 
    */
#endif
}
