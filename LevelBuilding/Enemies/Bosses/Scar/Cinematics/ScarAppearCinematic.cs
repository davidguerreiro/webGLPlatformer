using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarAppearCinematic : MonoBehaviour
{
    [Header("Components")]
    public CinematicManager cinematicManager;
    public Scar boss;
    public DarkPortal darkPortal;

    [Header("Dialogue data en")]
    public DialogueData ramiroDialogue1EN;
    public DialogueData ramiroDialogue2EN;
    public DialogueData scarDialogue1EN;
    public DialogueData scarDialogue2EN;
    public DialogueData scarDialogue3EN;

    [Header("Dialogue data es")]
    public DialogueData ramiroDialogue1ES;
    public DialogueData ramiroDialogue2ES;
    public DialogueData scarDialogue1ES;
    public DialogueData scarDialogue2ES;
    public DialogueData scarDialogue3ES;

    private Coroutine _playingCinematic;
       
    /// <summary>
    /// Play boss cinematic at the start of the boss
    /// battle level.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playingCinematic == null)
        {
            _playingCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }


    /// <summary>
    /// Play boss cinematic.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");

        cinematicManager.gameManager.inGamePlay = false;

        yield return new WaitForSeconds(1f);

        // thunder.
        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(2.5f);

        // appear boss.
        darkPortal.Appear();
        yield return new WaitForSeconds(.5f);
        boss.EnableBossSprite();
        yield return new WaitForSeconds(1.5f);
        darkPortal.Dissapear();

        // play dialogue.
        DialogueData ramiroDialogue1 = (lang == "english") ? ramiroDialogue1EN : ramiroDialogue1ES;
        DialogueData ramiroDialogue2 = (lang == "english") ? ramiroDialogue2EN : ramiroDialogue2ES;
        DialogueData scarDialogue1 = (lang == "english") ? scarDialogue1EN : scarDialogue1ES;
        DialogueData scarDialogue2 = (lang == "english") ? scarDialogue2EN : scarDialogue2ES;
        DialogueData scarDialogue3 = (lang == "english") ? scarDialogue3EN : scarDialogue3ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarDialogue1, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue1, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarDialogue2, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue2, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarDialogue3, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBox.Hide();
        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.Hide();

        yield return new WaitForSeconds(2f);

        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
