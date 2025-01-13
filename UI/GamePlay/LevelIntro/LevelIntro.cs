using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntro : MonoBehaviour
{
    [Header("Components")]
    public TextComponent levelText;
    public TextComponent introText;
    public FadeElement cover;
    public Image levelImage;

    [Header("Settings")]
    public float secondsDisplayed;
    public float secondsAfterDisplayed;

    [HideInInspector]
    public bool displayed;

    private LevelData _levelData;

    /// <summary>
    /// Set up level data on screen.
    /// </summary>
    private void SetUpData()
    {
        string lang = PlayerPrefs.GetString("language", "english");
        string introTextContent = (lang == "english") ? _levelData.levelIntro : _levelData.levelIntroEs;
        levelText.UpdateContent(_levelData.levelName);
        introText.UpdateContent(introTextContent);
        levelImage.sprite = _levelData.levelIcon;
    }

    /// <summary>
    /// Show level data.
    /// </summary>
    public void ShowLevelData()
    {
        StartCoroutine(ShowLevelDataRoutine());   
    }

    /// <summary>
    /// Show level data coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ShowLevelDataRoutine()
    {
        displayed = true;

        cover.FadeOut();
        yield return new WaitForSeconds(secondsDisplayed);

        cover.FadeIn();
        yield return new WaitForSeconds(secondsAfterDisplayed);

        cover.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        introText.gameObject.SetActive(false);
        levelImage.gameObject.SetActive(false);

        displayed = false;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="levelData">levelData</param>
    public void Init(LevelData levelData)
    {
        _levelData = levelData;
        SetUpData();
    }


}
