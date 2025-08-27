using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ShipGameManager : MonoBehaviour
{
    [Header("Level configuration")]
    public bool hasCinematic;

    [Header("GamePlay References")]
    public PlayerShip player;
    public Transform playerSpawn;

    [Header("UIs")]
    public ShipGamePlayUI gamePlayUI;

    [Header("Other managers and components")]
    public CinematicManager cinematicManager;

    [Header("Configuration")]
    public float secondsBeforePlayerRespawn;

    [Header("Events")]
    public UnityEvent onPlayerSpawn;
    public UnityEvent onPlayerRespawn;
    public UnityEvent beforeLoadLevel;
    public UnityEvent afterLoadLevel;

    [HideInInspector]
    public bool inGamePlay;

    [HideInInspector]
    public bool inGameOver;

    [HideInInspector]
    public bool isPaused;

    private AudioComponent audio;
    private SaveGame saveGame;

    private void Start()
    {
        Init();
        StartCoroutine(InitLevel());
    }

    private void Update()
    {
        // check for game pause.
        if ( ! inGameOver && player.playerController.rewiredPlayer.GetButtonDown("Start"))
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

    /// <summary>
    /// Init spaceship level.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator InitLevel()
    {
        beforeLoadLevel?.Invoke();

        // init UIs.
        gamePlayUI.Init(this);

        // init cinematic manager.
        if (hasCinematic)
        {
            cinematicManager.Init(this);
        }

        // init player.
        player.InitPlayer(this);
        PlayerSpawn(false);

        yield return new WaitForSeconds(.1f);

        // remove cover.
        gamePlayUI.cover.FadeOut();
        PlayLevelMainTheme();

        // starts gameplay.
        inGamePlay = true;

        afterLoadLevel?.Invoke();
        yield break;
    }

    /// <summary>
    /// Spawn player in current level.
    /// </summary>
    public void PlayerSpawn(bool invincible = false)
    {
        player.transform.position = playerSpawn.position;
        player.RestoreFullHealth();
        player.playerController.AllowControl();

        if (invincible)
        {
            player.Invincible();
        }

        onPlayerSpawn?.Invoke();
    }

    /// <summary>
    /// Spawn player in current level.
    /// </summary>
    public void PlayerSpawn()
    {
        player.transform.position = playerSpawn.position;
        player.playerController.AllowControl();

        onPlayerSpawn?.Invoke();
    }

    /// <summary>
    /// Player respawn after death.
    /// </summary>
    public void PlayerRespawn()
    {
        if (player.GetLife() > 0)
        {
            StartCoroutine(PlayerRespawnCoroutine());
        } else
        {
            inGameOver = true;
            gamePlayUI.DisplayGameOver();
        }
    }

    /// <summary>
    /// Respawn coroutine
    /// </summary>
    /// <returns>IEnumrator</returns>
    private IEnumerator PlayerRespawnCoroutine()
    {
        yield return new WaitForSeconds(secondsBeforePlayerRespawn);
        PlayerSpawn(true);

        onPlayerRespawn?.Invoke();
    }

    /// <summary>
    /// Play current level main theme.
    /// </summary>
    public void PlayLevelMainTheme()
    {
        audio.SetLoop(true);
        audio.PlaySound(0);
    }

    /// <summary>
    /// Play game over music.
    /// </summary>
    public void PlayGameOverMusic()
    {
        audio.SetLoop(false);
        audio.PlaySound(1);
    }

    /// <summary>
    /// Play level boss battle theme.
    /// </summary>
    public void PlayBossBattleMusic()
    {
        audio.SetLoop(true);
        audio.PlaySound(2);
    }

    /// <summary>
    /// Retry game after game over.
    /// </summary>
    public void RetryGame()
    {
        StartCoroutine(RetryGameRoutine());
    }

    /// <summary>
    /// Stops level music.
    /// </summary>
    public void StopLevelMusic()
    {
        audio.SetLoop(false);
        audio.StopAudio();
    }

    /// <summary>
    /// Pause game.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePlayUI.pauseUI.Display();

        if (inGamePlay)
        {
            player.playerController.RestrictControl();
        }

        isPaused = true;
    }

    /// <summary>
    /// Resume game after paused.
    /// </summary>
    public void ResumeGameAfterPause()
    {
        Time.timeScale = 1f;
        gamePlayUI.pauseUI.Hide();

        if (inGamePlay)
        {
            player.playerController.AllowControl();
        }

        isPaused = false;
    }

    /// <summary>
    /// Retry game coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RetryGameRoutine()
    {
        inGamePlay = false;
        gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        audio = GetComponent<AudioComponent>();
    }
}
