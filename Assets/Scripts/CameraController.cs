using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float baseFOV = 40.0f;
    private float targetFOV;

    private void Start()
    {
        targetFOV = baseFOV;

    }

    public void ChangeFOV(float FOV)
    {
        targetFOV = FOV;
    }
}
