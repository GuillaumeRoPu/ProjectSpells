using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [field : SerializeField] public List<Spell> spells { get; private set; } = new List<Spell>();
    [field : SerializeField] public List<Modifier> modifiers { get; private set; } = new List<Modifier>();

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
}
