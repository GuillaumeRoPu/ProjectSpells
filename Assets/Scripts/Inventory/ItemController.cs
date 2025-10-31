using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private CATEGORY _category;
    [SerializeField] private ModifiersDataBase _modifiersDataBase;
    [SerializeField] private SpellsDataBase _spellsDataBase;
    private Modifier _modifierData;
    private Spell _spellData;
    [SerializeField] private int _index;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
