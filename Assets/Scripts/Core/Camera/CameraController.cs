using NaughtyAttributes;
using System.Collections;
using UnityEngine;


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


            public void EnableControl(bool canControl)
            {
                _canControl = canControl;
            }

            private void Start()
            {
                
            }

            private void LateUpdate()
            {
                _currentCameraSettings = _baseCamSettings;
                //TO BE COMPLETED : the offset needs to depend on where the camera is looking. Basically, the offset needs to rotate with the camera
                _currentOffset = _currentCameraSettings.TargetOffset;

                UpdatePosition();
                LookAt();
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

            }

            public void ChangeTarget(Vector3 target)
            {
                _isTargetAnObject = false;
            }

            public void ChangeTarget(Transform target, Vector3 offset)
            {
                _isTargetAnObject = true;
            }

            public void SnapCameraBehindTarget()
            {
                if (_isTargetAnObject)
                {

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

