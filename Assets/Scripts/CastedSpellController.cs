using UnityEngine;

public class CastedSpellController : MonoBehaviour
{
    private Spell _storedSpell;
    private GameObject _child;




    public void InitiateSpell(Spell spell)
    {
        _storedSpell = spell;
        _child = Instantiate(spell.prefab, this.transform);
    }
}
