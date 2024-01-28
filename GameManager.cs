using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Level Configuration")]
    public LevelData levelData;

    [Header("GamePlay References")]
    public Player player;
    public Transform playerSpawn;
    public GameObject key;

    [Header("UIs")]
    public GamePlayUI gamePlayUI;

    [Header("Level Coins")]
    public int levelCoins;

    [Header("Events")]
    public UnityEvent OnPlayerSpawn;

    [HideInInspector]
    public bool inGamePlay;

    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(InitLevel());
    }

    private void Update()
    {
        // check player coins requisite to show key in game scene.
        if (inGamePlay && !key.activeSelf)
        {
            CheckForCoins();
        }
    }

    /// <summary>
    /// Init level coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator InitLevel()
    {
        // init UIs.
        gamePlayUI.Init(this);
        yield return new WaitForSeconds(1f);

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
        PlayLevelMainTheme();

        yield return new WaitForSeconds(.1f);

        // remove cover and start level gameplay.
        gamePlayUI.cover.FadeOut();

        inGamePlay = true;
    }

    /// <summary>
    /// Spawn player and allow movement.
    /// </summary>
    public void PlayerSpawn()
    {
        player.transform.position = playerSpawn.position;
        player.playerController.AllowControl();

        OnPlayerSpawn?.Invoke();
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
        if (player.GetHealth() > 0)
        {
            yield return new WaitForSeconds(1.5f);
            PlayerSpawn();
        }
        else
        {
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

        gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Level1-1");
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
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _audio.UpdateClip(3, levelData.levelMusic);
    }
}
