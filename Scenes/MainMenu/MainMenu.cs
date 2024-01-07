using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    public MainMenuUI mainMenuUI;

    [Header("Settings")]
    public float toWaitBefore;
    public string sceneToLoad;

    private AudioComponent _audio;
    private bool _readyToStartGame;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(MainMenuEvent());
    }

    // Update is called once per frame
    void Update()
    {
        if (_readyToStartGame && Input.GetKeyDown("space"))
        {
            StartCoroutine(LoadNextScene());
        }
    }

    /// <summary>
    /// Main menu load screen event.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MainMenuEvent()
    {
        yield return new WaitForSeconds(toWaitBefore);
        mainMenuUI.DisplayUIElements();

        while (! mainMenuUI.displayed)
        {
            yield return new WaitForFixedUpdate();
        }

        _readyToStartGame = true;
        _audio.PlaySound(0);
    }

    /// <summary>
    /// Load next scene after pressing space bar.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNextScene()
    {
        _readyToStartGame = false;
        mainMenuUI.cover.FadeIn();
        _audio.SetLoop(false);
        _audio.PlaySound(1);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
