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
}
