using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class Player_Controller : MonoBehaviour, IHealthSystem
{
    [SerializeField] CharacterController characterController;
    public GameObject TPPCameraTarget;
    public GameObject FPPCameraTarget;
    public GameObject AirCameraTarget;
    [SerializeField] MeshFilter _playerMesh;
    [SerializeField] MeshRenderer _playerMeshRenderer;
    [SerializeField] Mesh _deathMesh;
    [SerializeField] Material _deathMaterial;
    public GlobalGameManager GlobalManager;

    
    //private PlayerInput playerInput;
    public Player_Input_G playerInputActions;
    
    //Variables For Movement
    private readonly float _gravity = -0.6f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _velocityOnY = Vector3.zero;
    public float Speed = 10.0f;
    private bool _bIsGrounded = false;
    private bool _bIsJumping = false;
    public bool bIsActive;
    public bool bIsAlive;
    public bool bIsZoom = false;
    
    //Variables for Cameras
    public CinemachineVirtualCamera TPPCam;
    public CinemachineVirtualCamera FPPCam;
    //public CinemachineVirtualCamera AirDropCam;
    [SerializeField]  ActiveCameraManager CameraManager;

    //Health System
    private float _health = 10;


    //WeaponSystem
    [SerializeField] private WeaponsManager _weaponsManager;
    public int TeamID;
    private List<Player_Controller> _meleeEnemies;
    private void Awake()
    {
        bIsActive = false;
        bIsAlive = true;
        characterController = GetComponent<CharacterController>();
        
        playerInputActions = new Player_Input_G();
        playerInputActions.Player_G.Enable();

        //Input Callback Bindings
        playerInputActions.Player_G.Jump.started += Jump;
        playerInputActions.Player_G.Zoom.started += ToggleZoomBind;
        playerInputActions.Player_G.Shoot.started += Shoot;
        
        TPPCam = GameObject.Find("TPPCamera").GetComponent<CinemachineVirtualCamera>();
        FPPCam = GameObject.Find("FPPCamera").GetComponent<CinemachineVirtualCamera>();
        CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ActiveCameraManager>();
        
    }

    public void Start()
    {
        _weaponsManager.TeamID = TeamID;
        _weaponsManager.OnAttackComplete += EndTurn;
        _weaponsManager.OnAttackComplete += GlobalManager.NextTurn;
    }
    // Update is called once per frame
    void Update()
    {
        GetMoveDirection();
        PlayeronY();
        RotateCam();
        characterController.Move(_velocity * Time.deltaTime);
    }
    //Additional Forces to Move Player and Move the Player
    private void PlayeronY()
    {
        if(!_bIsGrounded)
        {
            if(_bIsJumping)
            {
                _bIsJumping = false;
            }
            _velocityOnY.y += _gravity;
            characterController.Move(_velocityOnY * Time.deltaTime);
        }
        if(_bIsJumping)
        {
            _bIsGrounded = false;
            _velocityOnY.y = 0;
            _velocityOnY.y += 40.0f;
            characterController.Move(_velocityOnY * Time.deltaTime);
        }
    }
    //Get the movement Directions from the Input bindings
    public void GetMoveDirection()
    {   
        if(bIsActive && !bIsZoom)
        {
            Vector2 MovementVec = playerInputActions.Player_G.Move.ReadValue<Vector2>();
            //_velocity.x = MovementVec.x;
            _velocity = TPPCam.transform.forward * MovementVec.y + TPPCam.transform.right * MovementVec.x;
            _velocity.y = 0;
            _velocity *= Speed;
        }
        else
        {
            _velocity = Vector3.zero;
        }
    }
    //Jump Function that is bound to the Jump Input Action
    public void Jump(InputAction.CallbackContext inputContext)
    {
        if(inputContext.started && bIsActive && !bIsZoom)
        {
            _bIsJumping = true;
        }
    }
    //Checking whether the Character Controller is Touching the Ground
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.CompareTag("Ground") && !_bIsGrounded)
        {
            _bIsGrounded = true;
            
        }
    }

    public void RotateCam()
    {
        if(bIsActive && !bIsZoom)
        {
            gameObject.transform.Rotate(0, playerInputActions.Player_G.MoveTPPCam.ReadValue<Vector2>().x, 0);
            if(TPPCameraTarget.transform.localRotation.x > -10 && TPPCameraTarget.transform.localRotation.x < 40)
            {
                TPPCameraTarget.transform.Rotate(100 * Time.deltaTime * new Vector3(-playerInputActions.Player_G.MoveTPPCam.ReadValue<Vector2>().y, 0, 0));
            }
            
        }
        else if(bIsActive && bIsZoom && CameraManager.CurrentCamera == ECurrentCam.Air)
        {
            AirCameraTarget.transform.Rotate(100 * Time.deltaTime * new Vector3(0, playerInputActions.Player_G.MoveAirCam.ReadValue<Vector2>().x, 0));
        }
        
        
    }

    public void TakeDamage(int InDamage)
    {
        Debug.Log("Hit: Health: " + _health);
        _health -= InDamage;
        if(_health < 0)
        {
            DeathState();
        }
    }

    public void GetHealth(int InHealth)
    {
        _health += InHealth;
    }

    public void DeathState()
    {
        bIsAlive = false;
        _playerMesh.mesh = _deathMesh;
        _playerMeshRenderer.material = _deathMaterial;
        _weaponsManager.DeathState();
    }

    public void ToggleZoomBind(InputAction.CallbackContext inputContext)
    {
        if (inputContext.started && bIsActive)
        {
            ToggleZoom();
        }
    }
    
    private void ToggleZoom()
    {
        if (bIsZoom)
        {
            bIsZoom = false;
            TPPCameraTarget.transform.localRotation = Quaternion.Euler(0, 0, 0);
            CameraManager.SetActiveTPPCamera(TPPCameraTarget.transform);
            CameraManager.SetActiveCamera(ECurrentCam.TPP);
        }
        else
        {
            if (_weaponsManager.GetCurrentWeapon().TypeWeapon == EWeaponType.AirDrop)
            {

                AirCameraTarget.transform.localRotation = Quaternion.Euler(0, 0, 0);
                CameraManager.SetActiveAirCamera(AirCameraTarget.transform);
                CameraManager.SetActiveCamera(ECurrentCam.Air);
                bIsZoom = true;
            }
            if (_weaponsManager.GetCurrentWeapon().TypeWeapon == EWeaponType.Projectile || _weaponsManager.GetCurrentWeapon().TypeWeapon == EWeaponType.RayCast)
            {
                FPPCameraTarget.transform.localRotation = Quaternion.Euler(0, 0, 0);
                FPPCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = gameObject.transform.rotation.y;
                CameraManager.SetActiveFPPCamera(FPPCameraTarget.transform);
                CameraManager.SetActiveCamera(ECurrentCam.FPP);
                bIsZoom = true;
            }
        }
    }

    public void SetPlayerMesh(Mesh InMesh)
    {
        _playerMesh.mesh = InMesh;
    }

    public void Shoot(InputAction.CallbackContext inputContext)
    {
        if(inputContext.started && bIsActive)
        {
            if(_weaponsManager.GetCurrentWeapon().TypeWeapon == EWeaponType.Melee)
            {
                _weaponsManager.Shoot();
            }
            else if(bIsZoom)
            {
                _weaponsManager.Shoot();
            }
        }
    }

    private void EndTurn()
    {
        ToggleZoom();
    }
}
