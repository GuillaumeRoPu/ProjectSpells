using Core.Menu;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Gameplay,
        UI,
    }

    private GameState _state;

    private static GameManager gameManager;
    public static GameManager GM { get { return gameManager; } }

    public GameState State {  get { return _state; } }

    public void ChangeState(GameState state)
    {
        _state = state;
        if (state == GameState.Gameplay)
        {
            CloseAllMenus();
            OpenMenu(PageType.HUD);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (state == GameState.UI)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.Gameplay);
    }

    public void OpenMenu(PageType type)
    {
        UIManager.Instance.OpenMenu(type);
    }

    public void CloseMenu(PageType type)
    {
        UIManager.Instance.CloseMenu(type);
    }

    public void CloseAllMenus()
    {
        UIManager.Instance.CloseAllMenus();
    }
}
