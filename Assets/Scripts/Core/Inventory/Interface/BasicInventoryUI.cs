
using UnityEngine;
using UnityEngine.UI;

public class BasicInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject itemUIPrefab;

    private void OnEnable()
    {
        BasicInventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        BasicInventoryManager.Instance.OnInventoryChanged -= Refresh;
    }

    private void Refresh()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var item in BasicInventoryManager.Instance.Items)
        {
            GameObject uiItem = Instantiate(itemUIPrefab, contentParent);
            uiItem.transform.Find("ItemName").GetComponent<Text>().text = item.itemName;
            uiItem.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;
        }
    }
}
