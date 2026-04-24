using Core.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicBrain : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _inputNext;
    [SerializeField] InputActionReference _inputPrior;
    [SerializeField] InputActionReference _leftClickInput;
    [SerializeField] InputActionReference _useInput;
    [SerializeField] InputActionReference _sprintAction;
    [SerializeField] InputActionReference _jumpAction;
    [SerializeField] InputActionReference _openMenusAction;
    [SerializeField] BasePlayerController _playerController;

    private void Start()
    {
        _moveInput.action.started += ActionOnStarted;
        _moveInput.action.performed += ActionOnPerformed;
        _moveInput.action.canceled += ActionOnCanceled;
        _inputNext.action.started += ScrolledUp;
        _inputPrior.action.started += ScrolledDown;
        _leftClickInput.action.started += PressedLeftClick;
        _useInput.action.started += PressedE;
        _jumpAction.action.started += PressedJump;
        _jumpAction.action.canceled += ReleasedJump;
        _sprintAction.action.started += StartedSprint;
        _sprintAction.action.canceled += ReleasedSprint;
        _openMenusAction.action.started += OpenCloseMenus;
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
        _jumpAction.action.started -= PressedJump;
        _jumpAction.action.canceled -= ReleasedJump;
        _sprintAction.action.started -= StartedSprint;
        _sprintAction.action.canceled -= ReleasedSprint;
        _openMenusAction.action.started -= OpenCloseMenus;
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

    }

    private void ScrolledDown(InputAction.CallbackContext obj)
    {
        
    }

    private void PressedE(InputAction.CallbackContext obj)
    {
        if (!GameManager.GM.State.HasFlag(GameManager.GameState.UI))
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
        
    }

    private void PressedJump(InputAction.CallbackContext obj)
    {
        if (!GameManager.GM.State.HasFlag(GameManager.GameState.UI))
        {
            _playerController.PressedJump();
        }
    }

    private void ReleasedJump(InputAction.CallbackContext obj)
    {
        if (!GameManager.GM.State.HasFlag(GameManager.GameState.UI))
        {
            _playerController.ReleasedJump();
        }
    }

    private void StartedSprint(InputAction.CallbackContext obj)
    {
        if (!GameManager.GM.State.HasFlag(GameManager.GameState.UI))
        {
            _playerController.ActivateSprint(true);
        }
    }

    private void ReleasedSprint(InputAction.CallbackContext obj)
    {
        if (!GameManager.GM.State.HasFlag(GameManager.GameState.UI))
        {
            _playerController.ActivateSprint(false);
        }
    }

    private void OpenCloseMenus(InputAction.CallbackContext obj)
    {
        if (GameManager.GM.State == GameManager.GameState.UI)
        {

        }
        else if (GameManager.GM.State == GameManager.GameState.Gameplay)
        {
        }
    }
}
