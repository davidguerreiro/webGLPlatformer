using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class SelectWorld : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoadOnExit;

    [Header("Data")]
    public LocalVars gameVars;

    [Header("Components")]
    public FadeElement cover;
    public Navegable worldSelectorMenu;
    public SelectWorldItem[] selectWorldItems;


    [HideInInspector]
    public Rewired.Player rewiredPlayer;

    private AudioComponent _audio;
    private SaveGame saveGame;
    private bool _listeningForInput;
    private Coroutine loadingWorld;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(InitScreen());
    }

    // Update is called once per frame
    void Update()
    {
        if (_listeningForInput)
        {
            if (rewiredPlayer.GetButtonDown("Cancel"))
            {
                StartCoroutine(CloseScreen());
            }
        }
    }

    /// <summary>
    /// Init screen coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator InitScreen()
    {
        yield return new WaitForSeconds(.5f);

        gameVars = saveGame.GetDataFromJson(gameVars);
        yield return new WaitForSeconds(.1f);
        UnLockWorldSelectables();

        _audio.PlaySound(0);
        cover.FadeOut();

        worldSelectorMenu.SetNavegable();
        _listeningForInput = true;
    }

    /// <summary>
    /// Unlock game menu items if world has been unlocked
    /// by player.
    /// </summary>
    public void UnLockWorldSelectables()
    {
        foreach (SelectWorldItem item in selectWorldItems)
        {
            item.Init(this);

            bool unlock = gameVars.GetVar(item.gameVarToUnlock);

            if (unlock)
            {
                item.UnLockItem();
            }
        }
    }

    /// <summary>
    /// Load world based on selected option.
    /// </summary>
    /// <param name="sceneToLoad">string</param>
    public void LoadWorld(string sceneToLoad)
    {
        if (loadingWorld == null)
        {
            StartCoroutine(LoadWorldRoutine(sceneToLoad));
        }
    }

    /// <summary>
    /// Scene to load routine.
    /// </summary>
    /// <param name="sceneToLoad">string</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator LoadWorldRoutine(string sceneToLoad)
    {
        _listeningForInput = false;

        cover.FadeIn();

        worldSelectorMenu.UnSetNavegable();

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }


    /// <summary>
    /// Close screen and return to main menu.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator CloseScreen()
    {
        _listeningForInput = false;

        _audio.PlaySound(2);
        cover.FadeIn();

        worldSelectorMenu.UnSetNavegable();

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoadOnExit);
    }

    

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        saveGame = GetComponent<SaveGame>();
        rewiredPlayer = ReInput.players.GetPlayer(0);

        saveGame.Init();
    }
}
