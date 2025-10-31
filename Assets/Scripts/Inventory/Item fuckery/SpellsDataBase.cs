using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpellsDataBase", menuName = "Items/Spells", order = 0)]
public class SpellsDataBase : ItemDataBase
{
    public CATEGORY category;

    public new List<Spell> datas = new();

    public Spell GetData(int id, bool random = false)
    {
        List<Spell> filteredList = datas;

        if (random)
            id = Random.Range(0, datas.Count);
        else
            id = Mathf.Clamp(id, 0, datas.Count - 1);

        // Return a clone of the EnemyData to ensure each item has its own instance
        return filteredList[id];
    }
}
