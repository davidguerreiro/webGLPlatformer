using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsItem : MonoBehaviour
{
    public bool selected;

    [Header("Components")]
    public GameObject selectedIcon;

    private AudioComponent _audio;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Displays selected icon.
    /// </summary>
    /// <param name="playAudio">bool</param>
    public void SetSelected(bool playAudio = true)
    {
        Debug.Log("called");
        if (playAudio) {
            _audio.PlaySound(0);
        }

        selected = true;
        selectedIcon.SetActive(true);
    }

    /// <summary>
    /// Hide selected icon.
    /// </summary>
    public void UnSetSelected()
    {
        selected = false;
        selectedIcon.SetActive(false);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
