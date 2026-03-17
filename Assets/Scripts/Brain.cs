using UnityEngine;
using UnityEngine.InputSystem;

public class Brain : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _inputNext;
    [SerializeField] InputActionReference _inputPrior;
    [SerializeField] InputActionReference _leftClickInput;
    [SerializeField] InputActionReference _useInput;
    [SerializeField] InputActionReference _sprintAction;
    [SerializeField] InputActionReference _jumpAction;
    [SerializeField] PlayerController _playerController;

    private void Start()
    {
        _moveInput.action.started += ActionOnStarted;
        _moveInput.action.performed += ActionOnPerformed;
        _moveInput.action.canceled += ActionOnCanceled;
        _inputNext.action.started += ScrolledUp;
        _inputPrior.action.started += ScrolledDown;
        _leftClickInput.action.started += PressedLeftClick;
        _useInput.action.started += PressedE;
        _jumpAction.action.performed += HoldJump;
        _sprintAction.action.performed += HoldSprint;
    }

    private void OnDestroy()
    {
        _moveInput.action.started -= ActionOnStarted;
        _moveInput.action.performed -= ActionOnPerformed;
        _moveInput.action.canceled -= ActionOnCanceled;
        _inputNext.action.started -= ScrolledUp;
        _inputPrior.action.started -= ScrolledDown;
        _leftClickInput.action.started -= PressedLeftClick;
        _useInput.action.started -= PressedE;
        _jumpAction.action.performed -= HoldJump;
        _sprintAction.action.performed -= HoldSprint;
    }

    private void ActionOnPerformed(InputAction.CallbackContext obj)
    {
        var direction = obj.ReadValue<Vector2>();
        _playerController.OnMove(new Vector3(direction.x, 0, direction.y));
    }

    private void ActionOnStarted(InputAction.CallbackContext obj)
    {
        var direction = obj.ReadValue<Vector2>();
        _playerController.OnMove(new Vector3(direction.x, 0, direction.y));
    }

    private void ActionOnCanceled(InputAction.CallbackContext obj)
    {
        var direction = Vector2.zero;
        _playerController.OnMove(new Vector3(direction.x, 0, direction.y));
    }

    private void ScrolledUp(InputAction.CallbackContext obj)
    {
        Debug.Log("Scrolled Up");
        _playerController.ChangeSpellUp();
    }

    private void ScrolledDown(InputAction.CallbackContext obj)
    {
        Debug.Log("Scrolled Down");
        _playerController.ChangeSpellDown();
    }

    private void PressedE(InputAction.CallbackContext obj)
    {
        if (!UIManager.Instance.inMenus)
        {
            RaycastHit hit;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(rayOrigin, out hit))
            {
                ItemPickup item = hit.transform.gameObject.GetComponent<ItemPickup>();
                if (item != null)
                {
                    item.PickUp();
                }
            }
            Debug.Log("PickedUp");
        }
    }

    private void PressedLeftClick(InputAction.CallbackContext obj)
    {
        if (!UIManager.Instance.inMenus)
        {
            _playerController.CastInHandSpell();
            Debug.Log("PressedLeftClick");
        }
    }

    private void HoldJump(InputAction.CallbackContext obj)
    {
        if (!GameManager.GM.State.HasFlag(GameManager.GameState.UI))
        {
            _playerController.CastInHandSpell();
            Debug.Log("PressedLeftClick");
        }
    }

    private void HoldSprint(InputAction.CallbackContext obj)
    {
        
    }
}

