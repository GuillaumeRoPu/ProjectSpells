using UnityEngine;
using System;

public class Item
{
    public string label;
    public int id;
    public string itemName;
    [NonSerialized] public string displayName;
    public string description;
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

[Serializable]
public class Spell : Item
{

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
    }

    public ModifierType _modType;
}
