﻿ using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
using System.Collections;
using System.Collections.Generic;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        //singleon
        public static ThirdPersonController Main;

        public CharacterMovement iaControls;
        private InputAction sprint;
        private InputAction jump;
        private InputAction move;
        [HideInInspector] public float targetSpeed;
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        public float AirborneSpeedChangeRate;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Tooltip("Higher values means the player slows down more quickly from extra forces")]
        [Range(1f, 100f)]
        private float GroundFriction = 5f;
        [Range(1f, 100f)]
        private float AirFriction = 3f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // GMHN edited fields
        private bool _movementLocked;
        [HideInInspector] public bool _lunaLocked;
        [HideInInspector] public bool _paused;
        [HideInInspector] public bool _stunned;
        [HideInInspector] public bool _canMove = true;
        [HideInInspector] public bool _inDialogue;
        [HideInInspector] public bool _manipulatingLasso;
        [HideInInspector] public bool _inCinematic;
        private Vector3 extraMotion;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

            //pickups
        bool standingOnPickup;
        public GameObject pickupUnderPlayer;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;


        private PlayerInput characterMovement;

#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";

                //return _playerInput.currentControlScheme == "CharacterMovement";

#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            iaControls = new CharacterMovement();
            _canMove = true;
            _inDialogue = false;
        }

        private void Start()
        {
            //singleton behavior
            Main = this;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            _movementLocked = IsMovementLocked();
            JumpAndGravity();
            GroundedCheck();
            DecelerateForces();
            Move();

        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            //Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                //QueryTriggerInteraction.Ignore);
                Grounded = GetComponent<CharacterController>().isGrounded;

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            //float targetSpeed = sprint.triggered ? SprintSpeed : MoveSpeed;
            if(sprint.IsPressed()){
                targetSpeed = SprintSpeed;
            }
            else{
                targetSpeed = MoveSpeed;
            }

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (move.ReadValue<Vector2>() == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? move.ReadValue<Vector2>().magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                float accelRate = SpeedChangeRate;
                if (!Grounded) accelRate = AirborneSpeedChangeRate;

                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * accelRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            var moving = move.ReadValue<Vector2>();
            Vector3 inputDirection = new Vector3(moving.x, 0.0f, moving.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (move.ReadValue<Vector2>() != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  Camera.main.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                if(!_movementLocked)
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;


            //set _speed to 0. Returning at top just causes animation issues galore.
            //pretending there is no input works better.
            if (_movementLocked)
            {
                _speed = 0.0f;
                targetDirection = new Vector3(0.0f, 0.0f, 0.0f);
                _animationBlend = 0.0f;
            }
            // move the player
            Vector3 moveVector = targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
            //Debug.Log("Move vector: " + moveVector + "\nMotion vector: " + extraMotion);
            if(!_movementLocked)
                if(_controller.enabled) _controller.Move(moveVector + (extraMotion * Time.deltaTime));

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if ((jump.triggered && _jumpTimeoutDelta <= 0.0f || 
                    GetComponent<LassoGrappleScript>().grappling && _jumpTimeoutDelta <= 0.0f)
                    && !_movementLocked)
                {
                    _jumpTimeoutDelta = 0;
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void Death()
        {
            Debug.Log("You died!");
        }

        //centralizing information about whether the player is locked
        //without having information accessible from wrong areas
        //for example, unpausing shouldn't unlock from luna's redirect movement lock
        private bool IsMovementLocked()
        {
            //dont delete this stuff because it's helpful debug.
            /*
            Debug.Log("_paused: " + _paused);
            Debug.Log("_lunaLocked: " + _lunaLocked);
            Debug.Log("!_canMove: " + !_canMove);
            Debug.Log("_inDialogue: " + _inDialogue);
            Debug.Log("_manipulatingLasso: " + _manipulatingLasso);
            */
            return _paused 
                || _lunaLocked || _stunned 
                || _inDialogue || _manipulatingLasso 
                || _inCinematic;
        }

        public void LockPlayerForDuration(float seconds)
        {
            StartCoroutine(LockPlayerRoutine(seconds));
        }

        private IEnumerator LockPlayerRoutine(float seconds)
        {
            _inCinematic = true;
            yield return new WaitForSeconds(seconds);
            _inCinematic = false;
        }

        public void ForceStartConversation()
        {
            _inDialogue = true;
        }

        public void ForceStopConversation()
        {
            _inDialogue = false;
        }

        private void OnEnable(){
            sprint = iaControls.CharacterControls.Sprint;
            move = iaControls.CharacterControls.Move;
            jump = iaControls.CharacterControls.Jump;

            sprint.Enable();
            move.Enable();
            jump.Enable();
        }
        private void OnDisable(){
            sprint.Disable();
            move.Disable();
            jump.Disable();
         }   
        
        public void ChangeSpeed(float newValue)
        {
            SprintSpeed += newValue;
            MoveSpeed += newValue;
        }

        public void ChangeSpeedByFactor(float factor)
        {
            SprintSpeed *= factor;
            MoveSpeed *= factor;
        }

        public void SetMotion(Vector3 motion)
        {
            extraMotion = motion;
        }

        public void Push(Vector3 force)
        {
            extraMotion += force;
        }

        //called every frame
        public void DecelerateForces()
        {
            if(extraMotion != Vector3.zero) Debug.Log("Extra motion: " + extraMotion);
            if(extraMotion.magnitude > 0.1f)
            {
                if (Grounded)
                {
                    extraMotion -= (Time.deltaTime * GroundFriction) * extraMotion;
                    //extraMotion /= 1 + (GroundFriction * Time.deltaTime);

                    /*
                    extraMotion.x = extraMotion.x / (1 + (GroundFriction * Time.deltaTime));
                    extraMotion.y = extraMotion.y / (1 + (GroundFriction * Time.deltaTime));
                    extraMotion.z = extraMotion.z / (1 + (GroundFriction * Time.deltaTime));
                    */

                }
                else
                {
                    extraMotion -= Time.deltaTime * AirFriction * extraMotion;
                    //extraMotion /= 1 + (AirFriction * Time.deltaTime);

                    /*
                    extraMotion.x = extraMotion.x / (1 + (AirFriction * Time.deltaTime));
                    extraMotion.y = extraMotion.y / (1 + (AirFriction * Time.deltaTime));
                    extraMotion.z = extraMotion.z / (1 + (AirFriction * Time.deltaTime));
                    */
                }
                    
            } else
            {
                //snap off
                extraMotion = new Vector3 (0, 0, 0);
            }
            
        }

        public Vector3 CorePosition()
        {
            return transform.position + new Vector3(0, 1.5f, 0);
        }

    }
    

}