using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Level Configuration")]
    public LevelData levelData;
    public bool isBossLevel;
    public bool hasCinematic;
    public bool hasBlueKey;

    [Header("Game Progresion")]
    public LocalVars gameProgresion;

    [Header("GamePlay References")]
    public Player player;
    public Transform playerSpawn;
    public GameObject key;

    [Header("UIs")]
    public GamePlayUI gamePlayUI;

    [Header("Other managers and components")]
    public CinematicManager cinematicManager;

    [Header("Level Coins")]
    public int levelCoins;

    [Header("Events")]
    public UnityEvent onPlayerSpawn;
    public UnityEvent beforeLoadLevel;
    public UnityEvent afterLoadLevel;

    [HideInInspector]
    public bool inGamePlay;

    [HideInInspector]
    public bool inGameOver;

    [HideInInspector]
    public bool isPaused;

    private AudioComponent _audio;
    private SaveGame _saveGame;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(InitLevel());
    }

    private void Update()
    {
        if (inGamePlay)
        {
            // check player coins requisite to show key in game scene.
            if (!key.activeSelf)
            {
                CheckForCoins();
            }

            // check for pause game.
            if (! inGameOver && player.playerController.rewiredPlayer.GetButtonDown("Start"))
            {
                if (isPaused)
                {
                    ResumeGameAfterPause();
                } else
                {
                    PauseGame();
                }
            }

            // check for exiting game.
            if (isPaused && player.playerController.rewiredPlayer.GetButtonDown("Exit"))
            {
                ExitToMainMenu();
            }
            
        }
    }

    /// <summary>
    /// Init level coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator InitLevel()
    {
        beforeLoadLevel?.Invoke();

        // update game progression if needed.
        if (levelData.saveDataAtInitLevel)
        {
            UpdateGameProgression();
        }

        // init UIs.
        gamePlayUI.Init(this);
        yield return new WaitForSeconds(1f);

        // init cinematic manager.
        if (isBossLevel || hasCinematic)
        {
            cinematicManager.Init(this);
        }

        // start gameplay UI.
        gamePlayUI.InitGamePlay();
        PlayLevelIntroMusic();

        while (gamePlayUI.initLevel != null)
        {
            yield return new WaitForFixedUpdate();
        }

        // init player.
        player.InitPlayer();
        PlayerSpawn();

        if (! isBossLevel)
        {
            PlayLevelMainTheme();
        }

        yield return new WaitForSeconds(.1f);

        // remove cover and start level gameplay.
        gamePlayUI.cover.FadeOut();

        inGamePlay = true;

        afterLoadLevel?.Invoke();
    }

    /// <summary>
    /// Spawn player and allow movement.
    /// </summary>
    public void PlayerSpawn()
    {
        player.transform.position = playerSpawn.position;
        player.playerController.AllowControl();

        if (isBossLevel)
        {
            player.playerController.RestrictPlayerInput();
        }

        onPlayerSpawn?.Invoke();
    }

    /// <summary>
    /// Check if player has collect all coins.
    /// </summary>
    private void CheckForCoins()
    {
        if (player.GetCoins() == levelCoins)
        {
            DisplayKey();
        }
    }

    /// <summary>
    /// Reveal key in game level.
    /// </summary>
    public void DisplayKey()
    {
        key.SetActive(true);
    }

    /// <summary>
    /// Pause game.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePlayUI.pauseUI.Display();
        player.playerController.RestrictPlayerInput();
        isPaused = true;
    }

    /// <summary>
    /// Resume game after paused.
    /// </summary>
    public void ResumeGameAfterPause()
    {
        Time.timeScale = 1f;
        gamePlayUI.pauseUI.Hide();
        player.playerController.AllowPlayerInput();
        isPaused = false;
    }

    /// <summary>
    /// Level finished logic coroutine.
    /// </summary>
    public void EndLevel()
    {
        StartCoroutine(EndLevelCoroutine());
    }

    /// <summary>
    /// Level finished logic coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator EndLevelCoroutine()
    {
        inGamePlay = false;

        player.playerController.EnterDoor();
        PlayCompletedLevelMusic();

        yield return new WaitForSeconds(5f);
        gamePlayUI.cover.FadeIn();

        SceneManager.LoadScene(levelData.nextLevelName);
    }

    /// <summary>
    /// Calculate how manu coins are left to be collected
    /// by the player.
    /// </summary>
    /// <returns>int</returns>
    public int GetCoinsLeftInLevel()
    {
        return levelCoins - player.coins;
    }

    /// <summary>
    /// Player damaged.
    /// </summary>
    public void PlayerDamaged()
    {
        StartCoroutine(PlayerDamagedRoutine());
    }

    /// <summary>
    /// Player damaged coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayerDamagedRoutine()
    {
        if (player.GetHealth() >= 0)
        {
            yield return new WaitForSeconds(1.5f);
            PlayerSpawn();
        }
        else
        {
            inGameOver = true;
            gamePlayUI.DisplayGameOver();
        }
    }

    /// <summary>
    /// Retry game after game over.
    /// </summary>
    public void RetryGame()
    {
        StartCoroutine(RetryGameRoutine());
    }

    /// <summary>
    /// Retry game coroutine.
    /// </summary>
    private IEnumerator RetryGameRoutine()
    {
        inGamePlay = false;
        gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Play level intro music.
    /// </summary>
    public void PlayLevelIntroMusic()
    {
        _audio.SetLoop(false);
        _audio.PlaySound(0);
    }

    /// <summary>
    /// Play level main theme music.
    /// </summary>
    public void PlayLevelMainTheme()
    {
        _audio.SetLoop(true);
        _audio.PlaySound(3);
    }

    /// <summary>
    /// Play completed level music.
    /// </summary>
    public void PlayCompletedLevelMusic()
    {
        _audio.SetLoop(false);
        _audio.PlaySound(1);
    }

    /// <summary>
    /// Play game over music.
    /// </summary>
    public void PlayGameOverMusic()
    {
        _audio.SetLoop(false);
        _audio.PlaySound(2);
    }

    /// <summary>
    /// Stops level music.
    /// </summary>
    public void StopLevelMusic()
    {
        _audio.SetLoop(false);
        _audio.StopAudio();
    }

    /// <summary>
    /// Update game progresion, usually called
    /// first time player enters into new world.
    /// </summary>
    private void UpdateGameProgression()
    {
        gameProgresion.SetVar(levelData.saveVariableName, true);
        _saveGame.WriteDataInJson(gameProgresion);
    }

    /// <summary>
    /// Exit current gameplay and load main
    /// menu scene.
    /// </summary>
    public void ExitToMainMenu()
    {
        StopLevelMusic();
        ResumeGameAfterPause();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Destroy(this);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _audio.UpdateClip(3, levelData.levelMusic);
        inGameOver = false;

        if (levelData.saveDataAtInitLevel)
        {
            _saveGame = GetComponent<SaveGame>();
            _saveGame.Init();
        }
    }
}
