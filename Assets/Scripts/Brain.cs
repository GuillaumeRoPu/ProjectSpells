using UnityEngine;
using UnityEngine.InputSystem;

public class Brain : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _InputNext;
    [SerializeField] InputActionReference _InputPrior;
    [SerializeField] BobController _playerController;

    private void Start()
    {
        _moveInput.action.started += ActionOnStarted;
        _moveInput.action.performed += ActionOnPerformed;
        _moveInput.action.canceled += ActionOnCanceled;
        _InputNext.action.started += ScrolledUp;
        _InputPrior.action.started += ScrolledDown;
    }

    private void OnDestroy()
    {
        _moveInput.action.started -= ActionOnStarted;
        _moveInput.action.performed -= ActionOnPerformed;
        _moveInput.action.canceled -= ActionOnCanceled;
        _InputNext.action.started -= ScrolledUp;
        _InputPrior.action.started -= ScrolledDown;
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
    }

    private void ScrolledDown(InputAction.CallbackContext obj)
    {
        Debug.Log("Scrolled Down");
    }
}

