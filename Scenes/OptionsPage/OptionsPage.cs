using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rewired;


public class OptionsPage : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoadOnExit;

    [Header("Components")]
    public FadeElement cover;
    public Navegable languageNav;
    public Navegable vibrationNav;

    [HideInInspector]
    public Rewired.Player rewiredPlayer;

    private AudioComponent _audio;
    private bool _listeningForInput;

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
            if (rewiredPlayer.GetButtonDown("Start"))
            {
                StartCoroutine(CloseScreen());
            }
        }
    }

    /// <summary>
    /// Make language menu navegable.
    /// </summary>
    public void MakeLanguageNavegable()
    {
        vibrationNav.UnSetNavegable();
        languageNav.SetNavegable();
    }

    /// <summary>
    /// Make vibration menu navegable.
    /// </summary>
    public void MakeVibrationNavegable()
    {
        languageNav.UnSetNavegable();
        vibrationNav.SetNavegable();
    }

    /// <summary>
    /// Init screen coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator InitScreen()
    {
        yield return new WaitForSeconds(.5f);
        languageNav.HideSelectedCursor();
        vibrationNav.HideSelectedCursor();
        cover.FadeOut();
        _audio.PlaySound(0);


        languageNav.SetNavegable();
        _listeningForInput = true;
    }

    /// <summary>
    /// Close screen and back to main menu coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator CloseScreen()
    {
        _listeningForInput = false;

        _audio.PlaySound(1);
        cover.FadeIn();

        languageNav.UnSetNavegable();
        vibrationNav.UnSetNavegable();

        yield return new WaitForSeconds(1f);

        SavePlayerPrefs();

        SceneManager.LoadScene(sceneToLoadOnExit);
    }

    /// <summary>
    /// Save player prefs from option
    /// selected settings.
    /// </summary>
    private void SavePlayerPrefs()
    {
        string languageSelected = languageNav.GetSettingsItemSelectedLabel();
        string vibrationSelectd = vibrationNav.GetSettingsItemSelectedLabel();

        PlayerPrefs.SetString("language", languageSelected);
        PlayerPrefs.SetString("vibration", vibrationSelectd);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set current language nav option.
    /// </summary>
    private void InitSelectedLanguageItem()
    {
        string currentLang = PlayerPrefs.GetString("language", "english");
        
        foreach (NavegableItem item in languageNav.items)
        {
            SettingsItem settingsItem = item.GetComponent<SettingsItem>();

            settingsItem.UnSetSelected();

            if (item.label == currentLang)
            {
                settingsItem.SetSelected(false);
            }
        }
    }

    /// <summary>
    /// Set current vibration option.
    /// </summary>
    private void InitSelectedVibrationItem()
    {
        string currentVibration = PlayerPrefs.GetString("vibration", "yes");
        
        foreach (NavegableItem item in vibrationNav.items)
        {
            SettingsItem settingsItem = item.GetComponent<SettingsItem>();

            settingsItem.UnSetSelected();

            if (item.label == currentVibration)
            {
                settingsItem.SetSelected(false);
            }
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        rewiredPlayer = ReInput.players.GetPlayer(0);

        InitSelectedLanguageItem();
        InitSelectedVibrationItem();
    }
}
