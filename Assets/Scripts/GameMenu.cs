using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    public GameObject Menu { get { return _menu; } }

    private void Awake()
    {
        UIManager.Instance.AddMenu(this);
    }

    public void OpenMenu()
    {
        
    }

    public void CloseMenu()
    {
        
    }
}
