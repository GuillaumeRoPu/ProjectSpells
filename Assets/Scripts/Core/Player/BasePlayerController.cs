using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    namespace Player
    {
        public class BasePlayerController : MonoBehaviour
        {

            //Reference Stuff
            [SerializeField] private BasicBrain _brain;
            [SerializeField] private Transform _transform;
            [SerializeField] private Rigidbody _rb;
            [SerializeField] private Camera _camera;
            [SerializeField] private Cam.CameraController _cameraController;

            [Space(5)]

            //Movement Stuff
            [SerializeField] private float _speed;
            [SerializeField] private float _acceleration;
            [SerializeField] private float _decceleration;
            [SerializeField] private float _velocityPower;
            [SerializeField] private bool _canUseSprint;
            [SerializeField, ShowIf(nameof(_canUseSprint))] private float _sprintSpeed;
            private float _slippingMeter = 1;
            private float _speedMult = 1;
            private bool _isSprinting = false;
            private bool _isGrounded = false;
            private float _airTimer;
            [SerializeField] private bool _canUseJump;
            [SerializeField, ShowIf(nameof(_canUseJump))] private float _coyoteTime;
            [SerializeField, ShowIf(nameof(_canUseJump))] private int _maxJumps;
            [SerializeField, ShowIf(nameof(_canUseJump))] private float _jumpStrength;
            [SerializeField, ShowIf(nameof(_canUseJump))] private float _airJumpStrength;
            [SerializeField, ShowIf(nameof(_canUseJump))] private Vector3 _raycastPos;
            [SerializeField, ShowIf(nameof(_canUseJump))] private float _raycastLength;
            [SerializeField, ShowIf(nameof(_canUseJump))] private LayerMask _groundLayers;
            private bool _pressedJump;
            private int _jumpsLeft;
            private bool _isJumping;
            private bool _hasJumped;

            private Vector3 _direction;


            // Anim stuff
            [SerializeField] private Animator _animator;

            private static readonly int AnimSpeed = Animator.StringToHash("Speed");
            private static readonly int AnimIsGrounded = Animator.StringToHash("IsGrounded");
            private static readonly int AnimIsSprinting = Animator.StringToHash("IsSprinting");
            private static readonly int AnimJump = Animator.StringToHash("Jump");


            private void FixedUpdate()
            {
                CheckPlatform();
                Jump();
                Move();
                GravityChanges();
                CameraAdjustement();
                UpdateAnimations();
            }

            [Button]
            private void NormalPlatform()
            {
                _slippingMeter = 1f;
                _speedMult = 1f;
            }

            private void AirControl()
            {
                _slippingMeter = 2f;
                _speedMult = 1f;
            }

            private void TranslateGroundValues((float slip, float speedMult) theBestTuple)
            {
                _slippingMeter = theBestTuple.slip;
                _speedMult = theBestTuple.speedMult;
            }


            public void OnMove(Vector3 direction)
            {
                _direction = direction.normalized;
            }

            private void Move()
            {
                var forward = _camera.transform.forward;
                forward.y = 0;
                forward.Normalize();
                var right = _camera.transform.right;
                right.y = 0;
                right.Normalize();
                Vector3 updatedDirection = (right * _direction.x + forward * _direction.z).normalized;
                float speed = _speed;
                if (_isSprinting && _canUseSprint)
                {
                    speed = _sprintSpeed;
                }
                Vector3 targetSpeed = updatedDirection * speed * _speedMult;
                Vector3 speedDif = targetSpeed - _rb.linearVelocity * _slippingMeter;
                float accelRate = (Mathf.Abs(targetSpeed.x) > 0.01f || Mathf.Abs(targetSpeed.z) > 0.01f) ? _acceleration * _slippingMeter : _decceleration * _slippingMeter;
                Vector3 movement = new Vector3(Mathf.Pow(Mathf.Abs(speedDif.x) * accelRate, _velocityPower) * Mathf.Sign(speedDif.x), 0, Mathf.Pow(Mathf.Abs(speedDif.z) * accelRate, _velocityPower) * Mathf.Sign(speedDif.z));
                _rb.AddForce(movement);
                //if the player is on a platform, it adds the platform Veclocity to the player each frame
                //if (onPlatform)
                //{
                //    transform.position += (Vector3)platformVelocity * Time.fixedDeltaTime;
                //}
            }

            private void CameraAdjustement()
            {
                
                Vector3 newForward = _rb.linearVelocity;
                newForward.y = 0;
                newForward.Normalize();

                if (newForward != Vector3.zero)
                {
                    _transform.forward = newForward;
                }
            }

            private void Jump()
            {
                if (!_canUseJump)
                {
                    return;
                }

                if (_isGrounded && _isJumping && _jumpsLeft > 0 && !_hasJumped)
                {
                    Debug.Log("Jumped");
                    _hasJumped = true;
                    _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
                    _rb.AddForce(new Vector3(0, _jumpStrength, 0), ForceMode.Impulse);

                    _animator?.SetTrigger(AnimJump);
                }
                else if (_jumpsLeft > 0 && !_isGrounded && _pressedJump)
                {
                    Debug.Log("Air jumped");
                    _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
                    _rb.AddForce(new Vector3(0, _airJumpStrength, 0), ForceMode.Impulse);
                    _jumpsLeft--;

                    _animator?.SetTrigger(AnimJump);
                }
                _pressedJump = false;
            }

            public void ReleasedJump()
            {
                _isJumping = false;
            }

            public void PressedJump()
            {
                Debug.Log("Pressed Jump");
                _isJumping = true;
                _pressedJump = true;
            }

            private void CheckPlatform()
            {
                RaycastHit[] hit = Physics.RaycastAll(_transform.position + _raycastPos, -_transform.up, _raycastLength, _groundLayers);
                bool foundGround = false;
                foreach (RaycastHit i in hit)
                {
                    if (!_hasJumped || _rb.linearVelocity.y <= 0.01f)
                    {
                        foundGround = true;
                    }
                    GroundValues ground = i.collider.GetComponent<GroundValues>();
                    if (ground != null)
                    {
                        TranslateGroundValues(ground.GiveValues());
                    }
                    else if (_coyoteTime < _airTimer)
                    {
                        NormalPlatform();
                    }
                }

                if (foundGround)
                {
                    _isGrounded = true;
                    _airTimer = 0;
                    _jumpsLeft = _maxJumps;
                    _hasJumped = false;
                    Debug.Log("Found Ground " + _jumpsLeft);
                }
                else
                {
                    AirControl();
                    _airTimer += Time.deltaTime;
                    if ((_airTimer > _coyoteTime && _isGrounded == true) || (_isGrounded && _hasJumped))
                    {
                        _isGrounded = false;
                        _jumpsLeft -= 1;
                    }
                }
            }

            public void ActivateSprint(bool isSprinting)
            {
                _isSprinting = isSprinting;
            }

            private void GravityChanges()
            {

            }

            private void OnDrawGizmosSelected()
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_transform.position + _raycastPos, _transform.position + _raycastPos - new Vector3(_transform.up.x, _transform.up.y * _raycastLength, _transform.up.z));
            }


            private void UpdateAnimations()
            {
                if (_animator == null) return;

                Vector3 horizontalVelocity = _rb.linearVelocity;
                horizontalVelocity.y = 0;

                float speed = horizontalVelocity.magnitude;

                _animator.SetFloat(AnimSpeed, speed);
                _animator.SetBool(AnimIsGrounded, _isGrounded);
                _animator.SetBool(AnimIsSprinting, _isSprinting && speed > 0.1f);
            }

        }
    }
}

