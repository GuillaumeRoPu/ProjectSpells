using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController Instance;

    //Reference Stuff
    [SerializeField] private Brain _brain;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameObject _prefabSpellController;
    [SerializeField] private Transform _parentCastedSpells;

    [Space(5)]

    //Movement Stuff
    [SerializeField] private float _speed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _decceleration;
    [SerializeField] private float _velocityPower;
    private float _slippingMeter = 1;
    private float _speedMult = 1;
    private bool _isSprinting = false;
    private bool _isGrounded = false;
    private float _airTimer;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private int _maxJumps;
    [SerializeField] private float _jumpStrenth;
    [SerializeField] private float _airJumpStrength;
    private int _jumpsLeft;

    [Space(5)]

    //Health Stuff
    [SerializeField] private float _maxHealth;
    [SerializeField, ProgressBar(nameof(_maxHealth), EColor.Red)] private float _health;
    public float MaxHealth { get { return _maxHealth; }}
    public float CurrentHealth { get { return _health; } }

    [Space(5)]

    //Spells Stuff
    private Spell _inHandSpell = null;
    private float _internalCooldown;
    public Spell InHandSpell { get => _inHandSpell; }
    private int _inHandSpellPosition = 0;
    public int InHandSpellPosition { get => _inHandSpellPosition; }

    public Action ChangedHotBarSpell;

    private Vector3 _direction;

    [Button]
    private void NormalPlatform()
    {
        _slippingMeter = 1f;
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InventoryManager.Instance.EquippedSpell += UpdateInHandSpell;
        InventoryManager.Instance.UnequippedSpell += UpdateInHandSpell;
        _internalCooldown = 0;
        _inHandSpell = null;
    }

    private void Update()
    {
        _internalCooldown -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.EquippedSpell -= UpdateInHandSpell;
        InventoryManager.Instance.UnequippedSpell -= UpdateInHandSpell;
    }

    private void FixedUpdate()
    {
        CheckPlatform();
        Move();
        CameraAdjustement();
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
        if (_isSprinting)
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
        _cameraController.ChangeFOV(_rb.linearVelocity.magnitude / (speed * _speedMult));
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

    public void Jump()
    {
        if (_jumpsLeft > 0)
        {
            _jumpsLeft -= 1;
            if (_isGrounded)
            {
                _isGrounded = false;
            }
            else {
                _isGrounded = false;

            }
        }
    }

    private void CheckPlatform()
    {
        RaycastHit[] hit = Physics.RaycastAll(_transform.position, -_transform.up, 0.5f);
        bool foundGround = false;
        foreach (RaycastHit i in hit)
        {
            if (i.collider.CompareTag("Ground"))
            {
                if (_airTimer > 0.05f)
                {
                    foundGround = true;
                }
                GroundValues ground = i.collider.GetComponent<GroundValues>();
                if (ground != null)
                {
                    TranslateGroundValues(ground.GiveValues());
                }
                else
                {
                    NormalPlatform();
                }
            }
        }

        if (foundGround)
        {
            _isGrounded = true;
            _airTimer = 0;
            _jumpsLeft = _maxJumps;
        }
        else
        {
            _airTimer += Time.deltaTime;
            if (_airTimer > _coyoteTime && _isGrounded == true)
            {
                _isGrounded = false;
                _jumpsLeft -= 1;
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
        Damage(20);
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

    public void Damage(float damageAmount)
    {
        if (damageAmount < 0)
        {
            Debug.LogWarning("WHY IS THE NUMBER UNDER 0");
            return;
        }
        _health -= damageAmount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        Debug.Log("Removed Health amount : " + damageAmount);
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        throw new NotImplementedException();
    }


    // SPELLS // SPELLS // SPELLS // SPELLS // SPELLS // SPELLS // SPELLS // SPELLS // SPELLS // SPELLS

    void EquipSpell(Spell spell, int positionInAllEquippedSpells)
    {
        _inHandSpell = spell;
        _inHandSpellPosition = positionInAllEquippedSpells;
    }

    public void ChangeSpellUp()
    {
        ChangedHotBarSpell?.Invoke();
        List<Spell> EquippedSpells = InventoryManager.Instance.EquippedSpells;
        int i = _inHandSpellPosition;
        bool foundSpell = false;
        int start = i;
        int bloqueur = 0;
        if (i + 1 >= EquippedSpells.Count)
        {
            i = 0;
        }
        else
        {
            i++;
        }
        while (!foundSpell)
        {
            if (EquippedSpells[i] == null)
            {
                if (i + 1 >= EquippedSpells.Count)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            else
            {
                EquipSpell(EquippedSpells[i], i);
                Debug.Log("Equipped Spell in Slot : " + i);
                foundSpell = true;
            }
            if (bloqueur >= 10)
            {
                foundSpell = true;
                Debug.Log("No Spell Found");
            }
            bloqueur++;
        }
    }
 

    public void ChangeSpellDown()
    {
        ChangedHotBarSpell?.Invoke();
        List<Spell> EquippedSpells = InventoryManager.Instance.EquippedSpells;
        int i = _inHandSpellPosition;
        bool foundSpell = false;
        int start = i;
        int bloqueur = 0;
        if (i - 1 < 0)
        {
            i = EquippedSpells.Count - 1;
        }
        else
        {
            i--;
        }
        while (!foundSpell)
        {
            if (EquippedSpells[i] == null)
            {
                if (i - 1 < 0)
                {
                    i = EquippedSpells.Count - 1;
                }
                else
                {
                    i--;
                }
            }
            else
            {
                EquipSpell(EquippedSpells[i], i);
                Debug.Log("Equipped Spell in Slot : " + i);
                foundSpell = true;
            }
            if (bloqueur >= 10)
            {
                foundSpell = true;
                Debug.Log("No spell Found");
            }
            bloqueur++;
        }
    }

    private void UpdateInHandSpell()
    {
        EquipSpell(InventoryManager.Instance.EquippedSpells[_inHandSpellPosition], _inHandSpellPosition);
    }

    public void CastInHandSpell()
    {
        if (_internalCooldown <= 0 && _inHandSpell != null)
        {
            Debug.Log("Pourquoi tu l'as fais");
            Instantiate(_prefabSpellController, _parentCastedSpells);
            var casSpeCont = _prefabSpellController.GetComponent<CastedSpellController>();
            casSpeCont.InitiateSpell(_inHandSpell);
            _internalCooldown = _inHandSpell.cooldown;
        }
    }
}