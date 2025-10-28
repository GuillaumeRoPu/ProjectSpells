using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _camera;
    [SerializeField] private float _baseFOV = 40.0f;
    [SerializeField] private float _maxFOV = 80.0f;
    [SerializeField] private float _lerpTime = 0.25f;
    [SerializeField] private float smoothSpeed = 5f;
    private float _timer = 0;
    private float _targetFOV;

    private void Start()
    {
        TryGetComponent(out _camera);
        _timer = 0;
        _targetFOV = _baseFOV;
    }

    private void Update()
    {
        _camera.Lens.FieldOfView = Mathf.Lerp(_camera.Lens.FieldOfView, _targetFOV, Time.deltaTime * smoothSpeed);
    }

    public void ChangeFOV(float FOV)
    {
        _targetFOV = Mathf.Lerp(_baseFOV, _maxFOV, FOV);
    }
}
