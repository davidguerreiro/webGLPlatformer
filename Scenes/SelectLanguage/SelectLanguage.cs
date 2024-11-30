using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLanguage : MonoBehaviour
{
    [Header("Components")]
    public FadeElement cover;
    public Navegable languageMenu;

    [Header("Settings")]
    public string sceneToLoadAfter;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Display and set language menu as navegable.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisplayMenu()
    {
        cover.FadeOut();

        yield return new WaitForSeconds(.5f);
        languageMenu.SetNavegable();
    }

    /// <summary>
    /// Hide language menu and load next scene.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator HideMenu()
    {
        cover.FadeIn();

        yield return new WaitForSeconds(2f);
        LoadNextScene();
    }
    /// <summary>
    /// Check if language has already been selected
    /// when loading scene.
    /// </summary>
    private void CheckIfLanguageSelected()
    {
        string languageSelected = PlayerPrefs.GetString("language", "");
        if (languageSelected == "")
        {
            StartCoroutine(DisplayMenu());
        } else
        {
            LoadNextScene();
        }
    }

    public void SelectedOption()
    {
        string selected = languageMenu.GetSelectedLabel();

        PlayerPrefs.SetString("language", selected);
        PlayerPrefs.Save();

        StartCoroutine(HideMenu());
    }
    
    /// <summary>
    /// Load next scene.
    /// </summary>
    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoadAfter);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        CheckIfLanguageSelected();
    }
}
