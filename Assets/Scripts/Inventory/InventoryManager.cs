using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [field : SerializeField] public List<Spell> spells { get; private set; } = new List<Spell>();
    [field : SerializeField] public List<Modifier> modifiers { get; private set; } = new List<Modifier>();

    public Transform spellsContent;
    public GameObject UISpell;

    public Transform modifiersContent;
    public GameObject UIModifier;

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
        ListSpells();
        ListModifiers();
    }

    private void ListSpells()
    {
        foreach (var spell in spells)
        {
            GameObject obj = Instantiate(UISpell, spellsContent);
            obj.name = spell.itemName;
            var spellName = obj.transform.Find("ItemName").GetComponent<Text>();
            var spellIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            spellName.text = spell.itemName;
            spellIcon.sprite = spell.icon;
        }
    }

    private void ListModifiers()
    {
        foreach (var modifier in modifiers)
        {
            GameObject obj = Instantiate(UIModifier, modifiersContent);
            var modifierName = obj.transform.Find("ItemName").GetComponent<Text>();
            var modifierIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            modifierName.text = modifier.itemName;
            modifierIcon.sprite = modifier.icon;
        }
    }
}
