using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject _inventoryPanel;

    public List<Spell> spells { get; private set; } = new List<Spell>();
    public List<Modifier> modifiers { get; private set; } = new List<Modifier>();
    public List<Spell> equippedSpells { get; private set; } = new List<Spell>();
    [SerializeField] private int _spellCapacity;
    [SerializeField] private int _maximumSpellCapacity;

    [field: SerializeField] public Transform equippedSpellsContent { get; private set; }
    [SerializeField] private GameObject UIEquippedSlot;

    [field : SerializeField] public Transform spellsContent { get; private set; }
    [SerializeField] private GameObject UISpell;

    [field: SerializeField] public Transform modifiersContent { get; private set; }
    [SerializeField] private GameObject UIModifier;

    [SerializeField] private Toggle EnableRemove;

    public InventoryItemController[] inventorySpells;
    public InventoryItemController[] inventoryModifiers;



    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Mathf.Clamp(_spellCapacity, 1, _maximumSpellCapacity);
        for (int i = 0; i < _spellCapacity; i++) 
        {
            equippedSpells.Add(null);
            Debug.Log(i);
        }
    }

    public void Add(Modifier modifier)
    {
        modifiers.Add(modifier);
    }

    public void Add(Spell spell)
    {
        spells.Add(spell);
    }

    public void Remove(Spell spell)
    {
        spells.Remove(spell);
    }

    public void Remove(Modifier modifier)
    {
        modifiers.Remove(modifier);
    }

    public void EquipSpell(Spell spell, int position)
    {
        equippedSpells[position] = spell;
        Remove(spell);
        ListItems();
    }

    public void UnEquipSpell(Spell spell, int position)
    {
        equippedSpells[position] = null;
        Add(spell);
        ListItems();
    }

    public void ListItems()
    {
        if (!_inventoryPanel.activeSelf)
        {
            ListSpells();
            ListModifiers();
            SetInventoryItems();
            ListEquippedSpells();
        }
    }

    private void ListEquippedSpells()
    {
        foreach (Transform item in equippedSpellsContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var spell in equippedSpells)
        {
            GameObject obj = Instantiate(UIEquippedSlot, equippedSpellsContent);
            var itemUI = obj.transform.Find("Item");
            if (spell != null)
            {
                itemUI.gameObject.SetActive(true);
                itemUI.name = spell.itemName;
                var spellName = itemUI.transform.Find("ItemName").GetComponent<Text>();
                var spellIcon = itemUI.transform.Find("ItemIcon").GetComponent<Image>();

                spellName.text = spell.itemName;
                spellIcon.sprite = spell.icon;
            }
            else
            {
                itemUI.gameObject.SetActive(false);
            }
            Debug.Log("Yes I have been there");
        }
    }

    private void ListSpells()
    {
        foreach(Transform item in spellsContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var spell in spells)
        {
            GameObject obj = Instantiate(UISpell, spellsContent);
            obj.name = spell.itemName;
            var spellName = obj.transform.Find("ItemName").GetComponent<Text>();
            var spellIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveItem").gameObject;

            spellName.text = spell.itemName;
            spellIcon.sprite = spell.icon;

            if (EnableRemove.isOn)
            {
                removeButton.SetActive(true);
            }
        }
    }

    private void ListModifiers()
    {
        foreach (Transform item in modifiersContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var modifier in modifiers)
        {
            GameObject obj = Instantiate(UIModifier, modifiersContent);
            var modifierName = obj.transform.Find("ItemName").GetComponent<Text>();
            var modifierIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveItem").gameObject;

            modifierName.text = modifier.itemName;
            modifierIcon.sprite = modifier.icon;

            if (EnableRemove.isOn) 
            {
                removeButton.SetActive(true);
            }
        }
    }

    public void EnableItemsRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in spellsContent)
            {
                item.Find("RemoveItem").gameObject.SetActive(true);
            }
            foreach (Transform item in modifiersContent)
            {
                item.Find("RemoveItem").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in spellsContent)
            {
                item.Find("RemoveItem").gameObject.SetActive(false);
            }
            foreach (Transform item in modifiersContent)
            {
                item.Find("RemoveItem").gameObject.SetActive(false);
            }
        }
    }

    private void SetInventoryItems()
    {
        inventorySpells = spellsContent.GetComponentsInChildren<InventoryItemController>();
        inventoryModifiers = modifiersContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < spells.Count; i++)
        {
            inventorySpells[i].AddItem(spells[i]);
        }
        for (int i = 0; i < modifiers.Count; i++)
        {
            inventoryModifiers[i].AddItem(modifiers[i]);
        }
    }


    private void ClearInventoryUI()
    {
        foreach (Transform item in spellsContent)
            Destroy(item.gameObject);

        foreach (Transform item in modifiersContent)
            Destroy(item.gameObject);
    }


    public void CloseInventory()
    {
        ClearInventoryUI();
        _inventoryPanel.SetActive(false);
    }

    public bool isRemoveOn()
    {
        return EnableRemove.isOn;
    }
}
