using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Item
{
    public string label;
    public int id;
    public string itemName;
    [NonSerialized] public string displayName;
    public string description;
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

[Serializable]
public class Spell : Item
{
    [Header("Spells Specifications"), AllowNesting]
    public List<(Modifier modifier, Modifier.ModifierType modifierType)> modifiersList = new();

    [Header("Used Spell Parameters")]
    public Action onWait1Second;
    public Action onHit;
    public Action onDeath;
    public UnityEvent onGenerateEvent;
    public UnityEvent onDeathEvent;
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
    //Work in progress

    public void UsedSpell()
    {

    }
}

[Serializable]
public class Modifier : Item
{
    public enum ModifierType
    {
        Random,
        Size,
        Damage,
        Route,
        Speed,
        Direction,
    }

    public ModifierType _modType;


}
