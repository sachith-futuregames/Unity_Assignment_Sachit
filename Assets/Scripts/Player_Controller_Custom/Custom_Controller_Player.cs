using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Custom_Controller_Player : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    //Variables For Movement
    private readonly float _gravity = -0.6f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _velocityOnY = Vector3.zero;
    public float Speed = 10.0f;
    private bool _bIsGrounded = false;
    private bool _bIsJumping = false;
    private string PlayerName;
    public bool bIsActive;
    public bool bIsAlive;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 InVelocity)
    {
        _rb.velocity = InVelocity;
    }

    public void GetMoveDirection()
    {
        if (bIsActive)
        {
        //    Vector2 MovementVec = playerInputActions.Player_G.Move.ReadValue<Vector2>();
        //    _velocity.x = MovementVec.x;
        //    _velocity.z = MovementVec.y;
        //    _velocity *= Speed;
        }
    }

    private void PlayeronY()
    {
        if (!_bIsGrounded)
        {
            if (_bIsJumping)
            {
                _bIsJumping = false;
            }
            _velocityOnY.y += _gravity;
            Move(_velocityOnY * Time.deltaTime);
        }
        if (_bIsJumping)
        {
            _bIsGrounded = false;
            _velocityOnY.y = 0;
            _velocityOnY.y += 40.0f;
            Move(_velocityOnY * Time.deltaTime);
        }
    }
}
