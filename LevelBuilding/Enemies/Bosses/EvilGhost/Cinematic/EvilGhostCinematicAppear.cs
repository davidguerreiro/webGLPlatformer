using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGhostCinematicAppear : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public EvilGhost boss;

    [Header("Dialogue data en")]
    public DialogueData ramiroDialogue1EN;
    public DialogueData ramiroDialogue2EN;

    public DialogueData falseijoDialogue1EN;
    public DialogueData falseijoDialogue2EN;
    public DialogueData falseijoDialogue3EN;

    [Header("Dialogue data es")]
    public DialogueData ramiroDialogue1ES;
    public DialogueData ramiroDialogue2ES;

    public DialogueData falseijoDialogue1ES;
    public DialogueData falseijoDialogue2ES;
    public DialogueData falseijoDialogue3ES;

    private Coroutine _playCinematic;

    /// <summary>
    /// Play boss cinematic at the start of the boss
    /// battle level.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playCinematic == null)
        {
            _playCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }
    
    /// <summary>
    /// Play boss cinematic at the start of the boss
    /// battle level coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        DarkPortal villainDarkPortal = cinematicManager.objects.GetObject(0).GetComponent<DarkPortal>();
        DarkPortal bossDarkPortal = cinematicManager.objects.GetObject(1).GetComponent<DarkPortal>();
        GameObject villain = cinematicManager.objects.GetObject(2);

        string lang = PlayerPrefs.GetString("language", "english");

        cinematicManager.gameManager.inGamePlay = false;

        DialogueData ramiroDialogue1 = (lang == "english") ? ramiroDialogue1EN : ramiroDialogue1ES;
        DialogueData ramiroDialogue2 = (lang == "english") ? ramiroDialogue2EN : ramiroDialogue2ES;

        DialogueData falseijoDialogue1 = (lang == "english") ? falseijoDialogue1EN : falseijoDialogue1ES;
        DialogueData falseijoDialogue2 = (lang == "english") ? falseijoDialogue2EN : falseijoDialogue2ES;
        DialogueData falseijoDialogue3 = (lang == "english") ? falseijoDialogue3EN : falseijoDialogue3ES;

        yield return new WaitForSeconds(1.5f);

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(falseijoDialogue1, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue1, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(falseijoDialogue2, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue2, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(falseijoDialogue3, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        villainDarkPortal.Appear();
        yield return new WaitForSeconds(1f);
        villain.SetActive(true);
        villain.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        villainDarkPortal.Dissapear();
        yield return new WaitForSeconds(1f);

        bossDarkPortal.Appear();
        yield return new WaitForSeconds(1f);

        boss.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        bossDarkPortal.Dissapear();

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
