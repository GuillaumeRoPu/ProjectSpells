using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    private Item item;

    [SerializeField] private Button button;
    
    public void RemoveItem()
    {
        if(item is Spell)
        {
            InventoryManager.Instance.Remove(item as Spell);
        }
        else if(item is Modifier)
        {
            InventoryManager.Instance.Remove(item as Modifier);
        }    
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }
}
