using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    //[SerializeField] private ItemDataBase itemDataBase;
    private Brain _playerBrain;
    private Item _item;
    [SerializeField] private ItemController _controller;
    public Action onPickup;

    public void PickUp()
    {
        switch (_controller.CATEGORY)
        {
            case CATEGORY.Spells:
                _item = _controller.SpellData;
                InventoryManager.Instance.Add(_item as Spell);
                break;
            case CATEGORY.Modifiers:
                _item = _controller.ModifierData;
                InventoryManager.Instance.Add(_item as Modifier);
                break;
        }
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log("PickedUp");
        PickUp();
    }
}
