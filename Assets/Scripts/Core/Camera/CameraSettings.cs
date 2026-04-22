using UnityEngine;

namespace Core
{
    namespace Cam
    {
        [CreateAssetMenu(fileName = "CameraSetting", menuName = "Camera/CameraSetting", order = 0)]
        public class CameraSettings : ScriptableObject
        {
            [SerializeField] private float _baseFOV = 60;
            public float BaseFOV { get { return _baseFOV; } }
            [SerializeField] private Vector3 _targetOffset;
            public Vector3 TargetOffset { get { return _targetOffset; } }
            [SerializeField] private float _sensitivityMultiplier = 1;
            public float SensitivityMult { get { return _sensitivityMultiplier; } }
            [SerializeField] private Vector3 _baseRotation;
            public Vector3 BaseRotation { get { return _baseRotation; } }
        }
    }
}
