using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World3IntroCinematic : MonoBehaviour
{
    public string sceneToLoadAfterCinematic;

    [Header("Components")]
    public CinematicManager cinematicManager;
    public DarkPortal darkPortalRight;
    public DarkPortal darkPortalLeft;
    public GameObject falseijo;

    [Header("Dialogue data en")]
    public DialogueData ramiroDialogue1EN;
    public DialogueData ramiroDialogue2EN;
    public DialogueData ramiroDialogue3EN;
    public DialogueData ramiroDialogue4EN;
    public DialogueData falseijoDialogue1EN;
    public DialogueData falseijoDialogue2EN;
    public DialogueData falseijoDialogue3EN;

    [Header("Dialogue data es")]
    public DialogueData ramiroDialogue1ES;
    public DialogueData ramiroDialogue2ES;
    public DialogueData ramiroDialogue3ES;
    public DialogueData ramiroDialogue4ES;
    public DialogueData falseijoDialogue1ES;
    public DialogueData falseijoDialogue2ES;
    public DialogueData falseijoDialogue3ES;

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
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1f);
        cinematicManager.sounds.PlayCinematicSound(0, true);

        string lang = PlayerPrefs.GetString("language", "english");

        yield return new WaitForSeconds(1.5f);

        darkPortalRight.Appear();
        yield return new WaitForSeconds(.5f);
        falseijo.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        darkPortalRight.Dissapear();

        Actor falseijoActor = falseijo.GetComponent<Actor>();

        yield return new WaitForSeconds(1f);

        DialogueData ramiroDialogue1 = (lang == "english") ? ramiroDialogue1EN : ramiroDialogue1ES;
        DialogueData ramiroDialogue2 = (lang == "english") ? ramiroDialogue2EN : ramiroDialogue2ES;
        DialogueData ramiroDialogue3 = (lang == "english") ? ramiroDialogue3EN : ramiroDialogue3ES;
        DialogueData ramiroDialogue4 = (lang == "english") ? ramiroDialogue4EN : ramiroDialogue4ES;

        DialogueData falseijoDialogue1 = (lang == "english") ? falseijoDialogue1EN : falseijoDialogue1ES;
        DialogueData falseijoDialogue2 = (lang == "english") ? falseijoDialogue2EN : falseijoDialogue2ES;
        DialogueData falseijoDialogue3 = (lang == "english") ? falseijoDialogue3EN : falseijoDialogue3ES;

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue1, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);
        falseijoActor.DisableAnimComponent();
        falseijoActor.ChangeSprite(1);
        yield return new WaitForSeconds(.5f);
        falseijoActor.ChangeSprite(0);
        falseijoActor.FlipXAxis(true);

        yield return new WaitForSeconds(1f);
        falseijoActor.EnableAnimComponent();

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(falseijoDialogue1, false);

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

        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(falseijoDialogue2, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue3, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.sounds.PlayCinematicSound(1, true);
        falseijoActor.MoveActor(0);

        while (falseijoActor.isMoving)
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

        darkPortalLeft.Appear();
        yield return new WaitForSeconds(1f);
        falseijo.SetActive(false);
        yield return new WaitForSeconds(1f);
        darkPortalLeft.Dissapear();

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue4, false);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneToLoadAfterCinematic);
    }
}
