using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ModifiersDataBase", menuName = "Items/Modifiers", order = 1)]
public class ModifiersDataBase : ItemDataBase
{
    public CATEGORY category;

    public new List<Modifier> datas = new();

    public Modifier GetData(int id, bool random = false)
    {
        List<Modifier> filteredList = datas;

        if (random)
            id = Random.Range(0, datas.Count);
        else
            id = Mathf.Clamp(id, 0, datas.Count - 1);

        // Return a clone of the EnemyData to ensure each item has its own instance
        return filteredList[id];
    }
}
