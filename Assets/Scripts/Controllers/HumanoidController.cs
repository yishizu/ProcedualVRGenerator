using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HumanoidController : MonoBehaviour
{
    public Transform CameraFollow;
    [SerializeField] private HumanoidLandInput _input;

    private Rigidbody _rigidbody = null;
    private CapsuleCollider _capsuleCollider = null;
    
    Vector3 _playerMoveInput= Vector3.zero;
    Vector3 _previousPlayerLookInput = Vector3.zero;
    Vector3 _playerLookInput = Vector3.zero;

    [SerializeField] private float _cameraPitch = 0.0f;
    [SerializeField] private float _playerLookInputLerpTime = 0.35f;

    [Header("Movement")] 
    [SerializeField] private float _movementMultiplier = 30.0f;
    [SerializeField] private float _notGroundedMovementMultiplier =1.25f;
    [SerializeField] private float _rotationSpeedMultiplier = 100.0f;
    [SerializeField] private float _pitchSpeedMutiplier = 180.0f;
    [SerializeField] private float _runMultiplier = 30.0f;

    [Header("Ground Check")] 
    [SerializeField] private bool _playerIsGrounded = true;

    [SerializeField] [Range(0.0f, 1.8f)] private float _groundCheckRadiusMultiplier = 0.5f;

    [SerializeField] [Range(-0.95f, 1.05f)] private float _groundCheckDistance = 0.05f;
    private RaycastHit _groundCheckHit = new RaycastHit();
    

    [Header("Gravity")] 
    [SerializeField] private float _gravityFallCurrent = -100.0f;
    [SerializeField] private float _gravityFallMin = -100.0f;
    [SerializeField] private float _gravityFallMax = -500.0f;
    [SerializeField] [Range(-5.00f, -35f)] private float _gravityFallIncrementAmount = -20f;
    [SerializeField]  private float _gravityFallIncrementTime= 0.05f;
    [SerializeField]  private float _playerFallTimer= 0.0f;
    [SerializeField]  private float _gravityGrounded= -1.0f;
    [SerializeField]  private float _maxSlopeAngle= 47.5f;
    [SerializeField]  private float _gravity= 0.0f;
    
    [Header("Stairs")] 
    [SerializeField] [Range(0.0f, 1.0f)]private float _maxStepHeight= 0.5f;
    [SerializeField] [Range(0.0f, 1.0f)]private float _minStepHeight = 0.2f;
    [SerializeField] private float _stairHeightPaddingMultiplier = 1.5f;
    [SerializeField] private bool _isFirstStep = true;
    [SerializeField] private float _firstStepVelocityDistanceMultiplier = 0.1f;
    [SerializeField] private bool _playerIsAscendingStairs = false;
    [SerializeField] private bool _playerIsDecendingStairs = false;
    [SerializeField] private float _AscendingStairsMovementMultiplier = 0.35f;
    [SerializeField] private float _DescendingStairsMovementMultiplier = 0.7f;
    [SerializeField] private float _maxAngleToAcend = 45f;
    
    private float _playerHeightToGround = 0.0f;
    private float _maxAscendRayDistance = 0.0f;
    private float _minDescendRayDistance = 0.0f;
    private int _numberOfStepDetectRays = 0;
    private float _rayIncrementAmount = 0.0f;
    private float _playerCenterToGroundDistance = 0.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        _maxAscendRayDistance = _maxStepHeight / Mathf.Cos(_maxAngleToAcend * Mathf.Deg2Rad);
        _maxAscendRayDistance = _maxStepHeight / Mathf.Cos(80.0f * Mathf.Deg2Rad);

        _numberOfStepDetectRays = Mathf.RoundToInt(((_maxStepHeight * 100) * 0.5f) + 1f);
        _rayIncrementAmount = _maxStepHeight / _numberOfStepDetectRays;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerLookInput = GetLookInput();
        PlayerLook();
        PitchCamera();

        _playerMoveInput = GetMoveInput();
        _playerIsGrounded = PlayerGroundCheck();

        _playerMoveInput = PlayerMove();
        _playerMoveInput = PlayerStairs();
        _playerMoveInput = PlayerSlope();
        _playerMoveInput = PlayerRun();
        _playerMoveInput.y = PlayerFallGravity();


        _playerMoveInput *= _rigidbody.mass;
        
        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput = _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x,
            (_input.InvertMouseY ? -_input.LookInput.y : _input.LookInput.y), 0.0f);
        return Vector3.Lerp(_previousPlayerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);
    }

     Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

     private void PlayerLook()
     {
         _rigidbody.rotation = Quaternion.Euler(0.0f, _rigidbody.rotation.eulerAngles.y + (_playerLookInput.x*_rotationSpeedMultiplier), 0.0f);
         
     }

     private void PitchCamera()
     {
         _cameraPitch += _playerLookInput.y * _pitchSpeedMutiplier;
         _cameraPitch = Mathf.Clamp(_cameraPitch, -89.9f, 89.9f);
         CameraFollow.rotation = Quaternion.Euler(_cameraPitch, CameraFollow.rotation.eulerAngles.y, CameraFollow.rotation.eulerAngles.z );
     }
    private Vector3  PlayerMove()
    {
        var calculatePlayerMovement = (new Vector3(_playerMoveInput.x * _movementMultiplier * _rigidbody.mass, _playerMoveInput.y*_rigidbody.mass,
            _playerMoveInput.z * _movementMultiplier * _rigidbody.mass));
        return calculatePlayerMovement;
    }

    private bool PlayerGroundCheck()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRadiusMultiplier;
        Physics.SphereCast(_rigidbody.position, sphereCastRadius, Vector3.down, out _groundCheckHit);
        _playerCenterToGroundDistance = _groundCheckHit.distance + sphereCastRadius;
        return ((_playerCenterToGroundDistance >= _capsuleCollider.bounds.extents.y - _groundCheckDistance) &&
               (_playerCenterToGroundDistance <= _capsuleCollider.bounds.extents.y + _groundCheckDistance));
    }


    private Vector3 PlayerStairs()
    {
        Vector3 calculatedStepInpt = _playerMoveInput;

        _playerHeightToGround = _capsuleCollider.bounds.extents.y;
        if (_playerCenterToGroundDistance < _capsuleCollider.bounds.extents.y)
        {
            _playerHeightToGround = _playerCenterToGroundDistance;
        }

        calculatedStepInpt = AscendStairs(calculatedStepInpt);
        if (!_playerIsAscendingStairs)
        {
            calculatedStepInpt = DescendStairs(calculatedStepInpt);
        }
        return calculatedStepInpt;
    }

    private Vector3 AscendStairs(Vector3 calculateStepInput)
    {
        if (_input.MoveIsPressed)
        {
            float calculateVelDistance = _isFirstStep
                ? (_rigidbody.velocity.magnitude * _firstStepVelocityDistanceMultiplier) + _capsuleCollider.radius
                : _capsuleCollider.radius;
            float ray = 0.0f;

            List<RaycastHit> raycastHits = new List<RaycastHit>();
            for(int x=1;x < _numberOfStepDetectRays; x++, ray += _rayIncrementAmount)
            {
                Vector3 rayLower = new Vector3( _rigidbody.position.x, (_rigidbody.position.y - _playerHeightToGround)+ray, _rigidbody.position.z);

                RaycastHit hitlower;
                if (Physics.Raycast(rayLower, _rigidbody.transform.TransformDirection(_playerMoveInput),  out hitlower, calculateVelDistance + _maxAscendRayDistance))
                {
                    float stairSlopeAngle = Vector3.Angle(hitlower.normal, _rigidbody.transform.up);
                    if(stairSlopeAngle ==90.0f)
                    {
                        raycastHits.Add(hitlower);
                    }
                }
            }

            if (raycastHits.Count > 0)
            {
                Vector3 rayUpper = new Vector3(_rigidbody.position.x,
                    (_rigidbody.position.y - _playerHeightToGround)+ _maxStepHeight + _rayIncrementAmount, _rigidbody.position.z);
                RaycastHit hitUpper;
                if (Physics.Raycast(rayUpper, _rigidbody.transform.TransformDirection(_playerMoveInput),  out hitUpper, calculateVelDistance + _maxAscendRayDistance*2f))
                {
                    float stairSlopeAngle = Vector3.Angle(hitUpper.normal, _rigidbody.transform.up);
                    if(stairSlopeAngle ==90.0f)
                    {
                        raycastHits.Add(hitUpper);
                    }
                }
            }
        }

        return calculateStepInput;
    }
    private Vector3 DescendStairs(Vector3 calculateStepInput)
    {
        return calculateStepInput;
    }
    private Vector3 PlayerSlope()
    {
        Vector3 calculatedPlayerMovement = _playerMoveInput;
        Debug.Log(_playerIsGrounded);
        if (_playerIsGrounded && !_playerIsAscendingStairs && !_playerIsDecendingStairs)
        {
            Vector3 localGroundCheckHitNormal = _rigidbody.transform.InverseTransformDirection(_groundCheckHit.normal);
            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, _rigidbody.transform.up);
            Debug.Log(groundSlopeAngle);
            if (groundSlopeAngle > 0f)
            {
                Quaternion slopeAngelRotation = Quaternion.FromToRotation(_rigidbody.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAngelRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, _rigidbody.transform.up) - 90.0f;
                

                if (groundSlopeAngle > _maxSlopeAngle)
                {
                    if (_input.MoveIsPressed)
                    {
                        calculatedPlayerMovement += calculatedPlayerMovement * (relativeSlopeAngle / _maxSlopeAngle);
                        calculatedPlayerMovement.y += _gravityGrounded;
                    }
                }
            }

        }

        return calculatedPlayerMovement;
    }

    private Vector3 PlayerRun()
    {
        Vector3 calculatedPlayerRunspeed = _playerMoveInput;
        if (_input.MoveIsPressed && _input.RunIsPressed)
        {
            calculatedPlayerRunspeed*= _runMultiplier;
        }

        return calculatedPlayerRunspeed;
    }

    private float PlayerFallGravity()
    {
        float gravity = _playerMoveInput.y;
        if (_playerIsGrounded || _playerIsAscendingStairs || _playerIsDecendingStairs)
        {
            _gravityFallCurrent = _gravityFallMin;
        }
        else
        {
            _playerFallTimer -= Time.fixedDeltaTime;
            if (_playerFallTimer < 0.0f)
            {
                float gravityFallMax = _movementMultiplier * _runMultiplier * 5f;
                float gravityFallIncrementAmount = (gravityFallMax - _gravityFallMin) * 0.1f;
                if (_gravityFallCurrent < gravityFallMax)
                {
                    _gravityFallCurrent += _gravityFallIncrementAmount;
                }

                _playerFallTimer = _gravityFallIncrementTime;
            }

            gravity = - _gravityFallCurrent;
        }

        return gravity;
    }
    private float PlayerGravity()
    {
        if (_playerIsGrounded)
        {
            _gravity = 0.0f;
            _gravityFallCurrent = _gravityFallMin;
        }
        else
        {
            _playerFallTimer -= Time.fixedDeltaTime;
            if (_playerFallTimer < 0.0f)
            {
                if (_gravityFallCurrent > _gravityFallMax)
                {
                    _gravityFallCurrent += _gravityFallIncrementAmount;
                }

                _playerFallTimer = _gravityFallIncrementTime;
                _gravity = _gravityFallCurrent;
            }
        }

        return _gravity;
    }
    
}
