using UnityEngine;

public class EquippedItemSlotController : MonoBehaviour
{
    private Spell storedSpell;
    private int id;

    public void EquipSpell(Spell spell)
    {
        InventoryManager.Instance.EquipSpell(spell, id);
        storedSpell = spell;
    }

    public void UnequipSpell()
    {
        InventoryManager.Instance.UnEquipSpell(storedSpell, id);
        storedSpell = null;
    }
}
