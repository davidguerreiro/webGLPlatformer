using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaguettiAppearCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public Spagueti boss;

    [Header("DialogueData")]
    public DialogueData spagueti1EN;
    public DialogueData spagueti1ES;
    public DialogueData spagueti2EN;
    public DialogueData spagueti2ES;
    public DialogueData ramiro1EN;
    public DialogueData ramiro1ES;
    public DialogueData ramiro2EN;
    public DialogueData ramiro2ES;


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
        string lang = PlayerPrefs.GetString("language", "english");

        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1f);

        // appear boss.
        StartCoroutine(boss.ShowsUp());
        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        DialogueData spagueti1 = (lang == "english") ? spagueti1EN : spagueti1ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(spagueti1);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData ramiro1 = (lang == "english") ? ramiro1EN: ramiro1ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiro1);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData spagueti2 = (lang == "english") ? spagueti2EN : spagueti2ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(spagueti2);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData ramiro2 = (lang == "english") ? ramiro2EN : ramiro2ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiro2);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBox.Hide();
        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.Hide();

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
