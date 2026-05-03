
using System.Collections.Generic;
using UnityEngine;
using System;
using Core.Menu;

public class BasicInventoryManager : MonoBehaviour
{
    public static BasicInventoryManager Instance;

    [SerializeField] private GameObject inventoryPanel;

    public List<BasicItem> Items { get; private set; } = new List<BasicItem>();

    public Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        OnInventoryChanged?.Invoke();
    }

    public void Add(BasicItem item)
    {
        if (item == null) return;

        Items.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void Remove(BasicItem item)
    {
        if (Items.Remove(item))
        {
            OnInventoryChanged?.Invoke();
        }
    }

    public void OpenInventory()
    {
        PageController.Instance.TurnPageOn(PageType.Inventory);
        OnInventoryChanged?.Invoke();
    }

    public void CloseInventory()
    {
        PageController.Instance.TurnPageOff(PageType.Inventory);
        GameManager.GM.ChangeState(GameManager.GameState.Gameplay);
    }
}
