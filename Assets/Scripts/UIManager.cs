using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool inUI { get; private set; }

    public void UIstateChange(bool state) 
    { 
        inUI = state;
    }

    public void Awake()
    {
        Instance = this;
    }
}
