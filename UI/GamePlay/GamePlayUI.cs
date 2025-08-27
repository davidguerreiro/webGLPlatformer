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
    public KeyGameplayUI blueKeyUI;
    public HealthUI healthUI;
    public GameOverUI gameOver;
    public DialogueBox dialogueBox;
    public DialogueBox dialogueBoxSecondary;
    public HelpBox helpbox;
    public PauseUI pauseUI;

    [HideInInspector]
    public Coroutine initLevel;

    private GameManager _gameManager;

    /// <summary>
    /// Init gameplay UI method.
    /// </summary>
    /// <param name="gameManager"></param>
    public void Init(GameManager gameManager)
    {
        // init level intro.
        levelIntro.Init(gameManager.levelData);

        // init coins UI.
        if (gameManager.isBossLevel || gameManager.removeCoinsUI)
        {
            coinsUI.gameObject.SetActive(false);
        } else
        {
            coinsUI.Init(gameManager);
        }

        // init key UI.
        if (gameManager.hasBlueKey)
        {
            keyUI.gameObject.SetActive(false);
            blueKeyUI.gameObject.SetActive(true);

            blueKeyUI.Init(gameManager.player);
        } else
        {
            keyUI.Init(gameManager.player);
        }

        // init health UI.
        healthUI.Init(gameManager.player);

        if (gameManager.isBossLevel || gameManager.hasCinematic)
        {
            dialogueBox.Init();
            dialogueBoxSecondary.Init();
        }

        _gameManager = gameManager;
    }

    /// <summary>
    /// Init gameplay UI for general cinematics.
    /// </summary>
    public void Init()
    {
        dialogueBox.Init();
        dialogueBoxSecondary.Init();
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
        gameOver.gameObject.SetActive(true);
        gameOver.EnableNavegable(_gameManager);
    }

    /// <summary>
    /// Hide game over modal.
    /// </summary>
    public void HideGameOver()
    {
        gameOver.gameObject.SetActive(false);
    }

    /// <summary>
    /// Disables UI elements.
    /// </summary>
    public void DisableMainGamePlayElements()
    {
        coinsUI.gameObject.SetActive(false);
        keyUI.gameObject.SetActive(false);
        blueKeyUI.gameObject.SetActive(false);
        healthUI.gameObject.SetActive(false);
    }
}
