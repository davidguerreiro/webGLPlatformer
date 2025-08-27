using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpBox : MonoBehaviour
{
    [Header("Components")]
    public TextComponent textComponent;
    public FadeElement fadeBack;
    public FadeElement fadeText;

    [Header("Settings")]
    public float timeDisplayed;

    [HideInInspector]
    public bool displayed;

    public Coroutine displayCoroutine;

    /// <summary>
    /// Display help box.
    /// </summary>
    /// <param name="textToShow">string</param>
    public void Display(string textToShow)
    {
        if (displayed == false && displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplayRoutine(textToShow));
        }
    }

    /// <summary>
    /// Display help box.
    /// </summary>
    /// <param name="textToShow">string</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator DisplayRoutine(string textToShow)
    {
        textComponent.UpdateContent(textToShow);

        fadeBack.FadeIn();
        fadeText.FadeIn();

        displayed = true;

        yield return new WaitForSeconds(timeDisplayed);

        Hide();
        yield return new WaitForSeconds(1f);

        displayed = false;
        displayCoroutine = null;
    }

    /// <summary>
    /// Hide help box.
    /// </summary>
    private void Hide()
    {
        fadeBack.FadeOut();
        fadeText.FadeOut();
    }
}
