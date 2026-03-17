using UnityEngine;
using NaughtyAttributes;

public class ItemController : MonoBehaviour
{
    [SerializeField] private CATEGORY _category;
    public CATEGORY CATEGORY { get { return _category; } }
    [SerializeField] private ModifiersDataBase _modifiersDataBase;
    [SerializeField] private SpellsDataBase _spellsDataBase;
    private Modifier _modifierData;
    public Modifier ModifierData { get { return _modifierData; } }
    private Spell _spellData;
    public Spell SpellData { get { return _spellData; } }
    [SerializeField, HideIf(nameof(random))] private int _index;
    [SerializeField] private bool random = false;


    void Start()
    {
        switch (_category)
        {
            case CATEGORY.Spells:
                _spellData = _spellsDataBase.GetData(_index, random);
                Debug.Log(_spellData.itemName);
                break;
            case CATEGORY.Modifiers:
                _modifierData = _modifiersDataBase.GetData(_index, random);
                Debug.Log(_modifierData.itemName);
                break;
        }
    }

    public void SetSpell(Spell spell)
    {
        _spellData = spell;
        _category = CATEGORY.Spells;
    }

    public void SetModifier(Modifier modifier)
    {
        _modifierData = modifier;
        _category = CATEGORY.Modifiers;
    }
}
