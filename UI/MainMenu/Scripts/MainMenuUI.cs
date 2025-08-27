using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Components")]
    public FadeElement cover;
    public FadeElement gameTitle;
    public FadeElement companyLogo;
    public GameObject pressStartText;
    public GameObject gameVersion;
    public GameObject mainMenuNavegable;

    [HideInInspector]
    public bool displayed;

    private AudioComponent _audio;
    private Navegable _navegable;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void DisplayUIElements()
    {
        StartCoroutine(DisplayUIElementsRoutine());
    }

    /// <summary>
    /// Display main menu UI anim.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator DisplayUIElementsRoutine()
    {
        gameTitle.FadeIn(.1f);
        yield return new WaitForSeconds(3f);

        companyLogo.FadeIn();
        yield return new WaitForSeconds(1f);

        pressStartText.SetActive(true);
        gameVersion.SetActive(true);
        displayed = true;
    }

    /// <summary>
    /// Display and make main menu navegable.
    /// </summary>
    public void DisplayAndEnableMainMenuNavegable()
    {
        _audio.PlaySound(0);
        DisablePressStartText();
        mainMenuNavegable.SetActive(true);

        _navegable = mainMenuNavegable.GetComponent<Navegable>();
        _navegable.SetNavegable();
    }

    /// <summary>
    /// Get selected option in navegable 
    /// main menu UI.
    /// </summary>
    /// <returns>string</returns>
    public string GetSelectedNavegableItem()
    {
        if (_navegable == null)
        {
            _navegable = GetComponent<Navegable>();
        }

        return _navegable.GetSelectedLabel();
    }

    /// <summary>
    /// Disable press start button text
    /// in main menu UI.
    /// </summary>
    public void DisablePressStartText()
    {
        pressStartText.SetActive(false);
    }

    /// <summary>
    /// Disable and hide main menu navegable.
    /// </summary>
    public void HideNavegableMenu()
    {
        mainMenuNavegable.SetActive(false);
    }

    /// <summary>
    /// Fade out logo and title from
    /// main menu UI.
    /// </summary>
    public void HideTitleAndLogo()
    {
        gameTitle.FadeOut();
        companyLogo.FadeOut();
    }

    /// <summary>
    /// Display cover.
    /// </summary>
    public void ShowCover()
    {
        cover.FadeIn();
    }

    /// <summary>
    /// Hide cover.
    /// </summary>
    public void RemoveCover()
    {
        cover.FadeOut();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
