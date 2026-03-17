using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public bool inMenus { get; private set; }

    private HashSet<GameMenu> _allMenus;

    public void UIstateChange(bool state) 
    { 
        inMenus = state;
        if (!inMenus)
        {
            CloseMenus();
        }
    }

    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        inMenus = false;
    }

    public void OpenMenus()
    {

    }

    public void CloseMenus()
    {

    }

    public void AddMenu(GameMenu menu)
    {
        _allMenus.Add(menu);
    }
}
