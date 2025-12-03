using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventoryCanvas;

    public List<Spell> spells { get; private set; } = new List<Spell>();
    public List<Modifier> modifiers { get; private set; } = new List<Modifier>();
    private List<Spell> _equippedSpells = new List<Spell>();
    public List<Spell> EquippedSpells { get => _equippedSpells; }
    [SerializeField] private int _spellCapacity;
    public int SpellCapacity { get { return _spellCapacity; } }
    [SerializeField] private int _maximumSpellCapacity;

    [field: SerializeField] public Transform _equippedSpellsContent { get; private set; }
    [SerializeField] private GameObject UIEquippedSlot;

    [field : SerializeField] public Transform spellsContent { get; private set; }
    [SerializeField] private GameObject UISpell;

    [field: SerializeField] public Transform modifiersContent { get; private set; }
    [SerializeField] private GameObject UIModifier;

    [SerializeField] private Toggle EnableRemove;

    public Action EquippedSpell;
    public Action UnequippedSpell;


    public List<InventoryItemController> inventorySpells = new List<InventoryItemController>();
    public List<InventoryItemController> inventoryModifiers = new List<InventoryItemController>();



    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Mathf.Clamp(_spellCapacity, 1, _maximumSpellCapacity);
        for (int i = 0; i < _spellCapacity; i++) 
        {
            _equippedSpells.Add(null);
        }
    }

    [NaughtyAttributes.Button]
    private void AddSpell()
    {
        Spell spell = new Spell();
        Add(spell);
    }

    [NaughtyAttributes.Button]
    private void AddModifier()
    {
        Modifier modifier = new Modifier();
        Add(modifier);
    }

    [NaughtyAttributes.Button]
    private void Updatedee()
    {
        UpdateListItems();
    }

    public void Add(Modifier modifier)
    {
        modifiers.Add(modifier);
        if (UIManager.Instance.inMenus)
        {
            UpdateListItems();
        }
    }

    public void Add(Spell spell)
    {
        spells.Add(spell);
        if (UIManager.Instance.inMenus)
        {
            UpdateListItems();
        }
    }

    public void Remove(Spell spell)
    {
        spells.Remove(spell);
        if (UIManager.Instance.inMenus)
        {
            UpdateListItems();
        }
    }

    public void Remove(Modifier modifier)
    {
        modifiers.Remove(modifier);
        if (UIManager.Instance.inMenus)
        {
            UpdateListItems();
        }
    }

    public void EquipSpell(Spell spell, int position)
    {
        _equippedSpells[position] = spell;
        Remove(spell);
        EquippedSpell?.Invoke();
    }

    public void UnEquipSpell(Spell spell, int position)
    {
        _equippedSpells[position] = null;
        Add(spell);
        UnequippedSpell?.Invoke();
        Debug.Log("Unequiped Item At Slot number : " + position);
    }

    public void ListItems()
    {
        ListSpells();
        ListModifiers();
        ListEquippedSpells();
    }

    private void UpdateListItems()
    {
        Debug.Log("Updated");
        ListSpells();
        ListModifiers();
        ListEquippedSpells();
    }

    private void ListEquippedSpells()
    {
        foreach (Transform item in _equippedSpellsContent)
        {
            Destroy(item.gameObject);
        }
        int i = 0;
        foreach (var spell in _equippedSpells)
        {
            GameObject obj = Instantiate(UIEquippedSlot, _equippedSpellsContent);
            GameObject ItemInObj = Instantiate(UISpell, obj.transform);
            if (spell != null)
            {
                ItemInObj.gameObject.SetActive(true);
                ItemInObj.name = spell.itemName;
                ItemInObj.transform.position = obj.transform.position;
                var spellName = ItemInObj.transform.Find("ItemName").GetComponent<Text>();
                var spellIcon = ItemInObj.transform.Find("ItemIcon").GetComponent<Image>();

                spellName.text = "";
                spellIcon.sprite = spell.icon;
            }
            else
            {
                ItemInObj.gameObject.SetActive(false);
            }

            var equipSlot = obj.GetComponent<EquippedItemSlotController>();

            if (equipSlot != null)
            {
                equipSlot.SetID(i);
                if(_equippedSpells[i] != null)
                {
                    equipSlot.EquipSpell(_equippedSpells[i]);
                }
                else if(equipSlot.isSpellStored())
                {
                    equipSlot.UnequipSpell();
                }
            }
            else
            {
                Debug.LogWarning("y'a un probleme");
            }
            i++;
        }
    }

    private void ListSpells()
    {
        foreach(Transform item in spellsContent)
        {
            Destroy(item.gameObject);
        }

        inventorySpells.Clear();

        foreach (var spell in spells)
        {
            GameObject obj = Instantiate(UISpell, spellsContent);
            var a = obj.GetComponent<InventoryItemController>();
            if (a != null)
            {
                inventorySpells.Add(a);
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

        for (int i = 0; i < spells.Count; i++)
        {
            inventorySpells[i].AddItem(spells[i]);
        }
    }

    private void ListModifiers()
    {
        foreach (Transform item in modifiersContent)
        {
            Destroy(item.gameObject);
        }

        inventoryModifiers.Clear();

        foreach (var modifier in modifiers)
        {
            GameObject obj = Instantiate(UIModifier, modifiersContent);
            var modifierName = obj.transform.Find("ItemName").GetComponent<Text>();
            var modifierIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveItem").gameObject;

            var a = obj.GetComponent<InventoryItemController>();
            inventoryModifiers.Add(a);

            modifierName.text = modifier.itemName;
            modifierIcon.sprite = modifier.icon;

            if (EnableRemove.isOn) 
            {
                removeButton.SetActive(true);
            }
        }

        for (int i = 0; i < modifiers.Count; i++)
        {
            inventoryModifiers[i].AddItem(modifiers[i]);
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


    private void ClearInventoryUI()
    {
        foreach (Transform item in spellsContent)
            Destroy(item.gameObject);

        foreach (Transform item in modifiersContent)
            Destroy(item.gameObject);

        foreach (Transform item in _equippedSpellsContent)
            Destroy(item.gameObject);
    }


    public void CloseInventory()
    {
        ClearInventoryUI();
        _inventoryCanvas.SetActive(false);
    }

    public bool isRemoveOn()
    {
        return EnableRemove.isOn;
    }
}
