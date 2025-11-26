using NaughtyAttributes;
using UnityEngine;

public class EquippedItemSlotController : MonoBehaviour
{
    [SerializeField, ReadOnly] private Spell storedSpell;
    private int id;

    public void EquipSpell(Spell spell)
    {
        storedSpell = spell;
        var item = GetComponentInChildren<InventoryItemController>();
        if (item!= null)
        {
            item.AddItem(spell);
            item.PlaceDown(InventoryItemController.UIState.Slot);
        }
        Debug.LogWarning(storedSpell.itemName);
    }

    public void UnequipSpell()
    {
        Debug.LogWarning("Unequipped Spell");
        storedSpell = null;
    }

    public int GetID()
    {
        return id;
    }

    public void SetID(int newId)
    {
        id = newId;
    }

    public bool isSpellStored()
    {
        return storedSpell != null;
    }
}
