using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class Item
{
    public string label;
    public int id;
    public string itemName;
    [NonSerialized] public string displayName;
    [TextArea] public string description;
    public int value;
    public Sprite icon;
    [Label("OnGroundObject"), AllowNesting]
    public GameObject prefab;

    public virtual void PickUp3D()
    {

    }

    public virtual void PickUpUI()
    {

    }

    public virtual void DropUI()
    {

    }

    public virtual void Drop3D()
    {

    }
}

public enum ModifierType
{
    Size,
    Damage,
    Route,
    Speed,
    Direction,
}

[Serializable]
public class Modifier : Item
{

    public ModifierType modType;

    public Spell assignedSpell;

    public void AddToSlot(ModifierSlot slot, Spell spell)
    {
        if (this.modType == slot.modType && slot.slotMode == ModifierSlot.SlotMode.Modifier)
        {
            slot.SetModifier(this);
            if (assignedSpell == null)
            {
                assignedSpell = spell;
            }
        }
    }

    public void RemoveFromSlot(ModifierSlot slot)
    {
        slot.RemoveModifier(this);
        assignedSpell = null;
    }
}

[Serializable]
public class Spell : Item
{
    public Spell assignedSpell;

    [Space(10)]
    public List<ModifierSlot> modifiersList = new();

    public Action onWait1Second;
    public Action onHit;
    public Action onDeath;
    //public UnityEvent onGenerateEvent;
    //public UnityEvent onDeathEvent;
    public UnityEvent onEnabled;
    [NonSerialized] public bool isInSpell = false;

    [Space(10)]
    [InfoBox("This container should have the GameObject's Mesh with its Collider. If it doesnt it'll automatically generate", EInfoBoxType.Warning)]
    public GameObject castedSpellPrefab;

    public enum SpellType
    {
        Projectile,
        Static,
        Summoner,
    }

    public enum SpellDirection
    {
        Forward,
        Up,
        Down,
        NoMove,
        Orbital,
        Heatseaking,
    }

    [Space(10)]
    public SpellType spellType;
    public float damage;
    public SpellDirection direction;
    public float lifeTime;
    public float speed;
    public float speedMultiplier;
    public float speedAcceleration;
    public float gravity;
    public float gravityMultiplier;
    [Tooltip("Determines how many bounces the spell can do before disappearing")]
    public int bounces;
    [Tooltip("Determines how many enemies the spell can penetrate through before disappearing")]
    public int penetrationCount;
    [Tooltip("Determines how many times the spell can damage a single enemy in a single hit")]
    public int pierceCount;
    public bool phasesThroughWalls;
    [ShowIf(nameof(spellType), SpellType.Summoner)]
    [AllowNesting]
    public int summoner;
    //HERE ADD ADDITIONAL DATAS TO HOLD IF NEEDED

    public Vector3 BasePosition;
    public Vector3 BaseDirection;

    //Here are ALL THE METHODS OF THIS CLASS
    public void CastSpell(Vector3 positon, Vector3 direction)
    {
        BasePosition = positon;
        BaseDirection = direction;
    }

    public void AddToSlot(ModifierSlot slot, Spell spell)
    {
        if (slot.slotMode == ModifierSlot.SlotMode.Spell)
        {
            slot.SetModifier(this);
            if (assignedSpell == null)
            {
                assignedSpell = spell;
            }
        }
        else
        {
            Debug.LogWarning("Already a spell in the slot");
        }
    }

    public void RemoveFromSlot(ModifierSlot slot)
    {
        if(slot.containedSpell == this)
        {
            slot.RemoveModifier(this);
            assignedSpell = null;
        }
        else
        {
            Debug.LogWarning("TF are you doing");
        }
    }
}

[Serializable]
public class ModifierSlot
{
    public enum SlotMode
    {
        Spell,
        Modifier
    }

    public enum SpellActivationMethod
    {
        OnDeath,
        OnWait1Second,
        OnCast,
        OnHit,
    }

    public SlotMode slotMode;

    [AllowNesting, ShowIf(nameof(slotMode), SlotMode.Modifier)]
    public ModifierType modType;

    [AllowNesting, ShowIf(nameof(slotMode), SlotMode.Spell)]
    public SpellActivationMethod spellActMethod;

    [NonSerialized] public Modifier containedModifier;

    [NonSerialized] public Spell containedSpell;

    [NonSerialized] public bool isEmpty = true;

    public void SetModifier(Modifier modifier)
    {
        containedModifier = modifier;
        //InventoryManager.Instance.Remove(modifier);
        isEmpty = false;
    }

    public void SetModifier(Spell spell)
    {
        containedSpell = spell;
        //InventoryManager.Instance.Add(spell);
        isEmpty = false;
    }

    public void RemoveModifier(Modifier modifier) 
    {
        containedModifier = null;
        //InventoryManager.Instance.Add(modifier);
        isEmpty = true;
    }

    public void RemoveModifier(Spell spell)
    {
        containedSpell = null;
        //InventoryManager.Instance.Add(spell);
        isEmpty = true;
    }
}
