using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject hazard;
    public float timeBeforeShow;
    public float timeBeforeHide;

    private Coroutine _showSpikes;
    private AudioComponent _audio;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inGamePlay)
        {
            if (_showSpikes == null)
            {
                _showSpikes = StartCoroutine(ShowHideSpikes());
            }
        }
    }

    /// <summary>
    /// Show and hide spikes loop coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ShowHideSpikes()
    {
        yield return new WaitForSeconds(timeBeforeShow);
        _anim.SetBool("Show", true);
        _audio.PlaySound();
        hazard.tag = "Hazard";

        yield return new WaitForSeconds(timeBeforeHide);
        _anim.SetBool("Show", false);
        hazard.tag = "Untagged";

        _showSpikes = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _anim = GetComponent<Animator>();
    }
}
