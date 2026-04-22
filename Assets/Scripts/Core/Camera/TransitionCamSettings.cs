using NaughtyAttributes;
using UnityEngine;


namespace Core
{
    namespace Cam
    {
        [CreateAssetMenu(fileName = "TransitionCamSetting", menuName = "Camera/TransitionSetting", order = 1)]
        public class TransitionCamSettings : ScriptableObject
        {
            [SerializeField] private bool _instantaneous; 
            public bool Instantaneous { get { return _instantaneous; } }
            [SerializeField, MinValue(0)] private float _duration;
            public float Duration { get { return _duration; } }
            [SerializeField] private bool _useCurve;
            public bool UseCurve { get { return _useCurve; } }
            [SerializeField] private AnimationCurve _curve;
            public AnimationCurve Curve { get { return _curve; } }
            [SerializeField] private CameraSettings _firstCamSetting;
            public CameraSettings FirstCamSetting { get { return _firstCamSetting; } }
            [SerializeField] private CameraSettings _secondCamSetting;
            public CameraSettings SecondCamSetting { get { return _secondCamSetting; } }

            public void ChangeSettings(bool instantaneous = false, float duration = 0, AnimationCurve animCurve = null)
            {
                _instantaneous = instantaneous;
                _duration = duration;
                if (_duration <= 0)
                {
                    _duration = 0;
                }
                if (animCurve != null) 
                { 
                    _useCurve = true;
                }
                _curve = animCurve;
            }
        }

    }
}
