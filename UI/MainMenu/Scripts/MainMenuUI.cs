using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Components")]
    public FadeElement cover;
    public GameObject board;
    public GameObject gameTitle;
    public GameObject logoSection;
    public GameObject pressSpaceBarSection;

    [HideInInspector]
    public bool displayed;

    private AudioComponent _audio;

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
        cover.FadeOut();
        yield return new WaitForSeconds(1.5f);

        board.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        gameTitle.SetActive(true);
        _audio.PlaySound(0);
        yield return new WaitForSeconds(1f);

        logoSection.SetActive(true);
        pressSpaceBarSection.SetActive(true);

        displayed = true;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
