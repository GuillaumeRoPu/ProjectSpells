using Core.Menu;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public bool inMenus { get; private set; }


    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        inMenus = false;
    }

    public void OpenMenu(PageType type)
    {
        PageController.Instance.TurnPageOn(type);
    }

    public void CloseAllMenus()
    {
        PageController.Instance.TurnAllPagesOff();
    }

    public void CloseMenu(PageType type)
    {
        PageController.Instance.TurnPageOff(type);
    }
}
