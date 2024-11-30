using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextComponent))]
public class LanguageLabel : MonoBehaviour
{
    public string englishLabel;
    public string spanishLabel;

    private TextComponent _textComponent;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        DisplayLabel();
    }

    /// <summary>
    /// Update label in text content based on language
    /// player settings.
    /// </summary>
    public void DisplayLabel()
    {
        string lang = PlayerPrefs.GetString("language", "english");
        string content = (lang == "english") ? englishLabel : spanishLabel;

        _textComponent.UpdateContent(content);
    }


    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _textComponent = GetComponent<TextComponent>();
    }
}
