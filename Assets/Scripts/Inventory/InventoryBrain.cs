using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryBrain : MonoBehaviour
{
    public static InventoryBrain Instance;

    [SerializeField] private InputActionReference _leftClick;
    [SerializeField] private BobController _playerController;
    [SerializeField] private GameObject _pickedUpItemSlot;


    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    private void Awake()
    {
        Instance = this;
    }

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
            else if(UIManager.Instance.inMenus && !InventoryManager.Instance.isRemoveOn())
            {
                PointerEventData pointerData = new PointerEventData(eventSystem);
                pointerData.position = Mouse.current.position.ReadValue();
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    var item = result.gameObject.GetComponent<InventoryItemController>();
                    if (item != null && item.currentState == InventoryItemController.UIState.Content)
                    {
                        item.PickUp();
                        item.transform.SetParent(_pickedUpItemSlot.transform);
                        Debug.LogWarning("ok y'a un prob");
                        break;
                    }
                    var slot = result.gameObject.GetComponent<EquippedItemSlotController>();
                    if (slot != null)
                    {
                        item = slot.GetComponentInChildren<InventoryItemController>();
                        if (item != null)
                        {
                            Destroy(item.gameObject);
                            InventoryManager.Instance.UnEquipSpell(item.item as Spell, slot.GetID());
                            break;
                        }
                        //slot.UnequipSpell();
                    }
                }

            }
        }
    }

    private void ReleasedLeftClick(InputAction.CallbackContext obj)
    {
        if (UIManager.Instance != null && UIManager.Instance.inMenus) 
        {
            InventoryItemController item = _pickedUpItemSlot.GetComponentInChildren<InventoryItemController>();
            if (item != null)
            {
                PointerEventData pointerData = new PointerEventData(eventSystem);
                pointerData.position = Mouse.current.position.ReadValue();
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerData, results);

                var state = new InventoryItemController.UIState();
                state = InventoryItemController.UIState.Content;
                bool foundContent = false;

                foreach (RaycastResult result in results)
                {
                    if (item.item is Spell)
                    {
                        EquippedItemSlotController slot = result.gameObject.GetComponent<EquippedItemSlotController>();
                        if (slot != null)
                        {
                            InventoryManager.Instance.EquipSpell(item.item as Spell, slot.GetID());
                            //slot.EquipSpell(item.item as Spell);
                            foundContent = true;
                            item.transform.SetParent(slot.transform);
                            state = InventoryItemController.UIState.Slot;
                            break;
                        }
                    }
                }
                item.PlaceDown(state);
                Debug.Log(item.item is Spell);
                if (item.item is Spell && !foundContent)
                {
                    item.transform.SetParent(InventoryManager.Instance.spellsContent);
                }
                else if (item.item is Modifier && !foundContent)
                {
                    item.transform.SetParent(InventoryManager.Instance.modifiersContent);
                }
            }
        }
    }
}
