using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class DialogueBox : MonoBehaviour
{
    [Header("Components")]
    public Animator background;
    public TextComponent text;
    public GameObject arrow;

    [Header("Settings")]
    public float displayAwait;
    public float betweenDialogueAwait;
    public float hideAwait;

    [HideInInspector]
    public Coroutine displaying;

    [HideInInspector]
    public Coroutine hidding;

    [HideInInspector]
    public Coroutine playingDialogue;

    [HideInInspector]
    public Coroutine playingFullDialogue;

    [HideInInspector]
    public Rewired.Player rewiredPlayer;

    private DialogueData _dialogue;
    private AudioComponent _audio;
    private bool _listeningForUserKey;

    private void Update()
    {
        // await user input to continue playing dialogue.
        if (_listeningForUserKey && (rewiredPlayer.GetButton("Jump") || rewiredPlayer.GetButton("Cancel")))
        {
            _listeningForUserKey = false;
        }    
    }

    /// <summary>
    /// Display dialogue box.
    /// </summary>
    /// <param name="dialogueData">DialogueData</param>
    public void Display(DialogueData dialogueData = null)
    {
        if (dialogueData != null)
        {
            SetDialogueData(dialogueData);
        }

        if (displaying == null)
        {
            displaying = StartCoroutine(DisplayRoutine());
        }
    }

    /// <summary>
    /// Display dialogue box.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator DisplayRoutine()
    {
        background.SetBool("Display", true);
        // _audio.PlaySound(0);

        yield return new WaitForSeconds(displayAwait);

        displaying = null;
    }

    /// <summary>
    /// Hide dialogue box.
    /// </summary>
    public void Hide()
    {
        if (hidding == null)
        {
            hidding = StartCoroutine(HideRoutine());
        }
    }

    /// <summary>
    /// Hide dialogue box routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator HideRoutine()
    {
        text.Clear();
        text.gameObject.SetActive(false);

        if (arrow.activeSelf)
        {
            arrow.SetActive(false);
        }

        background.SetBool("Display", false);
        // _audio.PlaySound(2);

        yield return new WaitForSeconds(hideAwait);

        hidding = null;
    }

    /// <summary>
    /// Play dialogue.
    /// </summary>
    public void PlayDialogue()
    {
        text.gameObject.SetActive(true);
        text.Clear();

        if (playingDialogue == null)
        {
            playingDialogue = StartCoroutine(PlayDialogueRoutine());
        }
    }

    /// <summary>
    /// Play dialogue coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PlayDialogueRoutine()
    {
        string[] dialogue = _dialogue.GetDialogue();

        for (int i = 0; i < dialogue.Length; i++)
        {
            // remove after first dialogue text.
            if (i > 0)
            {
                text.Clear();
                text.gameObject.SetActive(false);
                _audio.PlaySound(0);

                if (arrow.activeSelf)
                {
                    arrow.SetActive(false);
                }

                yield return new WaitForSeconds(betweenDialogueAwait);

                text.gameObject.SetActive(true);
            }

            // display current dialogue text.
            text.UpdateContent(dialogue[i]);
            yield return new WaitForSeconds(.1f);

            // display more text arrow if there are more text to display.
            if (i + 1 < dialogue.Length)
            {
                arrow.SetActive(true);
            }

            // await for user pressing space bar to continue playing dialogue.
            _listeningForUserKey = true;

            while (_listeningForUserKey)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        playingDialogue = null;
    }

    /// <summary>
    /// Play full dialogue routine.
    /// </summary>
    /// <param name="dialogueData"></param>
    public void PlayFullDialogue(DialogueData dialogueData)
    {
        if (playingFullDialogue == null)
        {
            playingFullDialogue = StartCoroutine(PlayFullDialogueRoutine(dialogueData));
        }
    }

    /// <summary>
    /// Play full dialogue.
    /// </summary>
    /// <param name="dialogueData">DialogueData</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayFullDialogueRoutine(DialogueData dialogueData)
    {
        Display(dialogueData);
        
        while (displaying != null)
        {
            yield return new WaitForFixedUpdate();
        }

        PlayDialogue();

        while (playingDialogue != null) 
        {
            yield return new WaitForFixedUpdate();
        }

        Hide();

        while (hidding != null)
        {
            yield return new WaitForFixedUpdate();
        }

        playingFullDialogue = null;
    }

    /// <summary>
    /// Set dialogue data.
    /// </summary>
    /// <param name="dialogueData"></param>
    public void SetDialogueData(DialogueData dialogueData)
    {
        _dialogue = dialogueData;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _audio = GetComponent<AudioComponent>();
        rewiredPlayer = ReInput.players.GetPlayer(0);
        _listeningForUserKey = false;
    }
}
