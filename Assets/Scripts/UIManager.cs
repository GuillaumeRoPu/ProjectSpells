using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool inMenus { get; private set; }

    public void UIstateChange(bool state) 
    { 
        inMenus = state;
    }

    public void Awake()
    {
        Instance = this;
        inMenus = false;
    }
}
