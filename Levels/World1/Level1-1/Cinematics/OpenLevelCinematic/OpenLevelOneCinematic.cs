using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelOneCinematic : MonoBehaviour
{
    [Header("Components")]
    public CinematicManager cinematicManager;
    public DialogueData enDialogueData;
    public DialogueData esDialogueData;

    private Coroutine _playingCinematic;

    /// <summary>
    /// Play Crusher enter scene cinematic.
    /// </summary>
    public void PlayInitLevelCinematic()
    {
        if (_playingCinematic == null)
        {
            _playingCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    /// <summary>
    /// Play cinematic coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");
        DialogueData dialogueToPlay = (lang == "english") ? enDialogueData : esDialogueData;

        cinematicManager.gameManager.player.playerController.RestrictPlayerInput();
        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueToPlay);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        // resume game.
        cinematicManager.gameManager.player.playerController.AllowPlayerInput();
    }
}
