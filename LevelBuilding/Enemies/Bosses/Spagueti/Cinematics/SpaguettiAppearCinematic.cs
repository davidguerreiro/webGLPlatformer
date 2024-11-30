using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaguettiAppearCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public Spagueti boss;
    public DialogueData dialogueData;

    private Coroutine _playCinematic;

    /// <summary>
    /// Play boss cinematic at the start of the
    /// boss battle level.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playCinematic == null)
        {
            _playCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    /// <summary>
    /// Play cinematic routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1f);

        // appear boss.
        StartCoroutine(boss.ShowsUp());
        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueData);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        boss.IncreaseGoingDownSpeed();
        StartCoroutine(boss.GoDown());
        yield return new WaitForSeconds(.5f);
        boss.RestoreGoingDownSpeed();

        yield return new WaitForSeconds(.5f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
