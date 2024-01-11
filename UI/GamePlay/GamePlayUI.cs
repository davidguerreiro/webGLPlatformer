using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    [Header("Components")]
    public FadeElement cover;
    public LevelIntro levelIntro;
    public CoinsGameplayUI coinsUI;
    public KeyGameplayUI keyUI;
    public HealthUI healthUI;
    public GameObject gameOver;

    [HideInInspector]
    public Coroutine initLevel;

    private GameManager _gameManager;

    /// <summary>
    /// Init gameplay UI method.
    /// </summary>
    public void Init(GameManager gameManager)
    {
        // init level intro.
        levelIntro.Init(gameManager.levelData);

        // init coins UI.
        // coinsUI.Init(gameManager.player);

        // init key UI.
        keyUI.Init(gameManager.player);

        // init health UI.
        healthUI.Init(gameManager.player);

        _gameManager = gameManager;
    }

    /// <summary>
    /// Init gameplay UI.
    /// </summary>
    public void InitGamePlay()
    {
        initLevel = StartCoroutine(InitGamePlayRoutine());
    }

    /// <summary>
    /// Init gameplay UI routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator InitGamePlayRoutine()
    {
        levelIntro.ShowLevelData();

        while (levelIntro.displayed)
        {
            yield return new WaitForFixedUpdate();
        }

        initLevel = null;
    }

    /// <summary>
    /// Display game over modal.
    /// </summary>
    public void DisplayGameOver()
    {
        _gameManager.PlayGameOverMusic();
        gameOver.SetActive(true);
    }

    /// <summary>
    /// Hide game over modal.
    /// </summary>
    public void HideGameOver()
    {
        gameOver.SetActive(false);
    }
}
