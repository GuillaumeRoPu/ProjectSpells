using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    private Item item;

    [SerializeField] private Button button;
    
    public void RemoveItem()
    {
        if (item is Spell)
        {
            InventoryManager.Instance.Remove(item as Spell);
        }
        else if (item is Modifier)
        {
            InventoryManager.Instance.Remove(item as Modifier);
        }
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem() 
    {
        BobController.Instance.AddHealth(item.value);
        RemoveItem();
    }
}
