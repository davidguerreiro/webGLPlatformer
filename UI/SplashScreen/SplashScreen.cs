using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public FadeElement logo;

    [Header("Settings")]
    public string nextSceneName;
    public float secondsBeforeDisplaying;
    public float secondsDisplayed;
    public float secondsAfterDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayCompanyLogo());
    }

    /// <summary>
    /// Display company logo in splash
    /// screen.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator DisplayCompanyLogo()
    {
        yield return new WaitForSeconds(secondsBeforeDisplaying);

        logo.FadeIn();
        yield return new WaitForSeconds(secondsDisplayed);

        logo.FadeOut();
        yield return new WaitForSeconds(secondsAfterDisplayed);

        SceneManager.LoadScene(nextSceneName);
    }
}
