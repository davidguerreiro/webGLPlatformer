using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public Navegable navegable;

    private GameManager _gameManager;

    /// <summary>
    /// Enable navegable for user input.
    /// </summary>
    /// <param name="gameManager">Gamemanager</param>
    public void EnableNavegable(GameManager gameManager)
    {
        navegable.SetNavegable();
        _gameManager = gameManager;
    }

    /// <summary>
    /// Logic when option selected in navegable.
    /// </summary>
    public void SelectOption()
    {
        string selected = navegable.GetSelectedLabel();
        navegable.UnSetNavegable();

        switch (selected)
        {
            case "tryagain":
                _gameManager.RetryGame();
                break;
            case "exitmainmenu":
                _gameManager.ExitToMainMenu();
                break;
            default:
                _gameManager.RetryGame();
                break;
        }
    }
}
