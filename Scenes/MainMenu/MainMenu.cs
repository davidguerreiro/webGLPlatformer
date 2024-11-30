using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    public MainMenuUI mainMenuUI;
    public GameObject playerShipWrapper;

    [Header("Scenes to load")]
    public string newGameScene;
    public string continueScene;
    public string optionsScene;

    [HideInInspector]
    public Rewired.Player rewiredPlayer;

    private AudioComponent _audio;
    private Animator _playerShipAnimator;
    private bool _readyForUserInput;
    private bool _readyToStartGame;
    private Coroutine _optionSelected;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(MainMenuEvent());
    }

    // Update is called once per frame
    void Update()
    {
        if (_readyForUserInput && (rewiredPlayer.GetButton("Jump") || rewiredPlayer.GetButton("Start")))
        {
            DisplayNavegable();
        }
    }

    /// <summary>
    /// Main menu load screen event.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MainMenuEvent()
    {
        yield return new WaitForSeconds(1f);

        mainMenuUI.RemoveCover();
        yield return new WaitForSeconds(1f);

        playerShipWrapper.SetActive(true);
        _playerShipAnimator = playerShipWrapper.GetComponent<Animator>();

        _audio.PlaySound(1);
        yield return new WaitForSeconds(1.5f);

        _audio.SetLoop(true);
        _audio.PlaySound(0);

        mainMenuUI.DisplayUIElements();

        while (mainMenuUI.displayed == false)
        {
            yield return new WaitForFixedUpdate();
        }

        _readyForUserInput = true;
    }

    /// <summary>
    /// Display navegable menu on
    /// user input.
    /// </summary>
    public void DisplayNavegable()
    {
        _readyForUserInput = false;
        mainMenuUI.DisplayAndEnableMainMenuNavegable();
    }

    /// <summary>
    /// Navegable option selected
    /// </summary>
    public void NavegableOptionSelected()
    {
        if (_optionSelected == null)
        {
            _optionSelected = StartCoroutine(NavegableOptionSelectedCoroutine());
        }
    }

    /// <summary>
    /// Navegable option selected logic coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator NavegableOptionSelectedCoroutine()
    {
        string optionSelected = mainMenuUI.GetSelectedNavegableItem();
        _audio.SetLoop(false);
        _audio.PlaySound(2);

        if (optionSelected == "newgame")
        {
            mainMenuUI.HideNavegableMenu();
            mainMenuUI.HideTitleAndLogo();
            yield return new WaitForSeconds(1.5f);

            _audio.PlaySound(1);
            _playerShipAnimator.SetBool("MoveOut", true);

            yield return new WaitForSeconds(2.5f);
        }

        mainMenuUI.ShowCover();
        yield return new WaitForSeconds(1.5f);

        switch (optionSelected)
        {
            case "newgame":
                LoadNewGame();
                break;
            case "continue":
                LoadContinueGame();
                break;
            case "options":
                LoadOptionsScreen();
                break;
            case "exit":
                ExitGame();
                break;
            default:
                LoadNewGame();
                break;
        }
    }

    /// <summary>
    /// Load new game logic.
    /// </summary>
    private void LoadNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    /// <summary>
    /// Continue game logic.
    /// </summary>
    private void LoadContinueGame()
    {
        SceneManager.LoadScene(continueScene);
    }

    /// <summary>
    /// Options screen logic.
    /// </summary>
    private void LoadOptionsScreen()
    {
        SceneManager.LoadScene(optionsScene);
    }

    /// <summary>
    /// Exit game option logic.
    /// </summary>
    private void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        rewiredPlayer = ReInput.players.GetPlayer(0);
    }
}
