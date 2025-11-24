using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BobController : MonoBehaviour
{
    public static BobController Instance;

    //Reference Stuff
    [SerializeField] private Brain _brain;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraController _cameraController;

    //Movement Stuff
    [SerializeField] private float _speedMove;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _decceleration;
    [SerializeField] private float _velocityPower;
    private float _slippingMeter = 1;
    private float _speedMult = 1;

    //Health Stuff
    [SerializeField] private float _maxHealth;
    [SerializeField, ProgressBar(nameof(_maxHealth), EColor.Red)] private float _health;

    //Mouse Stuff
    public Item _inHandItem { get; private set; }
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;


    private Vector3 _direction;

    [Button]
    private void NormalPlatform()
    {
        _slippingMeter = 1f;
        _speedMult = 1f;
    }


    [Button]
    private void IcyPlatform()
    {
        _slippingMeter = 0.2f;
        _speedMult = 1.5f;
    }


    public void OnMove(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckPlatform();
        Move();
        CameraAdjustement();
        //_transform.Translate(_direction * Time.deltaTime * 10f);
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
        Vector3 targetSpeed = updatedDirection * _speedMove * _speedMult;
        Vector3 speedDif = targetSpeed - _rb.linearVelocity * _slippingMeter;
        float accelRate = (Mathf.Abs(targetSpeed.x) > 0.01f || Mathf.Abs(targetSpeed.z) > 0.01f) ? _acceleration * _slippingMeter : _decceleration * _slippingMeter;
        Vector3 movement = new Vector3(Mathf.Pow(Mathf.Abs(speedDif.x) * accelRate, _velocityPower) * Mathf.Sign(speedDif.x), 0, Mathf.Pow(Mathf.Abs(speedDif.z) * accelRate, _velocityPower) * Mathf.Sign(speedDif.z));
        _rb.AddForce(movement);
        //if the player is on a platform, it adds the platform Veclocity to the player each frame
        //if (onPlatform)
        //{
        //    transform.position += (Vector3)platformVelocity * Time.fixedDeltaTime;
        //}
        _cameraController.ChangeFOV(_rb.linearVelocity.magnitude / (_speedMove * _speedMult));
    }

    private void CameraAdjustement()
    {
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        if (cameraForward != Vector3.zero)
        {
            _transform.forward = cameraForward;
        }
    }

    private void CheckPlatform()
    {
        RaycastHit[] hit = Physics.RaycastAll(_transform.position, -_transform.up, 10.0f);
        foreach (RaycastHit i in hit)
        {
            if (i.collider.CompareTag("Ice"))
            {
                IcyPlatform();
            }
            else if (i.collider.CompareTag("Ground"))
            {
                NormalPlatform();
            }
        }
    }





    // HEALTH // HEALTH // HEALTH // HEALTH // HEALTH // HEALTH // HEALTH // HEALTH // HEALTH

    [Button]
    private void Add20Health()
    {
        AddHealth(20);
    }

    [Button]
    private void Remove20Health()
    {
        RemoveHealth(20);
    }

    public void AddHealth(float value)
    {
        if (value < 0)
        {
            Debug.LogWarning("WHY IS THE NUMBER UNDER 0");
            return;
        }
        _health += value;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        Debug.Log("Added Health amount : " + value);
    }

    public void RemoveHealth(float value)
    {
        if (value < 0)
        {
            Debug.LogWarning("WHY IS THE NUMBER UNDER 0");
            return;
        }
        _health -= value;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        Debug.Log("Removed Health amount : " + value);
    }



    // MOUSE // MOUSE // MOUSE // MOUSE // MOUSE // MOUSE // MOUSE // MOUSE // MOUSE // MOUSE 

    public void SetInHandItem(Item item)
    {
         _inHandItem = item;
    }


}