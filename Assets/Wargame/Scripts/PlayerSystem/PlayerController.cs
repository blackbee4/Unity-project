using System;
using UnityEngine;

namespace WargameSystem.PlayerSystem
{
    [RequireComponent(typeof(CharacterController), typeof(WargameInputs))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Stats")]
        public float MoveSpeed = 3.0f;

        public float SprintSpeed = 5.5f;

        public float JumpHeight = 1.2f;

        public float RotationSpeed = 1.0f;
        
        public float Gravity = -15.0f;

        public float FallTimeout = 0.15f;

        [Header("Ground Check")]
        public bool Grounded = true;

        public float GroundedOffset = -0.14f;

        public float GroundedRadius = 0.28f;

        public LayerMask GroundLayers;

        [Header("Camera")]
        public GameObject CameraTarget;

        private float _cinemachineTargetPitch;
        
        private float _speed;
        private float _verticalVelocity;

        private float _fallTimeoutDelta;

        private int _animIDSpeed;
        private int _animIDJump;

        private Animator _animator;
        private CharacterController _controller;
        private WargameInputs _input;
        private GameObject _cam;

        private void Awake()
        {
            if (!_cam)
            {
                _cam = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<WargameInputs>();

            AssignAnimationIDs();
            
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            JumpAndGravity(); 
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDJump = Animator.StringToHash("Jump");
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }
            }
            else
            {
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                // 혹시 Value가 아니라 Button을 사용한 경우
                // _input.jump = false;
            }

            _verticalVelocity += Gravity * Time.deltaTime;
            
        }

        private void GroundedCheck()
        {
            Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePos, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }

        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            if (_input.move == Vector2.zero) targetSpeed = 0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;

            const float speedOffset = 0.1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * 10f);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 moveDir = transform.right * _input.move.x + transform.forward * _input.move.y;

            _controller.Move(moveDir.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
        }

        private void CameraRotation()
        {
            // 만약 입력값이 존재한다면
            if (_input.look.sqrMagnitude >= 0.01f)
            {
                _cinemachineTargetPitch -= _input.look.y * RotationSpeed;

                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, -90f, 90f);
                
                CameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);
                
                transform.Rotate(_input.look.x * RotationSpeed * Vector3.up);
            }
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}