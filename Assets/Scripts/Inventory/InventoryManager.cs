using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject inventoryPanel;

    [field : SerializeField] public List<Spell> spells { get; private set; } = new List<Spell>();
    [field : SerializeField] public List<Modifier> modifiers { get; private set; } = new List<Modifier>();

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

    public void ListItems()
    {
        if (!inventoryPanel.activeSelf)
        {
            ListSpells();
            ListModifiers();
            SetInventoryItems();
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
        inventoryPanel.SetActive(false);
    }

}
