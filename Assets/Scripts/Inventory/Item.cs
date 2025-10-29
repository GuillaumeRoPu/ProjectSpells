using UnityEngine;
using System;

[Serializable]

public class Item
{
    public string label;
    public int id;
    public string itemName;
    [NonSerialized] public string displayName;
    public int value;
    public Sprite icon;
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

public class Spell : Item
{

}

public class Modifiers : Item
{
    public enum ModifierType
    {
        Random,
        Size,
        Damage,
        Route,
        Speed,
    }

    public ModifierType _modType;
}
