using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTowerBossAppearCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public CannonTower bossLeft;
    public CannonTower bossRight;
    public DialogueData dialogueData;

    private Coroutine _playCinematic;

    /// <summary>
    /// Play boss cinematic.
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
        cinematicManager.sounds.PlayCinematicSound(0);
        StartCoroutine(bossLeft.ShowsUp());
        StartCoroutine(bossRight.ShowsUp());

        // TOOD: Add appear boss right.

        yield return new WaitForSeconds(2.5f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueData);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        bossLeft.StartBattle();
        bossRight.StartBattle();

        // TODO: Add start bttle for boss right.
    }
}
