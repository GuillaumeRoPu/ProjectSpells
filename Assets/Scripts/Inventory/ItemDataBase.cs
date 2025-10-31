using System.Collections.Generic;
using UnityEngine;


public enum CATEGORY
{
    Spells,
    Modifiers,
}

public class ItemDataBase : ScriptableObject
{
    public List<Item> datas = new();

    public virtual Item GetItemData(int id, bool random = false)
    {
        List<Item> filteredList = datas;

        if (random)
            id = Random.Range(0, datas.Count);
        else
            id = Mathf.Clamp(id, 0, datas.Count - 1);

        // Return a clone of the EnemyData to ensure each item has its own instance
        return filteredList[id];
    }
}


