using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryBrain : MonoBehaviour
{
    [SerializeField] InputActionReference _leftClick;

    private void Start()
    {
        _leftClick.action.started += RaycastItem;
    }

    private void OnDestroy()
    {
        _leftClick.action.started -= RaycastItem;
    }

    private void RaycastItem(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(rayOrigin, out hit))
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
