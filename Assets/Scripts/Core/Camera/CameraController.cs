using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Core
{
    namespace Cam
    {
        public class CameraController : MonoBehaviour
        {
            [SerializeField] private Camera _camera;
            [SerializeField] private bool _isTargetAnObject;
            [SerializeField, HideIf(nameof(_isTargetAnObject))] private Vector3 _targetPosition;
            [SerializeField, ShowIf(nameof(_isTargetAnObject))] private GameObject _targetObject;
            [SerializeField] private float _sensitivity;
            [SerializeField] private CameraSettings _baseCamSettings;
            [SerializeField] private CameraSettings _scopeCamSettings;
            private CameraSettings _currentCameraSettings;
            private Vector3 _currentOffset;

            [SerializeField] private bool _canControl;

            private float _rotationY;
            private float _rotationX;

            private float _distanceFromTarget = 3.0f;

            private Vector3 _currentRotation;
            private Vector3 _smoothVelocity = Vector3.zero;

            [SerializeField]
            private float _smoothTime = 0.2f;

            [SerializeField]
            private Vector2 _rotationXMinMax = new Vector2(-40, 40);

            [SerializeField] private InputActionReference _cameraLook;
            [SerializeField] private InputActionReference _resetCamera;


            public void EnableControl(bool canControl)
            {
                _canControl = canControl;
            }

            private void Start()
            {
                _currentCameraSettings = ScriptableObject.CreateInstance<CameraSettings>();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            private void OnDestroy()
            {
                _resetCamera.action.started -= SnapCameraBehindTarget;
            }

            private void LateUpdate()
            {
                _currentCameraSettings = _baseCamSettings;
                //TO BE COMPLETED : the offset needs to depend on where the camera is looking. Basically, the offset needs to rotate with the camera
                _currentOffset = _currentCameraSettings.TargetOffset;

                UpdatePosition();
            }

            private void LookAt()
            {
                Vector3 target = new Vector3();
                if (_isTargetAnObject)
                {
                    target = _targetObject.transform.position + _currentOffset;
                }
                else
                {
                    target = _targetPosition + _currentOffset;
                }
                _camera.transform.LookAt(target);
            }

            private void UpdatePosition()
            {
                //Vector2 mouseDelta = _cameraLook.action.ReadValue<Vector2>();
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                float mouseX = mouseDelta.x * _sensitivity * Time.deltaTime;
                float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime * -1;

                _rotationY += mouseX;
                _rotationX += mouseY;

                _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

                Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

                _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
                transform.localEulerAngles = _currentRotation;

                transform.position = _targetObject.transform.position - transform.forward * _distanceFromTarget;
            }

            public void ChangeTarget(Vector3 target)
            {
                _isTargetAnObject = false;
            }

            public void ChangeTarget(Transform target, Vector3 offset)
            {
                _isTargetAnObject = true;
            }

            private void SnapCameraBehindTarget(InputAction.CallbackContext obj)
            {
                if (_isTargetAnObject)
                {
                    //_targetObject.transform.forward;
                }
            }

            public void SwitchCamSettings(CameraSettings settings, TransitionCamSettings transition = null)
            {
                if (transition != null)
                {
                    StopCoroutine(nameof(SwitchCamSettingsCor));
                    StartCoroutine(SwitchCamSettingsCor(settings, transition));
                }
                else
                {
                    _currentCameraSettings = settings;
                }
            }

            private IEnumerator SwitchCamSettingsCor(CameraSettings settings, TransitionCamSettings transition)
            {
                float timer = 0;
                while (transition.Duration > timer)
                {
                    timer++;
                    //TO BE COMPLETED
                    yield return null;
                }
                _currentCameraSettings = settings;
            }

            private void OnDrawGizmos()
            {
                Vector3 cameraPosition = new Vector3();
                Vector3 lookAtPosition = new Vector3();
                CameraSettings usedCamSettings = new CameraSettings();
                Gizmos.color = Color.yellow;
                if (_isTargetAnObject && _targetObject != null && _currentCameraSettings != null)
                {
                    lookAtPosition = _targetObject.transform.position + _currentCameraSettings.TargetOffset;
                }
                else if (_currentCameraSettings != null)
                {
                    lookAtPosition = _targetPosition + _currentCameraSettings.TargetOffset;
                }
                Gizmos.DrawWireSphere(lookAtPosition, 1);
            }

            [Button]
            private void ChangeCurrentCamSettingsToBase()
            {
                _currentCameraSettings = _baseCamSettings;
            }

            [Button]
            private void ChangeCurrentCamSettingsToScope()
            {
                _currentCameraSettings = _scopeCamSettings;
            }
        }
    }
}

