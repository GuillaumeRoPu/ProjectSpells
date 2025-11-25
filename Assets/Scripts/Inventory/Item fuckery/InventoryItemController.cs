using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public enum UIState
    {
        Content,
        PickedUp,
        Slot,
    }

    public UIState currentState { get; private set; }

    public Item item { get; private set; }

    [SerializeField] private Button button;

    private void Awake()
    {
        currentState = UIState.Content;
    }

    private void Update()
    {
        if (currentState == UIState.PickedUp)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

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

    public void PickUp() 
    {
        Debug.Log("Picked Up From Inventory");
        currentState = UIState.PickedUp;
    }

    public void PlaceDown(UIState state)
    {
        currentState = state;
    }
}
