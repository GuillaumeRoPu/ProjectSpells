using UnityEngine;
using UnityEngine.InputSystem;

public class Brain : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _InputNext;
    [SerializeField] InputActionReference _InputPrior;
    [SerializeField] InputActionReference _leftClickInput;
    [SerializeField] InputActionReference _UseInput;
    [SerializeField] BobController _playerController;

    private void Start()
    {
        _moveInput.action.started += ActionOnStarted;
        _moveInput.action.performed += ActionOnPerformed;
        _moveInput.action.canceled += ActionOnCanceled;
        _InputNext.action.started += ScrolledUp;
        _InputPrior.action.started += ScrolledDown;
        _leftClickInput.action.started += PressedLeftClick;
        _UseInput.action.started += PressedE;
    }

    private void OnDestroy()
    {
        _moveInput.action.started -= ActionOnStarted;
        _moveInput.action.performed -= ActionOnPerformed;
        _moveInput.action.canceled -= ActionOnCanceled;
        _InputNext.action.started -= ScrolledUp;
        _InputPrior.action.started -= ScrolledDown;
        _leftClickInput.action.started -= PressedLeftClick;
        _UseInput.action.started -= PressedE;
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
}

