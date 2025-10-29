using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Datas/Item", order = 3)]
public class ItemDataBase : ScriptableObject
{
    public enum CATEGORY
    {
        RANDOM,
        Spells,
        Modifiers,
    }

    public CATEGORY category;

    public List<Item> datas = new();

    public Item GetData(int id, bool random = false)
    {
        if (random)
            id = Random.Range(0, datas.Count);
        else
            id = Mathf.Clamp(id, 0, datas.Count - 1);

        // Return a clone of the EnemyData to ensure each item has its own instance
        return datas[id];
    }
}

