using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryCloudAppearsCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public AngryCloud boss;
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
    /// Play boss cinematic routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1f);

        // thunder.
        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(3.5f);

        // appear boss.
        cinematicManager.objects.EnableObject(0);
        cinematicManager.sounds.PlayCinematicSound(1);
        yield return new WaitForSeconds(2f);
        boss.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueData);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
