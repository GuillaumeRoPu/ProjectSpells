using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryBrain : MonoBehaviour
{
    [SerializeField] private InputActionReference _leftClick;
    [SerializeField] private BobController _playerController;
    [SerializeField] private GameObject _pickedUpItemSlot;


    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;


    private void Start()
    {
        _leftClick.action.started += PressedLeftClick;
        _leftClick.action.canceled += ReleasedLeftClick;
    }

    private void OnDestroy()
    {
        _leftClick.action.started -= PressedLeftClick;
        _leftClick.action.canceled -= ReleasedLeftClick;
    }

    private void PressedLeftClick(InputAction.CallbackContext obj)
    {
        if (UIManager.Instance != null)
        {
            if (!UIManager.Instance.inUI)
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
            else
            {
                PointerEventData pointerData = new PointerEventData(eventSystem);
                //OLD NEEDS TO MAKE A BETTER VERSION
                pointerData.position = Mouse.current.position.ReadValue();

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    InventoryItemController item = result.gameObject.GetComponent<InventoryItemController>();
                    if (item != null)
                    {
                        item.PickUp();
                        item.transform.SetParent(_pickedUpItemSlot.transform);
                    }
                }

            }
        }
    }

    private void ReleasedLeftClick(InputAction.CallbackContext obj)
    {
        if (UIManager.Instance != null) 
        {
            InventoryItemController item = _pickedUpItemSlot.GetComponentInChildren<InventoryItemController>();
            if (UIManager.Instance.inUI && item != null)
            {
                item.PlaceDown(InventoryItemController.UIState.Content);
                if(item.item is Spell)
                {
                    item.transform.SetParent(InventoryManager.Instance.spellsContent);
                }
                else if(item.item is Modifier)
                {
                    item.transform.SetParent(InventoryManager.Instance.modifiersContent);
                }
            }
        }
    }
}
