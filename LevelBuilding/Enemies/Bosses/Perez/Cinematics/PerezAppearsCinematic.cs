using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerezAppearsCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public PerezShootingAttack bossForCinematic;
    public Perez boss;

    [Header("Dialogues")]
    public DialogueData perezAppearsEN;
    public DialogueData perezAppearsES;
    public DialogueData ramiroAnswersEN;
    public DialogueData ramiroAnswersES;
    public DialogueData perezEndEN;
    public DialogueData perezEndES;

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
        string lang = PlayerPrefs.GetString("language", "english");
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(2f);

        // boss appear.
        bossForCinematic.Show();
        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        DialogueData perezAppears = (lang == "english") ? perezAppearsEN : perezAppearsES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(perezAppears);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiroAnswers = (lang == "english") ? ramiroAnswersEN : ramiroAnswersES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroAnswers);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData perezAttacks = (lang == "english") ? perezEndEN: perezEndES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(perezAttacks);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);



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
