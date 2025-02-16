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
    public DialogueData ramiroDialogueEn;
    public DialogueData[] scarDialogueEn;

    [Header("Dialogue data es")]
    public DialogueData ramiroDialogueEs;
    public DialogueData[] scarDialogueEs;

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
        DialogueData ramiroDialogue = (lang == "english") ? ramiroDialogueEn : ramiroDialogueEs;
        DialogueData[] scarDialogue = (lang == "english") ? scarDialogueEn : scarDialogueEs;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarDialogue[0], false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarDialogue[1], false);

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
