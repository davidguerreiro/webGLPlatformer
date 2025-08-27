using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public Navegable navegable;
    public bool inSpaceLevel;

    private GameManager _gameManager;
    private ShipGameManager shipGameManager;

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
    /// Enable navegable for user input in spaceship
    /// type levels.
    /// </summary>
    /// <param name="gameManager">ShipGameManager</param>
    public void EnableNavegable(ShipGameManager gameManager)
    {
        navegable.SetNavegable();
        shipGameManager = gameManager;
    }

    /// <summary>
    /// Logic when option selected in navegable.
    /// </summary>
    public void SelectOption()
    {
        if (inSpaceLevel == false)
        {
            SelectNormalOption();
        } else
        {
            SelectShipOption();
        }
    }

    /// <summary>
    /// Select gameover menu option in platform
    /// standard levels.
    /// </summary>
    private void SelectNormalOption()
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

    /// <summary>
    /// Select gameover menu option in spaceship
    /// levels.
    /// </summary>
    private void SelectShipOption()
    {
        string selected = navegable.GetSelectedLabel();
        navegable.UnSetNavegable();

        switch (selected)
        {
            case "tryagain":
                shipGameManager.RetryGame();
                break;
            case "exitmainmenu":
                shipGameManager.ExitToMainMenu();
                break;
            default:
                shipGameManager.RetryGame();
                break;
        }
    }
}
