using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [Header("Components")]
    public GameObject background;

    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Display pause UI.
    /// </summary>
    public void Display()
    {
        _audio.PlaySound();
        background.SetActive(true);
    }

    /// <summary>
    /// Hide pause UI,
    /// </summary>
    public void Hide()
    {
        background.SetActive(false);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
