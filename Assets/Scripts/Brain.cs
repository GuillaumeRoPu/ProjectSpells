using UnityEngine;
using UnityEngine.InputSystem;

public class Brain : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] BobController _playerController;

    private void Start()
    {
        _moveInput.action.started += ActionOnStarted;
        _moveInput.action.performed += ActionOnPerformed;
        _moveInput.action.canceled += ActionOnCanceled;
    }

    private void OnDestroy()
    {
        _moveInput.action.started -= ActionOnStarted;
        _moveInput.action.performed -= ActionOnPerformed;
        _moveInput.action.canceled -= ActionOnCanceled;
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

    private void RaycastItem(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(rayOrigin, out hit))
        {
            ItemPickup item = hit.transform.gameObject.GetComponent<ItemPickup>();
            if (hit.transform.gameObject.GetComponent<ItemPickup>() != null)
            {
                item.PickUp();
            }
        }
        Debug.Log("PickedUp");
    }
}

