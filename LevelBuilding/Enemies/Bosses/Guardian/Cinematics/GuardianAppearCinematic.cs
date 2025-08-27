using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAppearCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public Guardian boss;

    [Header("Dialogue Data")]
    public DialogueData guardian1EN;
    public DialogueData guardian2EN;
    public DialogueData guardian3EN;
    public DialogueData guardian1ES;
    public DialogueData guardian2ES;
    public DialogueData guardian3ES;
    public DialogueData ramiro1EN;
    public DialogueData ramiro2EN;
    public DialogueData ramiro1ES;
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
    /// Play boss cinematic at the start of the boss battle
    /// level coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");
        DarkPortal darkPortal = cinematicManager.objects.GetObject(0).GetComponent<DarkPortal>();

        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1f);

        darkPortal.Appear();
        yield return new WaitForSeconds(2f);

        boss.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);

        darkPortal.Dissapear();
        yield return new WaitForSeconds(1f);

        // play dialogue.
        DialogueData guardian1 = (lang == "english") ? guardian1EN: guardian1ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(guardian1);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData ramiro1 = (lang == "english") ? ramiro1EN : ramiro1ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiro1);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData guardian2 = (lang == "english") ? guardian2EN : guardian2ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(guardian2);

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

        DialogueData guardian3 = (lang == "english") ? guardian3EN : guardian3ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(guardian3);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
