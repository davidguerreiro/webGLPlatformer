using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerezAppearsCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public PerezShootingAttack bossForCinematic;
    public Perez boss;
    public DialogueData dialogueData;

    private Coroutine _playingCinematic;

    /// <summary>
    /// Play Crusher enter scene cinematic.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playingCinematic == null)
        {
            _playingCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    /// <summary>
    /// Play boss cinematic routine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCinematicRoutine()
    {
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(2f);

        // boss appear.
        bossForCinematic.Show();
        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueData);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        // boss hide.
        bossForCinematic.Hide();
        yield return new WaitForSeconds(1.5f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
