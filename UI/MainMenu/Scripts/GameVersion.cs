using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVersion : MonoBehaviour
{
    private TextComponent textComponent;

    private void OnEnable()
    {
        DisplayGameVersion();
    }

    /// <summary>
    /// Display game version.
    /// </summary>
    private void DisplayGameVersion()
    {
        textComponent = GetComponent<TextComponent>();

        if (textComponent != null)
        {
            textComponent.UpdateContent("V." + Application.version);
        }
    }
}
