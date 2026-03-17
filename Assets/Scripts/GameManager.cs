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
    }

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);

        _state = GameState.Gameplay;
    }

    public void OpenMenus()
    {
        UIManager.Instance.OpenMenus();
    }
}
