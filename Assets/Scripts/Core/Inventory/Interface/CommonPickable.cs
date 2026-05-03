using UnityEngine;

public class CommonPickable : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] private BasicItem item;

    public void OnInteract()
    {
        PickUp();
    }

    public void PickUp()
    {
        BasicInventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }
}
