using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemDataBase _itemdataBase;
    private Item _itemData;
    [SerializeField] private int _index;

    void Start()
    {
        _itemData = _itemdataBase.GetData(_index);
        Debug.Log(_itemData.itemName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
