using UnityEngine;

public class OpenInventoryScript : MonoBehaviour
{
    public void OpenInventory()
    {
        GameManager.GM.OpenMenu(Core.Menu.PageType.Inventory);
    }
}
